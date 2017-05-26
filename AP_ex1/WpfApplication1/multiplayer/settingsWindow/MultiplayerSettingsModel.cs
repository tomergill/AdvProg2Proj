﻿using MazeLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class MultiplayerSettingsModel : Model
    {
        private List<String> games;
        private int rows;
        private int cols;
        private string name;

        public MultiplayerSettingsModel()
        {
            Games = GetGameList();
            Rows = Properties.Settings.Default.MazeRows;
            Cols = Properties.Settings.Default.MazeCols;
            Name = "";
        }

        public List<string> Games { get => games; set => games = value; }
        public int Rows
        {
            get => rows;
            set { rows = value; NotifyPropertyChanged("Rows"); }
        }
        public int Cols
        {
            get => cols;
            set { cols = value; NotifyPropertyChanged("Cols"); }
        }
        public string Name
        {
            get => name;
            set { name = value; NotifyPropertyChanged("Name"); }
        }

        private List<String> GetGameList()
        {
            string ip = Properties.Settings.Default.ServerIP;
            int port = Properties.Settings.Default.ServerPort;
            IPEndPoint server = new IPEndPoint(IPAddress.Parse(ip), port);
            TcpClient serverSocket = new TcpClient();
            serverSocket.Connect(server);

            while (!serverSocket.Connected) ;

            using (NetworkStream stream = serverSocket.GetStream())
            using (BinaryReader reader = new BinaryReader(stream))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write("list");
                string output = reader.ReadString();
                if (output != null)
                    return JsonConvert.DeserializeObject<List<String>>(output);
                return new List<string>();
            }
        }

        public Maze JoinGame(string mazeName, out TcpClient serverSocket)
        {
            string ip = Properties.Settings.Default.ServerIP;
            int port = Properties.Settings.Default.ServerPort;
            IPEndPoint server = new IPEndPoint(IPAddress.Parse(ip), port);
            serverSocket = new TcpClient();
            serverSocket.Connect(server);

            while (!serverSocket.Connected) ;

            NetworkStream stream = serverSocket.GetStream();
            BinaryReader reader = new BinaryReader(stream);
            BinaryWriter writer = new BinaryWriter(stream);
            try
            {
                writer.Write("join " + mazeName);
                string output = reader.ReadString();
                if (output == null || output == "")
                    return null;
                return Maze.FromJSON(output);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Maze StartGame(out TcpClient serverSocket, string mName, int mRows, int mCols)
        {
            serverSocket = null;
            if (mName == "" || mRows < 1 || mCols < 1)
                return null;

            string ip = Properties.Settings.Default.ServerIP;
            int port = Properties.Settings.Default.ServerPort;
            IPEndPoint server = new IPEndPoint(IPAddress.Parse(ip), port);
            serverSocket = new TcpClient();
            serverSocket.Connect(server);

            while (!serverSocket.Connected) ;

            NetworkStream stream = serverSocket.GetStream();
            BinaryReader reader = new BinaryReader(stream);
            BinaryWriter writer = new BinaryWriter(stream);

            try
            {
                writer.Write($"start {mName} {mRows} {mCols}");
                string output = reader.ReadString();
                if (output == null || output == "")
                    return null;
                return Maze.FromJSON(output);
            }
            catch (Exception)
            {
                return null;
            }
            
        }
    }
}