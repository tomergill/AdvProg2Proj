using MazeLib;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1.multiplayer
{
    class MultiplayerModel : Model
    {
        private Maze maze;
        private Position playerPos, otherPos;
        private TcpClient serverSocket;
        private bool stop;
        private Task read;

        public String PlayerPos
        {
            get { return playerPos.Row + "," + playerPos.Col; }
            set
            {
                String[] splitStr = value.Split(',');
                if (splitStr.Length != 2)
                    throw new InvalidOperationException("Format must be \"number,number\"");
                playerPos.Row = int.Parse(splitStr[0]);
                playerPos.Col = int.Parse(splitStr[1]);
                NotifyPropertyChanged("PlayerPos");
            }
        }

        public String OtherPos
        {
            get { return otherPos.Row + "," + otherPos.Col; }
            set
            {
                String[] splitStr = value.Split(',');
                if (splitStr.Length != 2)
                    throw new InvalidOperationException("Format must be \"number,number\"");
                otherPos.Row = int.Parse(splitStr[0]);
                otherPos.Col = int.Parse(splitStr[1]);
                NotifyPropertyChanged("OtherPos");
            }
        }

        public int MazeRows
        {
            get { return maze.Rows; }
        }

        public int MazeCols
        {
            get { return maze.Cols; }
        }

        public String GoalPos
        {
            get { return maze.GoalPos.Row + "," + maze.GoalPos.Col; }
        }

        public String InitPos
        {
            get { return maze.InitialPos.Row + "," + maze.InitialPos.Col; }
        }

        public String MazeRepr
        {
            get
            {
                string mazeRepo = "";

                for (int i = 0; i < maze.Rows; i++)
                {
                    for (int j = 0; j < maze.Cols; j++)
                    {
                        //if ((i != maze.InitialPos.Row && j != maze.InitialPos.Col) ||
                        //    (i != maze.GoalPos.Row && j != maze.GoalPos.Col))
                        //{
                        if (maze[i, j] == CellType.Wall)
                            mazeRepo += "1";
                        else
                            mazeRepo += "0";
                        if (i != maze.Rows - 1 || j != maze.Cols - 1)
                            mazeRepo += ",";
                        //}
                    }
                }
                return mazeRepo;
            }
        }

        public MultiplayerModel(Maze m, TcpClient serverSocket)
        {
            stop = false;
            maze = m;
            playerPos = new Position(maze.InitialPos.Row, maze.InitialPos.Col);
            otherPos = new Position(maze.InitialPos.Row, maze.InitialPos.Col);
            this.serverSocket = serverSocket;

            read = new Task(delegate ()
            {
                using (NetworkStream stream = serverSocket.GetStream())
                using (BinaryReader reader = new BinaryReader(stream))
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    string input;
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
                        if (input.StartsWith("ERROR"))
                        {
                            //Console.WriteLine("ERROR");
                            continue;
                        }
                        else if (input == "CLOSED " + maze.Name)
                        {
                            stop = true;
                            //Console.WriteLine(input/*"CLOSED " + maze.Name*/);
                            serverSocket.Close();
                            return;
                        }
                        else //play move
                        {
                            JObject move = JObject.Parse(input);
                            MakeAMove(false, move["Direction"].ToString());
                        }
                    }
                }
            });
        }

        private bool MakeAMove(bool thisPlayer, String direction)
        {
            Position pos = thisPlayer ? playerPos : otherPos;
            switch (direction)
            {
                case "right":
                    if (pos.Col + 1 < maze.Cols && maze[pos.Row, pos.Col + 1] == CellType.Free)
                        pos.Col += 1;
                    break;
                case "left":
                    if (pos.Col - 1 >= 0 && maze[pos.Row, pos.Col - 1] == CellType.Free)
                        pos.Col -= 1;
                    break;
                case "down":
                    if (pos.Row + 1 < maze.Rows && maze[pos.Row + 1, pos.Col] == CellType.Free)
                        pos.Row += 1;
                    break;
                case "up":
                    if (pos.Row - 1 >= 0 && maze[pos.Row - 1, pos.Col] == CellType.Free)
                        pos.Row -= 1;
                    break;
                default:
                    break;
            }
            string prop = thisPlayer ? "Player" : "Other";
            NotifyPropertyChanged(prop + "Pos");

            return (pos.Row == maze.GoalPos.Row && pos.Col == maze.GoalPos.Col);
        }
    }
}
