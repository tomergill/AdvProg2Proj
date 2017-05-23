using MazeLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    /// <summary>
    /// Command that generates a maze.
    /// </summary>
    public class GenerateMazeCommand : AbstractCommand
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="model">Model the command may change or read from.</param>
        public GenerateMazeCommand(IModel model) : base(model)
        { }

        /// <summary>
        /// Generates a Maze, keeps it, send the JSON description of it to the client and return the JSON.
        /// </summary>
        /// <param name="args">[name of the maze, number of rows, number of columns].</param>
        /// <param name="client">TcpClient to send data to. null if not specified</param>
        /// <returns>The new maze JSON representation.</returns>
        public override string Execute(string[] args, out bool shouldCloseConnection, TcpClient client = null, BinaryWriter writer = null)
        {
            shouldCloseConnection = true;
            if (args.Length != 3)
                return null;
            string name = args[0];
            int rows = int.Parse(args[1]);
            int cols = int.Parse(args[2]);
            Maze maze = model.GenerateMaze(name, rows, cols);
            if (maze == null)
                return "ERROR - maze with this name already exist";
            ////sends to client maze's JSON representation.
            //if (client != null && client.Connected)
            //{
            //    using (NetworkStream stream = client.GetStream())
            //    using (BinaryWriter writer = new BinaryWriter(stream))
            //    {
            //        writer.Write(maze.ToJSON());
            //    }
            //    client.Close();
            //}
            Console.WriteLine(maze);
            return maze.ToJSON();
        }
    }
}
