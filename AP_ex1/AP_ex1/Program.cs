using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeLib;
using MazeGeneratorLib;

namespace AP_ex1
{
    class Program
    {
        public static void CompareSolvers()
        {
            DFSMazeGenerator myMazeGenerator = new DFSMazeGenerator();
            Maze myMaze = myMazeGenerator.Generate(50, 50);
            Console.WriteLine(myMaze.ToString());
            Console.WriteLine("*******************************************");
            ObjectAdapter objectAdapter = new ObjectAdapter(myMaze);
            BfsAlgorithm<Position> bfsSearcher = new BfsAlgorithm<Position>();
            DfsAlgorithm<Position> dfsSearcher = new DfsAlgorithm<Position>();
            Solution<Position> bfsSolution = bfsSearcher.Search(objectAdapter);
            if (bfsSolution == default(Solution<Position>))
                Console.WriteLine("no solution to this maze with BFS");
            else
                Console.WriteLine("number of states developed in BFS: " + bfsSearcher.GetNumberOfNodesEvaluated());
            Solution<Position> dfsSolution = dfsSearcher.Search(objectAdapter);
            if (dfsSolution == default(Solution<Position>))
                Console.WriteLine("no solution to this maze with DFS");
            else
                Console.WriteLine("number of states developed in DFS: " + dfsSearcher.GetNumberOfNodesEvaluated());
        }

        static void Main(string[] args)
        {
            CompareSolvers();
        }
    }
}
