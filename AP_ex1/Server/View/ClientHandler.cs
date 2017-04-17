using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ClientHandler : IClientHandler
    {
        private bool stop = false;

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
                        string commandLine = reader.ReadString();
                        //Console.WriteLine("GOT " + commandLine);
                        string result = controller.ExecuteCommand(commandLine, out bool shouldCloseConnection, client);
                        if (result != null)
                        {
                            //Console.WriteLine("SEND " + result);
                            writer.Write(result);
                        }
                        else
                        {
                            writer.Write("ERROR");
                        }
                        if (shouldCloseConnection)
                        {
                            client.Close();
                            break;
                        }
                    }
                    client.Close();
                }
            });
            t.Start();
            //t.Wait();
        }

        public void Stop()
        {
            this.stop = true;
        }
    }
}
