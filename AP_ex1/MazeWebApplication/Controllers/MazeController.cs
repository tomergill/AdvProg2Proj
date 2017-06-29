using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MazeWebApplication.Models;
using MazeLib;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace MazeWebApplication.Controllers
{
    /// <summary>
    /// Controller for the singleplayer maze game.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class MazeController : ApiController
    {
        /// <summary>
        /// The manager of the mazes.
        /// </summary>
        private static IMazeManager manager = new MazeManager();

        // GET : api/Maze/mymaze?algoId=0
        /// <summary>
        /// Gets the solution to the maze.
        /// </summary>
        /// <param name="name">The name of the maze.</param>
        /// <param name="algoId">algorithm identifier.</param>
        /// <returns>A JSON representation of the list of solution's positions.</returns>
        [HttpGet]
        public JArray GetSolution(string name, int algoId)
        {
            algoId = (algoId >= 0 && algoId <= 1) ? algoId : algoId % 2;
            IEnumerable<Position> ie = manager.GetSolution(name, algoId);
            if (ie == null)
                return null;
            return JArray.FromObject(ie);
        }

        // GET: api/Maze/mymaze?rows=5&cols=6
        /// <summary>
        /// Generates a maze.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="rows">The rows.</param>
        /// <param name="cols">The cols.</param>
        /// <returns>JSON representation of a maze.</returns>
        [HttpGet]
        public JObject GenerateMaze(string name, int rows, int cols)
        {
            try
            {
                return JObject.Parse(manager.GenerateMaze(name, rows, cols).ToJSON());
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
