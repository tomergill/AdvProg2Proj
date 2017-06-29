using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MazeWebApplication.Models;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MazeWebApplication.Controllers
{
    /// <summary>
    /// Controller that handles the users DB.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class UserController : ApiController
    {
        /// <summary>
        /// The database.
        /// </summary>
        private MazeWebApplicationContext db = new MazeWebApplicationContext();

        // GET: api/User
        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <returns>All the users ordered by the number of wins minus loses</returns>
        public IQueryable<User> GetUsers()
        {
            return db.Users.OrderByDescending(e => e.Wins - e.Losses);
        }

        // GET: api/User/5
        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="passWord">The pass word.</param>
        /// <returns>The requested user.</returns>
        [Route("api/User/{id}/{passWord}")]
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> GetUser(string id, string passWord)
        {
            User user = await db.Users.FindAsync(id);
            if (user == null || user.Password != ComputeHash(passWord))
            {
                return NotFound();
            }
            return Ok(user);
        }

        // PUT: api/User/5
        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="user">The updated user.</param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUser(string id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.UserName)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/User
        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="user">The new user.</param>
        /// <returns></returns>
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> PostUser([FromBody]JObject user)
        {
            string userName = user["UserName"].ToObject<string>();
            string password = user["Password"].ToObject<string>();
            int wins = user["Wins"].ToObject<int>();
            int losses = user["Losses"].ToObject<int>();
            string email = user["EmailAdress"].ToObject<string>();

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest(ModelState);
            }

            User newUser = new User();
            newUser.UserName = userName;
            newUser.Password = ComputeHash(password);
            newUser.Wins = wins;
            newUser.Losses = losses;
            newUser.EmailAdress = email;
            newUser.FirstSignedIn = DateTime.Today;

            db.Users.Add(newUser);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserExists(newUser.UserName))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = newUser.UserName }, newUser);
        }

        // DELETE: api/User/5
        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> DeleteUser(string id)
        {
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            await db.SaveChangesAsync();

            return Ok(user);
        }

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// returns wether a user with this id exists or not.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        private bool UserExists(string id)
        {
            return db.Users.Count(e => e.UserName == id) > 0;
        }

        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        string ComputeHash(string input)
        {
            SHA1 sha = SHA1.Create();
            byte[] buffer = Encoding.ASCII.GetBytes(input);
            byte[] hash = sha.ComputeHash(buffer);
            string hash64 = Convert.ToBase64String(hash);
            return hash64;
        }

        // POST: api/User
        /// <summary>
        /// Updates the rank.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="won">if set to <c>true</c> the user won, otherwise lost.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/User/{id}/{won}")]
        public async Task<IHttpActionResult> UpdateRank(string id, bool won)
        {
            User user = await db.Users.FindAsync(id);
            if (user == null)
                return BadRequest();
            if (won)
                user.Wins++;
            else
                user.Losses++;

            await db.SaveChangesAsync();
            return Ok();
        }
    }
}