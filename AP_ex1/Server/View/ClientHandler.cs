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
    /// Handles a client.
    /// </summary>
    /// <seealso cref="Server.IClientHandler" />
    public class ClientHandler : IClientHandler
    {
        /// <summary>
        /// True if should stop.
        /// </summary>
        private bool stop = false;

        /// <summary>
        /// Handles the client.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="controller">The controller.</param>
        public void HandleClient(TcpClient client, IController controller)
        {
            Task t = new Task(() =>
            {
                using (NetworkStream stream = client.GetStream())
                using (BinaryReader reader = new BinaryReader(stream))
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    while (!stop && client.Connected)
                    {
                        string commandLine;
                        try
                        {
                            commandLine = reader.ReadString();

                            Console.WriteLine("GOT " + commandLine);
                            string result = controller.ExecuteCommand(commandLine, out bool shouldCloseConnection, client, writer);
                            if (result != null)
                            {
                                Console.WriteLine("SEND " + result);
                                if (client.Connected)
                                    writer.Write(result);
                            }
                            else
                            {
                                if (client.Connected)
                                    writer.Write("ERROR");
                            }
                            if (shouldCloseConnection)
                            {
                                client.Close();
                                break;
                            }
                        }
                        catch (Exception e)
                        {
                            //Console.WriteLine(e);
                            //Console.WriteLine(e.Message);
                            continue;
                        }
                    }
                    client.Close();
                }
            });
            t.Start();
            //t.Wait();
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            this.stop = true;
        }
    }
}
