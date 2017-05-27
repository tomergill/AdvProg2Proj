using MazeLib;
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
    /// <summary>
    /// The model of the MutiplayerSettings window.
    /// </summary>
    /// <seealso cref="WpfApplication1.Model" />
    class MultiplayerSettingsModel : Model
    {
        /// <summary>
        /// The games names.
        /// </summary>
        private List<String> games;

        /// <summary>
        /// The rows.
        /// </summary>
        private int rows;

        /// <summary>
        /// The columns.
        /// </summary>
        private int cols;

        /// <summary>
        /// The name.
        /// </summary>
        private string name;

        private TcpClient serverSocketRef = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiplayerSettingsModel"/> class.
        /// </summary>
        public MultiplayerSettingsModel()
        {
            Games = GetGameList();
            Rows = Properties.Settings.Default.MazeRows;
            Cols = Properties.Settings.Default.MazeCols;
            Name = "";
        }

        /// <summary>
        /// Gets or sets the games.
        /// </summary>
        /// <value>
        /// The games.
        /// </value>
        public List<string> Games { get => games; set => games = value; }

        /// <summary>
        /// Gets or sets the rows.
        /// </summary>
        /// <value>
        /// The rows.
        /// </value>
        public int Rows
        {
            get => rows;
            set { rows = value; NotifyPropertyChanged("Rows"); }
        }

        /// <summary>
        /// Gets or sets the columns.
        /// </summary>
        /// <value>
        /// The columns.
        /// </value>
        public int Cols
        {
            get => cols;
            set { cols = value; NotifyPropertyChanged("Cols"); }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get => name;
            set { name = value; NotifyPropertyChanged("Name"); }
        }

        /// <summary>
        /// Gets the game list.
        /// </summary>
        /// <returns>list of games names</returns>
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

        /// <summary>
        /// Joins the game.
        /// </summary>
        /// <param name="mazeName">Name of the maze.</param>
        /// <param name="serverSocket">The server socket.</param>
        /// <returns>The maze to be played</returns>
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

        /// <summary>
        /// Starts the game.
        /// </summary>
        /// <param name="serverSocket">The server socket.</param>
        /// <param name="mName">Name of the m.</param>
        /// <param name="mRows">The m rows.</param>
        /// <param name="mCols">The m cols.</param>
        /// <returns>The maze to be played.</returns>
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

            serverSocketRef = serverSocket;

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

        public void CloseGame(string name)
        {
            if (name == null || serverSocketRef == null)
                return;
            try
            {
                BinaryWriter writer = new BinaryWriter(serverSocketRef.GetStream());
                writer.Write($"close {name}");
                writer.Flush();
                serverSocketRef.GetStream().Dispose();
                serverSocketRef.Close();
            }
            catch (Exception e)
            {
                return;
            }
        }
    }
}