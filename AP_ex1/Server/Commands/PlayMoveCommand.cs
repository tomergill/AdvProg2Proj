using Newtonsoft.Json.Linq;
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
    /// Command that notifies the rival of the player's move.
    /// </summary>
    public class PlayMoveCommand : AbstractCommand
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="model">Model the command may change or read from.</param>
        public PlayMoveCommand(IModel model) : base(model)
        {
        }

        /// <summary>
        /// Notifies the rival of the move made.
        /// </summary>
        /// <param name="args">[move direction].</param>
        /// <param name="client">TcpClient to send data to. null if not specified</param>
        /// <returns>Empty string if success, null otherwise.</returns>
        public override string Execute(string[] args, out bool shouldCloseConnection, TcpClient client = null)
        {
            shouldCloseConnection = false;
            if (client == null || args.Length != 1)
                return null;
            string move = args[0];
            string[] directions = { "up", "down", "right", "left" };
            if (!directions.Contains(move))
                return null;
            MultiplayerGame game = model.GetMultiplayerGameByPlayer(client);
            if (game == null)
                return null;
            JObject json = new JObject
            {
                ["Name"] = game.GetMazeName(),
                ["Direction"] = move
            };

            //send to other client
            TcpClient other = game.GetOtherPlayer(client);
            if (other == null)
                return null;
            using (NetworkStream stream = other.GetStream())
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(json.ToString());
            }
            return "";
        }
    }
}
