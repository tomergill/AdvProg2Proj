using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json;
using System.IO;
using MazeLib;
using Newtonsoft.Json.Linq;

namespace Client
{
    /// <summary>
    /// Handles a client of the maze game.
    /// </summary>
    public class Client
    {
        /// <summary>
        /// The server Ip and port.
        /// </summary>
        private IPEndPoint server;

        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        /// <param name="ip">The ip of the server.</param>
        /// <param name="port">The port of the server.</param>
        public Client(string ip, int port)
        {
            server = new IPEndPoint(IPAddress.Parse(ip), port);
        }

        /// <summary>
        /// Handles the command.
        /// </summary>
        /// <param name="commandLine">The command line.</param>
        public void HandleCommand(string commandLine)
        {
            string[] singleplayer = { "generate", "solve", "list" };
            string[] startMultiplayer = { "start", "join" };
            String[] split = commandLine.Split(' ');

            if (!startMultiplayer.Contains(split[0]) && !singleplayer.Contains(split[0]))
            {
                Console.WriteLine("ERROR WITH COMMAND");
                return;
            }

            TcpClient serverSocket = new TcpClient();
            serverSocket.Connect(server);

            while (!serverSocket.Connected) ;

            using (NetworkStream stream = serverSocket.GetStream())
            using (BinaryReader reader = new BinaryReader(stream))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {


                writer.Write(commandLine);

                /*If command is either*/
                if (startMultiplayer.Contains(split[0]))
                {
                    string res = MultiplayerGame(reader, writer, split[0]);
                    serverSocket.Close();
                    if (res != "")
                    {
                        if (res == "CLOSE")
                            System.Environment.Exit(0);
                        else
                        {
                            HandleCommand(res);
                        }
                    }
                }
                else
                {
                    /*Hendles single player commands*/
                    /*Generate maze command*/
                    string input = reader.ReadString();
                    //Console.WriteLine("GOT " + input);

                    if (input == "ERROR")
                        Console.WriteLine("ERROR");
                    else if (split[0] == "generate")
                    {
                        Maze maze = Maze.FromJSON(input);
                        Console.WriteLine(/*maze*/input);
                    }
                    else if (split[0] == "solve")
                    {
                        //JObject solution = JObject.Parse(input);
                        //Console.WriteLine($"Solution of {solution["Name"]} takes {solution["NodesEvaluated"]} steps:");
                        //string sol = (string)solution["Solution"];
                        //for (int i = 0; i < sol.Length - 1; i++)
                        //{
                        //    string si;
                        //    switch (sol[i])
                        //    {
                        //        case '0': si = "Left"; break;
                        //        case '1': si = "Right"; break;
                        //        case '2': si = "Up"; break;
                        //        default: si = "Down"; break;
                        //    }
                        //    Console.Write(si + ", ");
                        //}
                        //string s;
                        //switch (sol[sol.Length - 1])
                        //{
                        //    case '0': s = "Left"; break;
                        //    case '1': s = "Right"; break;
                        //    case '2': s = "Up"; break;
                        //    default: s = "Down"; break;
                        //}
                        //Console.WriteLine(s + ".");
                        Console.WriteLine(input);
                    }
                    else if (split[0] == "list")
                    {
                        Console.WriteLine(input);
                        //string s = "";
                        //List<string> games = JsonConvert.DeserializeObject<List<String>>(input);
                        //Console.Write("{0} Games open to join: ", games.Count);
                        //for (int i = 0; i < games.Count; i++)
                        //{
                        //    s += games[i];
                        //    if (i != games.Count - 1)
                        //        s += ", ";
                        //}
                        //Console.WriteLine(s + ".");
                    }
                }
            }
            serverSocket.Close();
        }

        /// <summary>
        /// Plays a multiplayer game.
        /// </summary>
        /// <param name="reader">The serever's reader stream.</param>
        /// <param name="writer">The serever's writer stream.</param>
        /// <param name="command">The command.</param>
        public string MultiplayerGame(BinaryReader reader, BinaryWriter writer, string command)
        {
            string input = reader.ReadString();
            if (input == "")
                input = reader.ReadString();
            if (input == "ERROR")
            {
                Console.WriteLine("ERROR");
                return "";
            }
            Maze maze = Maze.FromJSON(input);
            Console.WriteLine(/*maze.ToString()*/ input);
            bool stop = false;
            Task<string> write = null;
            Task read = new Task(delegate ()
            {
                while (!stop)
                {
                    if (stop)
                    {
                        return;
                    }
                    try
                    {
                        input = reader.ReadString();
                    }
                    catch (Exception)
                    {

                        continue;
                    }
                    if (input == "")
                        continue;
                    if (input == "ERROR")
                    {
                        Console.WriteLine("ERROR");
                        continue;
                    }
                    else if (input == "CLOSED " + maze.Name)
                    {
                        stop = true;
                        Console.WriteLine(input/*"CLOSED " + maze.Name*/);
                        if (write != null)
                            write.Wait();
                        return;
                    }
                    else //play move
                    {
                        JObject move = JObject.Parse(input);
                        Console.WriteLine(/*"Other player moved " + move["Direction"] + " in maze " + move["Name"]*/input);
                    }
                }
            });
            write = new Task<string>(() =>
            {
                command = Console.ReadLine();
                while (!stop)
                {
                    if (command == "CLOSE")
                    {
                        System.Environment.Exit(0);
                    }
                    writer.Write(command);
                    if (command.Split()[0] == "close")
                    {
                        stop = true;
                        read.Wait();
                        return "";
                    }
                    command = Console.ReadLine();
                }
                return command;
            });
            read.Start();
            write.Start();
            Task.WaitAll(read, write);
            return write.Result;
        }

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        public static void Main()
        {

            string ip = "127.0.0.1";
            int port = int.Parse(ConfigurationManager.AppSettings["port"]);

            Client client = new Client(ip, port);
            string input = Console.ReadLine();
            while (input != "CLOSE")
            {
                client.HandleCommand(input);
                input = Console.ReadLine();
            }
        }
    }
}
