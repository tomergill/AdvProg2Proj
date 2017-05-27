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
    /// Closes a multiplayer game and notifies the other player.
    /// </summary>
    public class CloseGameCommand : AbstractCommand
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="model">Model the command may change or read from.</param>
        public CloseGameCommand(IModel model) : base(model)
        {
        }

        /// <summary>
        /// Closes a multiplayer game and notifies the other player.
        /// </summary>
        /// <param name="args">[name of the maze].</param>
        /// <param name="client">TcpClient to send data to. null if not specified</param>
        /// <returns>Empty string if succes, otherwise null.</returns>
        public override string Execute(string[] args, out bool shouldCloseConnection, TcpClient client = null, BinaryWriter writer = null)
        {
            shouldCloseConnection = true;
            if (args.Length != 1)
                return null;
            MultiplayerGame game = model.CloseGame(args[0]);
            if (game == null)
                return null;

            //send to other client
            TcpClient other = game.GetOtherPlayer(client);
            BinaryWriter writer2 = game.GetPlayersWriter(other);
            if (writer2 == null)
                return null;
            try
            {
                writer2.Write("CLOSED " + game.GetMazeName());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return "";
        }
    }
}
