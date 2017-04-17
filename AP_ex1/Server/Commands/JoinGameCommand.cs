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
    /// Joins an open game and sends back the info.
    /// </summary>
    public class JoinGameCommand : AbstractCommand
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="model">Model the command may change or read from.</param>
        public JoinGameCommand(IModel model) : base(model)
        {
        }

        /// <summary>
        /// Joins the requested game if exist, and informs both players the maze info.
        /// </summary>
        /// <param name="args">Name of the game.</param>
        /// <param name="client">TcpClient to send data to. null if not specified</param>
        /// <returns>JSON represention of the game, or null.</returns>
        public override string Execute(string[] args, out bool shouldCloseConnection, TcpClient client)
        {
            shouldCloseConnection = false;
            if (args.Length != 1)
                return null;
            List<string> openGames = model.GetOpenGamesList();
            if (client == null || !openGames.Contains(args[0]))
                return null;
            MultiplayerGame game = model.JoinMultiplayerGame(args[0], client);
            if (game == null)
                return null;
            ////sends to client maze's JSON representation.
            //if (client != null && client.Connected)
            //{
            //    using (NetworkStream stream = client.GetStream())
            //    using (BinaryWriter writer = new BinaryWriter(stream))
            //    {
            //        writer.Write(maze.ToJSON());
            //    }
            //}

            return game.Maze.ToJSON();
        }
    }
}
