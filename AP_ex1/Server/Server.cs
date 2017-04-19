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
    /// <summary>
    /// View of MVC server.
    /// </summary>
    /// <seealso cref="Server.IView" />
    public class Server : IView
    {
        /// <summary>
        /// The port of the server.
        /// </summary>
        private int port;

        /// <summary>
        /// The listener for new clients.
        /// </summary>
        private TcpListener listener;

        /// <summary>
        /// The ip address of the server.
        /// </summary>
        private string ipAddr;

        /// <summary>
        /// The controller of the mvc server.
        /// </summary>
        private IController controller;

        /// <summary>
        /// The client handler.
        /// </summary>
        private IClientHandler ch;

        /// <summary>
        /// True if should stop.
        /// </summary>
        private bool stop;

        /// <summary>
        /// Initializes a new instance of the <see cref="Server"/> class.
        /// </summary>
        /// <param name="port">The port of the server.</param>
        /// <param name="ipAddr">The ip address of the server.</param>
        /// <param name="controller">The controller of the mvc server.</param>
        /// <param name="ch">The client handler.</param>
        public Server(int port, string ipAddr, IController controller, IClientHandler ch)
        {
            this.port = port;
            this.ipAddr = ipAddr;
            this.controller = controller;
            this.ch = ch;
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
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

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            ch.Stop();
            this.stop = true;
        }

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
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
