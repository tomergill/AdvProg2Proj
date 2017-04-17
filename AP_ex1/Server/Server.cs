using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Configuration;
using MazeGeneratorLib;

namespace Server
{
    public class Server : IView
    {
        private int port;
        private TcpListener listener;
        private string ipAddr;
        private IController controller;
        private IClientHandler ch;
        private bool stop;

        //int.Parse(ConfigurationManager.AppSettings["port"])

        public Server(int port, string ipAddr, IController controller, IClientHandler ch)
        {
            this.port = port;
            this.ipAddr = ipAddr;
            this.controller = controller;
            this.ch = ch;
        }

        public void Start()
        {
            this.stop = false;
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ipAddr), port);
            listener = new TcpListener(ep);

            listener.Start();
            //Console.WriteLine("Starting listening");
            Task receiveConnections = new Task(() =>
            {
                while(!stop)
                {
                    try
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        //Console.WriteLine("Accepted Connection");
                        ch.HandleClient(client, controller);
                    }
                    catch (SocketException e)
                    {
                        Console.WriteLine("Socket Exception: " + e.Message);
                        break;
                    }
                }
            });
            receiveConnections.Start();
        }

        public void Stop()
        {
            ch.Stop();
            this.stop = true;
        }

        public static void Main()
        {
            int port = int.Parse(ConfigurationManager.AppSettings["port"]);
            string ip = "127.0.0.1";
            IMazeGenerator generator = new DFSMazeGenerator();
            IModel model = new Model(generator);
            IController controller = new Controller(model);
            IClientHandler ch = new ClientHandler();
            Server s = new Server(port, ip, controller, ch);
            s.Start();
            while (true)
            {
                if (Console.ReadLine() == "CLOSE")
                {
                    s.Stop();
                    break;
                }
            }
            System.Environment.Exit(0);
        }
    }
}
