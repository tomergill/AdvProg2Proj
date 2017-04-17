using Newtonsoft.Json;
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
    /// Lists all the open games to join.
    /// </summary>
    public class ListCommand : AbstractCommand
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="model">Model the command may change or read from.</param>
        public ListCommand(IModel model) : base(model)
        {
        }

        /// <summary>
        /// Lists all the open games to join.
        /// </summary>
        /// <param name="args">None.</param>
        /// <param name="client">TcpClient to send data to. null if not specified</param>
        /// <returns>JSON represention of the open game list.</returns>
        public override string Execute(string[] args, out bool shouldCloseConnection, TcpClient client = null)
        {
            shouldCloseConnection = true;
            if (args.Length != 0)
                return null;
            List<string> openGames = model.GetOpenGamesList();
            string json = JsonConvert.SerializeObject(openGames, Formatting.Indented);

            ////sends to client
            //if (client != null && client.Connected)
            //{
            //    using (NetworkStream stream = client.GetStream())
            //    using (BinaryWriter writer = new BinaryWriter(stream))
            //    {
            //        writer.Write(json);
            //    }
            //}
            return json;
        }
    }
}
