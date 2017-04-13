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
            ObjectAdapter objadptr = new ObjectAdapter(myMaze);
            BfsAlgorithm<Position> BFSSearcher = new BfsAlgorithm<Position>();
            DfsAlgorithm<Position> DFSSearcher = new DfsAlgorithm<Position>();
            Solution<Position> solBFS = BFSSearcher.search(objadptr);
            if (solBFS == default(Solution<Position>))
                Console.WriteLine("no solution to this maze with BFS");
            else
                Console.WriteLine("number of states developed in BFS: " + BFSSearcher.getNumberOfNodesEvaluated());
            Solution<Position> solDFS = DFSSearcher.search(objadptr);
            if (solDFS == default(Solution<Position>))
                Console.WriteLine("no solution to this maze with DFS");
            else
                Console.WriteLine("number of states developed in DFS: " + DFSSearcher.getNumberOfNodesEvaluated());
        }

        static void Main(string[] args)
        {
            CompareSolvers();
        }
    }
}
