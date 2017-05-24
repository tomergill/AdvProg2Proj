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

namespace WpfApplication1
{
    class singlePlayerModel
    {
        private IPEndPoint server;
        private string serverIP;
        private int portNum;
        private Maze maze;
        private Position playerPos;

        public Maze Maze
        {
            get{return maze;}
        }

        public String PlayerPos
        {
            get{return playerPos.Row + "," + playerPos.Col;}
            set
            {
                String[] splitStr = value.Split(',');
                if (splitStr.Length!=2)
                    throw new InvalidOperationException("Format must be \"number,number\"");
                playerPos.Row = int.Parse(splitStr[0]);
                playerPos.Col = int.Parse(splitStr[1]);
            }
        }

        public int MazeRows
        {
            get {return maze.Rows; }
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

        public singlePlayerModel(string mazeName, int rowsNum, int colsNum)
        {
            serverIP = Properties.Settings.Default.ServerIP;
            portNum = Properties.Settings.Default.ServerPort;
            server = new IPEndPoint(IPAddress.Parse(serverIP), portNum);

            TcpClient serverSocket = new TcpClient();
            serverSocket.Connect(server);

            while (!serverSocket.Connected) ;

            using (NetworkStream stream = serverSocket.GetStream())
            using (BinaryReader reader = new BinaryReader(stream))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write("generate " + mazeName + " " + rowsNum + " " + colsNum);
                string mazeInfo = reader.ReadString();
                maze = Maze.FromJSON(mazeInfo);
                playerPos = maze.InitialPos;
            }
        }
    }
}
