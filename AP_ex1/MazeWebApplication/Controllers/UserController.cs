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
    public class UserController : ApiController
    {
        private MazeWebApplicationContext db = new MazeWebApplicationContext();

        // GET: api/User
        public IQueryable<User> GetUsers()
        {
            return db.Users;
        }

        // GET: api/User/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> GetUser(string id,string passWord)
        {
            User user = await db.Users.FindAsync(id);
            if (user == null || user.Password != ComputeHash(passWord))
            {
                return NotFound();
            }
            return Ok(user);
        }

        // PUT: api/User/5
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(string id)
        {
            return db.Users.Count(e => e.UserName == id) > 0;
        }

        string ComputeHash(string input)
        {
            SHA1 sha = SHA1.Create();
            byte[] buffer = Encoding.ASCII.GetBytes(input);
            byte[] hash = sha.ComputeHash(buffer);
            string hash64 = Convert.ToBase64String(hash);
            return hash64;
        }
    }
}