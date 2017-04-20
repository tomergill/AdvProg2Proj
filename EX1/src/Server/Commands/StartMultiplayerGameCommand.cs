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
    /// Creates a maze and waits for another player to join before sending the maze info.
    /// </summary>
    public class StartMultiplayerGameCommand : AbstractCommand
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="model">Model the command may change or read from.</param>
        public StartMultiplayerGameCommand(IModel model) : base(model)
        {
        }

        /// <summary>
        /// Creates the maze, and then waits to other player to join game before sending the client the info.
        /// </summary>
        /// <param name="args">[name of the maze, algorithm (0 for BFS, 1 for DFS)].</param>
        /// <param name="client">TcpClient to send data to. null if not specified</param>
        /// <returns>The maze info, or null if there was an error.</returns>
        public override string Execute(string[] args, out bool shouldCloseConnection, TcpClient client = null, BinaryWriter writer = null)
        {
            shouldCloseConnection = false;
            if (client == null || args.Length != 3)
                return null;
            string name = args[0];
            int rows = int.Parse(args[1]);
            int cols = int.Parse(args[2]);
            MultiplayerGame game = model.AddMultiplayerGame(client, writer, name, rows, cols);
            if (game == null)
                return null;
            while (game.IsJoinable()) { }
            ////sends to client maze's JSON representation.
            //if (client != null && client.Connected)
            //{
            //    using (NetworkStream stream = client.GetStream())
            //    using (BinaryWriter writer = new BinaryWriter(stream))
            //    {
            //        writer.Write(game.Maze.ToJSON());
            //    }
            //}
            return game.Maze.ToJSON();
        }
    }
}
