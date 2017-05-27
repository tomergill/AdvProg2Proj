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

namespace WpfApplication1
{
    /// <summary>
    /// single player model
    /// </summary>
    class singlePlayerModel : Model
    {
        /// <summary>
        /// fields
        /// </summary>
        private IPEndPoint server;
        private string serverIP;
        private int portNum;
        private String mazeName;
        //checks if maze was generated successfully
        private Boolean mazeOK;
        //bfs or dfs
        private int algorithm;
        private Maze maze;
        private Position playerPos;
        //solution in string
        private String solveWay;
        private Boolean endPointReached;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="mazeName"></param>
        /// <param name="rowsNum"></param>
        /// <param name="colsNum"></param>
        public singlePlayerModel(string mazeName, int rowsNum, int colsNum)
        {
            /// <summary>
            /// initializing
            /// </summary>
            serverIP = Properties.Settings.Default.ServerIP;
            portNum = Properties.Settings.Default.ServerPort;
            this.mazeName = mazeName;
            algorithm = Properties.Settings.Default.SearchAlgorithm;
            server = new IPEndPoint(IPAddress.Parse(serverIP), portNum);
            endPointReached = false;

            /// <summary>
            /// creating connection to get maze
            /// </summary>
            TcpClient serverMazeSocket = new TcpClient();
            serverMazeSocket.Connect(server);
            while (!serverMazeSocket.Connected) ;

            //connecting
            using (NetworkStream stream = serverMazeSocket.GetStream())
            using (BinaryReader reader = new BinaryReader(stream))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write("generate " + mazeName + " " + rowsNum + " " + colsNum);
                string mazeInfo = reader.ReadString();
                //error getting maze
                if (mazeInfo.Contains("ERROR"))
                    this.mazeOK = false;
                else
                {
                    //maze received successfully
                    this.mazeOK = true;
                    maze = Maze.FromJSON(mazeInfo);
                    playerPos = maze.InitialPos;
                    if (playerPos.Row == maze.GoalPos.Row && playerPos.Col == maze.GoalPos.Col)
                        endPointReached = true;
                }
            }

        }

        /// <summary>
        /// getter for maze
        /// </summary>
        public Maze Maze
        {
            get { return maze; }
        }

        /// <summary>
        /// getter and setter for current player position
        /// </summary>
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

        /// <summary>
        /// getter for maze rows
        /// </summary>
        public int MazeRows
        {
            get { return maze.Rows; }
        }

        /// <summary>
        /// getter for maze columns
        /// </summary>
        public int MazeCols
        {
            get { return maze.Cols; }
        }

        /// <summary>
        /// getter for maze goal position
        /// </summary>
        public String GoalPos
        {
            get { return maze.GoalPos.Row + "," + maze.GoalPos.Col; }
        }

        /// <summary>
        /// getter for maze initial position
        /// </summary>
        public String InitPos
        {
            get { return maze.InitialPos.Row + "," + maze.InitialPos.Col; }
        }

        /// <summary>
        /// getter for maze representation
        /// </summary>
        public String MazeRepr
        {
            get
            {
                string mazeRepo = "";

                for (int i = 0; i < maze.Rows; i++)
                {
                    for (int j = 0; j < maze.Cols; j++)
                    {
                        if (maze[i, j] == CellType.Wall)
                            mazeRepo += "1";
                        else
                            mazeRepo += "0";
                        if (i != maze.Rows - 1 || j != maze.Cols - 1)
                            mazeRepo += ",";
                    }
                }
                return mazeRepo;
            }
        }

        /// <summary>
        /// getter for maze solution string
        /// </summary>
        public String GetSolveWay
        {
            get
            {
                if (solveWay == default(String))
                    GetSolutionFromServer();
                return this.solveWay;
            }
        }

        /// <summary>
        /// getter for boolean check if maze OK
        /// </summary>
        public Boolean GetMazeOK
        {
            get { return this.mazeOK; }
        }

        /// <summary>
        /// view model notified restart, model is restarting and notifying view model back
        /// </summary>
        public void Restart()
        {
            playerPos.Row = maze.InitialPos.Row;
            playerPos.Col = maze.InitialPos.Col;
            if (playerPos.Row != maze.GoalPos.Row || playerPos.Col != maze.GoalPos.Col)
            {
                this.endPointReached = false;
                NotifyPropertyChanged("GetEndPointReached");
            }
            NotifyPropertyChanged("PlayerPos");
        }

        /// <summary>
        /// getter for boolean check if maze goal position was reached
        /// </summary>
        public Boolean GetEndPointReached
        {
            get { return endPointReached; }
        }

        /// <summary>
        /// model was notified that a solution is required
        /// </summary>
        /// <returns></returns>
        private String GetSolutionFromServer()
        {
            //opening connection
            TcpClient serverSolSocket = new TcpClient();
            serverSolSocket.Connect(server);
            while (!serverSolSocket.Connected) ;

            //getting solution
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

        /// <summary>
        /// view model notified left, model calls CheckIfMovePossible to see if step is valid
        /// </summary>
        public void GoLeft()
        {
            CheckIfMovePossible(Direction.Left);
        }

        /// <summary>
        /// view model notified right, model calls CheckIfMovePossible to see if step is valid
        /// </summary>
        public void GoRight()
        {
            CheckIfMovePossible(Direction.Right);
        }

        /// <summary>
        /// view model notified up, model calls CheckIfMovePossible to see if step is valid
        /// </summary>
        public void GoUp()
        {
            CheckIfMovePossible(Direction.Up);
        }

        /// <summary>
        /// view model notified down, model calls CheckIfMovePossible to see if step is valid
        /// </summary>
        public void GoDown()
        {
            CheckIfMovePossible(Direction.Down);
        }

        /// <summary>
        /// function checks if step is valid to move
        /// </summary>
        /// <param name="direction"></param>
        private void CheckIfMovePossible(Direction direction)
        {
            switch (direction)
            {
                case Direction.Right:
                    if (playerPos.Col + 1 < maze.Cols && maze[playerPos.Row, playerPos.Col + 1] == CellType.Free)
                        playerPos.Col += 1;
                    break;
                case Direction.Left:
                    if (playerPos.Col - 1 >= 0 && maze[playerPos.Row, playerPos.Col - 1] == CellType.Free)
                        playerPos.Col -= 1;
                    break;
                case Direction.Down:
                    if (playerPos.Row + 1 < maze.Rows && maze[playerPos.Row + 1, playerPos.Col] == CellType.Free)
                        playerPos.Row += 1;
                    break;
                case Direction.Up:
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
