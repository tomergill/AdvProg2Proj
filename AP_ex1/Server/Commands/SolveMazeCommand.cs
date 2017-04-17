using MazeLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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
            Maze m = model.GetMazeByName(args[0]);
            if (null == m)
                return null;
            if (int.Parse(args[1]) == 1)
            {
                //DFS
            }
            else
            {
                //BFS
            }
            //TODO after lotan is finished, finish this method.
            return "";
        }
    }
}
