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
    public class Client
    {
        private IPEndPoint server;

        public Client(string ip, int port)
        {
            server = new IPEndPoint(IPAddress.Parse(ip), port);
        }

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
                    MultiplayerGame(reader, writer, split[0]);

                /*Hendles single player commands*/
                /*Generate maze command*/
                string input = reader.ReadString();
                //Console.WriteLine("GOT " + input);

                if (input == "ERROR")
                    Console.WriteLine("ERROR");
                else if (split[0] == "generate")
                {
                    Maze maze = Maze.FromJSON(input);
                    Console.WriteLine(maze);
                }
                else if (split[0] == "solve")
                {
                    JObject solution = JObject.Parse(input);
                    Console.WriteLine($"Solution of {solution["Name"]} takes {solution["NodesEvaluated"]} steps:");
                    string sol = (string) solution["Solution"];
                    for (int i = 0; i < sol.Length - 1; i++)
                    {
                        string si;
                        switch (sol[i])
                        {
                            case '0': si = "Left"; break;
                            case '1': si = "Right"; break;
                            case '2': si = "Up"; break;
                            default: si = "Down"; break;
                        }
                        Console.Write(si + ", ");
                    }
                    string s;
                    switch (sol[sol.Length - 1])
                    {
                        case '0': s = "Left"; break;
                        case '1': s = "Right"; break;
                        case '2': s = "Up"; break;
                        default: s = "Down"; break;
                    }
                    Console.WriteLine(s + ".");
                }
                else if (split[0] == "list")
                {
                    
                    string s = "";
                    List<string> games = JsonConvert.DeserializeObject<List<String>>(input);
                    Console.Write("{0} Games open to join: ", games.Count);
                    for (int i = 0;  i < games.Count; i++)
                    {
                        s += games[i];
                        if (i != games.Count - 1)
                            s+= ", ";
                    }
                    Console.WriteLine(s + ".");
                }
            }
            serverSocket.Close();
        }

        public void MultiplayerGame(BinaryReader reader, BinaryWriter writer, string command)
        {
            string input = reader.ReadString();
            if (input == "")
                input = reader.ReadString();
            if (input == "ERROR")
            {
                Console.WriteLine("ERROR");
                return;
            }
            Maze maze = Maze.FromJSON(input);
            Console.WriteLine(maze.ToString());
            bool stop = false;
            Task read = new Task(delegate ()
            {
                while (!stop)
                {
                    input = reader.ReadString();
                    if (input == "ERROR")
                    {
                        Console.WriteLine("ERROR");
                        continue;
                    }
                    else if (input == "CLOSED " + maze.Name)
                    {
                        stop = true;
                        Console.WriteLine("CLOSED " + maze.Name);
                        return;
                    }
                    else //play move
                    {
                        JObject move = JObject.Parse(input);
                        Console.WriteLine("Other player moved +" + move["Direction"] + " in maze " + move["Name"]);
                    }
                }
            });
            Task write = new Task(() =>
            {
                while (!stop)
                {
                    command = Console.ReadLine();
                    if (command == "CLOSE")
                    {
                        System.Environment.Exit(0);
                    }
                    writer.Write(command);
                    if (command.Split()[0] == "close")
                    {
                        stop = true;
                        break;
                    }
                }
            });
            read.Start();
            write.Start();
            Task.WaitAll(read, write);
        }

        public static void Main()
        {
            Position p = new Position(2, 2);
            Console.WriteLine(p.ToString());

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
