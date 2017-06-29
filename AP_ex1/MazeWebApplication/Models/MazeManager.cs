using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MazeLib;
using MazeGeneratorLib;
using SearchAlgorithmsLib;

namespace MazeWebApplication.Models
{
    /// <summary>
    /// Manager of singleplayer games.
    /// </summary>
    /// <seealso cref="MazeWebApplication.Models.IMazeManager" />
    public class MazeManager : IMazeManager
    {
        /// <summary>
        /// Holds the mazes
        /// </summary>
        private static IDictionary<String, Maze> mazes = new Dictionary<string, Maze>();

        /// <summary>
        /// The maze generator
        /// </summary>
        private IMazeGenerator generator = new DFSMazeGenerator();

        /// <summary>
        /// The solutions' dictionary.
        /// </summary>
        private static Dictionary<String, List<Position>> solutions = 
            new Dictionary<string, List<Position>>();


        /// <summary>
        /// Gets the maze.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public Maze GetMaze(string name)
        {
            if (!mazes.ContainsKey(name))
                return null;
            return mazes[name];
        }

        /// <summary>
        /// Generates a maze.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="rows">The rows.</param>
        /// <param name="cols">The cols.</param>
        /// <returns>The maze</returns>
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

        /// <summary>
        /// Gets all mazes.
        /// </summary>
        /// <returns>List of all mazes</returns>
        public IEnumerable<Maze> GetAllMazes()
        {
            return mazes.Values.ToList();
        }

        /// <summary>
        /// Solutions to list.
        /// </summary>
        /// <param name="sol">The sol.</param>
        /// <returns></returns>
        private List<Position> SolutionToList(Solution<Position> sol)
        {
            if (sol == null)
                return null;
            State<Position> node;
            List<Position> list = new List<Position>();
            while ((node = sol.Pop()) != null)
            {
                list.Add(node.GetState());
            }
            return list;
        }

        /// <summary>
        /// Gets the solution to the maze.
        /// </summary>
        /// <param name="name">The name of the maze.</param>
        /// <param name="algoId">The algo identifier.</param>
        /// <returns>The solution</returns>
        public IEnumerable<Position> GetSolution(string name, int algoId)
        {
            if (solutions.ContainsKey(name))
                return solutions[name];
            
            if (!mazes.ContainsKey(name))
                return null;
            Maze maze = mazes[name];
            Solution<Position> temp;
            if (algoId == 0) //BFS
            {
                BfsAlgorithm<Position> bfs = new BfsAlgorithm<Position>();
                temp = bfs.Search(new ObjectAdapter(maze));
            }
            else //DFS  
            {
                DfsAlgorithm<Position> dfs = new DfsAlgorithm<Position>();
                temp = dfs.Search(new ObjectAdapter(maze));
            }
            solutions[name] = SolutionToList(temp);
            return solutions[name];
        }
    }
}