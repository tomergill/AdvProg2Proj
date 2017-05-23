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

        public Position PlayerPos
        {
            get{return playerPos;}
            set{playerPos = value;}
        }

        public singlePlayerModel(string mazeName, int rowsNum, int colsNum)
        {
            serverIP = ConfigurationManager.AppSettings["ServerIP"];
            portNum = int.Parse(ConfigurationManager.AppSettings["ServerPort"]);
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
                /*
                this.mazeEndPoint = maze.GoalPos;
                this.mazeColsNumber = maze.Cols;
                this.mazeRowsNumber = maze.Rows;
                this.mazeName = maze.Name;
                //is this ok?!
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(MazeString));
                */
            }
        }
    }
}
