using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGeneratorLib;
using MazeLib;

namespace MazeWebApplication.Models
{
    /// <summary>
    /// Interface of a manger for singleplayer games.
    /// </summary>
    public interface IMazeManager
    {
        /// <summary>
        /// Generates a maze.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="rows">The rows.</param>
        /// <param name="cols">The cols.</param>
        /// <returns>The maze</returns>
        Maze GenerateMaze(String name, int rows, int cols);

        /// <summary>
        /// Gets all mazes.
        /// </summary>
        /// <returns>List of all mazes</returns>
        IEnumerable<Maze> GetAllMazes();

        /// <summary>
        /// Gets the solution to the maze.
        /// </summary>
        /// <param name="name">The name of the maze.</param>
        /// <param name="algoId">The algo identifier.</param>
        /// <returns>The solution</returns>
        IEnumerable<Position> GetSolution(string name, int algoId);        
    }
}
