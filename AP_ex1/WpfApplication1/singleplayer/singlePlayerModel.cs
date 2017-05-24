using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using MazeLib;
using System.ComponentModel;
using Newtonsoft.Json.Linq;
using static WpfApplication1.DIRECTION;

namespace WpfApplication1
{
    class singlePlayerModel : Model
    {
        private IPEndPoint server;
        private string serverIP;
        private int portNum;
        private String mazeName;
        private int algorithm;
        private Maze maze;
        private Position playerPos;
        private String solveWay;
        private Boolean endPointReached;


        public singlePlayerModel(string mazeName, int rowsNum, int colsNum)
        {
            serverIP = Properties.Settings.Default.ServerIP;
            portNum = Properties.Settings.Default.ServerPort;
            this.mazeName = mazeName;
            algorithm = Properties.Settings.Default.SearchAlgorithm;
            server = new IPEndPoint(IPAddress.Parse(serverIP), portNum);
            endPointReached = false;

            TcpClient serverMazeSocket = new TcpClient();
            serverMazeSocket.Connect(server);
            while (!serverMazeSocket.Connected) ;

            using (NetworkStream stream = serverMazeSocket.GetStream())
            using (BinaryReader reader = new BinaryReader(stream))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write("generate " + mazeName + " " + rowsNum + " " + colsNum);
                string mazeInfo = reader.ReadString();
                maze = Maze.FromJSON(mazeInfo);
                playerPos = maze.InitialPos;
                
            }

        }

        public Maze Maze
        {
            get { return maze; }
        }

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

        public String GetSolveWay
        {
            get
            {
                if (solveWay==default(String))
                    GetSolutionFromServer();
                return this.solveWay;
            }
        }

        public void Restart()
        {
            playerPos.Row = maze.InitialPos.Row;
            playerPos.Col = maze.InitialPos.Col;
            NotifyPropertyChanged("PlayerPos");
        }

        public Boolean GetEndPointReached
        {
            get { return endPointReached; }
        }

        private String GetSolutionFromServer()
        {
            TcpClient serverSolSocket = new TcpClient();
            serverSolSocket.Connect(server);
            while (!serverSolSocket.Connected) ;

            using (NetworkStream stream = serverSolSocket.GetStream())
            using (BinaryReader reader = new BinaryReader(stream))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write("solve " + mazeName + " " + algorithm);
                String mazeSolution = reader.ReadString();
                JObject solution = JObject.Parse(mazeSolution);
                solveWay = (string)solution["Solution"];
            }
            return solveWay;
        }

        public void GoLeft()
        {
            CheckIfMovePossible(DIRECTION.left);
        }

        public void GoRight()
        {
            CheckIfMovePossible(DIRECTION.right);
        }

        public void GoUp()
        {
            CheckIfMovePossible(DIRECTION.up);
        }

        public void GoDown()
        {
            CheckIfMovePossible(DIRECTION.down);
        }

        private void CheckIfMovePossible(DIRECTION direction)
        {
            switch (direction)
            {
                case DIRECTION.right:
                    if (playerPos.Col + 1 < maze.Cols && maze[playerPos.Row, playerPos.Col + 1] == CellType.Free)
                        playerPos.Col += 1;
                    break;
                case DIRECTION.left:
                    if (playerPos.Col - 1 >= 0 && maze[playerPos.Row, playerPos.Col - 1] == CellType.Free)
                        playerPos.Col -= 1;
                    break;
                case DIRECTION.down:
                    if (playerPos.Row + 1 < maze.Rows && maze[playerPos.Row + 1, playerPos.Col] == CellType.Free)
                        playerPos.Row += 1;
                    break;
                case DIRECTION.up:
                    if (playerPos.Row - 1 >= 0 && maze[playerPos.Row - 1, playerPos.Col] == CellType.Free)
                        playerPos.Row -= 1;
                    break;
                default:
                    break;

            }
            NotifyPropertyChanged("PlayerPos");
            if (playerPos.Row == maze.GoalPos.Row && playerPos.Col == maze.GoalPos.Col)
                endPointReached = true;
            NotifyPropertyChanged("getEndPointReached");
        }
    }
}
