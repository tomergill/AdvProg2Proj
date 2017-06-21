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
    public class MazeController : ApiController
    {
        private static IMazeManager manager = new MazeManager();

        // GET: api/Maze
        [HttpGet]
        public IEnumerable<JObject> GetAllMazes()
        {
            IEnumerable<Maze> mazes = manager.GetAllMazes();
            List<JObject> list = new List<JObject>();
            foreach (Maze maze in mazes)
            {
                list.Add(JObject.Parse(maze.ToJSON()));
            }
            return list;
        }

        //// GET: api/Maze/5  
        //[HttpGet]
        //public JObject GetMaze(string name)
        //{
        //    return JObject.Parse(manager.GetMaze(name).ToJSON());
        //}

        // GET : api/Maze/mymaze?algoId=0
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

        // PUT: api/Maze/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Maze/5
        public void Delete(int id)
        {
        }
    }
}
