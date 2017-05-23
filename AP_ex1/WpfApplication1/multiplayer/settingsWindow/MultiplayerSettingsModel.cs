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
    class MultiplayerSettingsModel
    {
        private List<String> games;
        private int rows;
        private int cols;

        public MultiplayerSettingsModel()
        {
            Games = GetGameList();
            Rows = Properties.Settings.Default.MazeRows;
            Cols = Properties.Settings.Default.MazeCols;
        }

        public List<string> Games { get => games; set => games = value; }
        public int Rows { get => rows; set => rows = value; }
        public int Cols { get => cols; set => cols = value; }

        private List<String> GetGameList()
        {
            string ip = Properties.Settings.Default.ServerIP;
            int port = int.Parse(Properties.Settings.Default.ServerIP);
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

            }
        }
    }
}
