using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGeneratorLib;
using MazeLib;

namespace MazeWebApplication.Models
{
    public interface IMazeManager
    {
        Maze GenerateMaze(String name, int rows, int cols);
        //Maze GetMaze(string name);
        IEnumerable<Maze> GetAllMazes();
        IEnumerable<Position> GetSolution(string name, int algoId);
    }
}
