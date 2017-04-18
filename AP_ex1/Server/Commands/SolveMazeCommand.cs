using MazeLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SearchAlgorithmsLib;
using Newtonsoft.Json.Linq;

namespace Server
{
    /// <summary>
    /// Command that solves the requested maze.
    /// </summary>
    public class SolveMazeCommand : AbstractCommand
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="model">Model the command may change or read from.</param>
        public SolveMazeCommand(IModel model) : base(model)
        {
        }

        /// <summary>
        /// Solves the requested maze, using the requested algorithm and sends the client the solution.
        /// </summary>
        /// <param name="args">[name of the maze, algorithm (0 for BFS, 1 for DFS)].</param>
        /// <param name="client">TcpClient to send data to. null if not specified</param>
        /// <returns>The JSON representation of the solution of the maze, or null if something went wrong.</returns>
        public override string Execute(string[] args, out bool shouldCloseConnection, TcpClient client = null)
        {
            shouldCloseConnection = true;
            if (args.Length != 2)
                return null;
            SolutionWithNodesEvaluated<Position> ret = model.SolveMaze(args[0], int.Parse(args[1]));
            Solution<Position> sol = ret.Solution;
            JObject solve = new JObject();
            
                solve["Name"] = args[0];
            string jsonSolution = "";
            State<Position> father, son;
            father = sol.Pop();
            son = sol.Pop();
            while (father != null && son != null)
            {
                Position f = father.GetState();
                Position s = son.GetState();
                if (f.Col > s.Col)
                {
                    jsonSolution += "0"; //left
                }
                else if (f.Col < s.Col)
                {
                    jsonSolution += "1"; //right
                }
                else if (f.Row > s.Row)
                {
                    jsonSolution += "2"; //up
                }
                else //f.Row < s.Row
                {
                    jsonSolution += "3"; //down
                }
                father = son;
                son = sol.Pop();
            }
            solve["Solution"] = jsonSolution;
            solve["NodesEvaluated"] = ret.NodesEvaluated;

            return solve.ToString();
        }
    }
}
