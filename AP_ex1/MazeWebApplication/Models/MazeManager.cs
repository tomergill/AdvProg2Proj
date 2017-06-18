using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MazeLib;
using MazeGeneratorLib;

namespace MazeWebApplication.Models
{
    public class MazeManager : IMazeManager
    {
        private static Dictionary<String, Maze> mazes = new Dictionary<string, Maze>();
        private IMazeGenerator generator = new DFSMazeGenerator();

        public Maze GetMaze(string name)
        {
            if (!mazes.ContainsKey(name))
                return null;
            return mazes[name];
        }

        public Maze GenerateMaze(string name, int rows, int cols)
        {
            if (mazes.ContainsKey(name)) //already has a maze by this name
                return null;

            if (rows <= 0 || rows > 100 || cols <= 0 || cols > 100) //size not supported
                return null;

            try
            {
                Maze m = generator.Generate(rows, cols);
                m.Name = name;
                mazes[name] = m;
                return m;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public IEnumerable<Maze> GetAllMazes()
        {
            return mazes.Values.ToList();
        }
    }
}