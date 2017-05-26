using MazeLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    /// <summary>
    /// The view-model of the MultiplayerSettings window.
    /// </summary>
    /// <seealso cref="WpfApplication1.ViewModel" />
    class MultiplayerSettingsViewModel : ViewModel
    {
        /// <summary>
        /// The model.
        /// </summary>
        private MultiplayerSettingsModel model;

        /// <summary>
        /// Gets the games.
        /// </summary>
        /// <value>
        /// The games.
        /// </value>
        public List<String> Games
        {
            get { return model.Games; }
        }

        /// <summary>
        /// Gets or sets the rows.
        /// </summary>
        /// <value>
        /// The rows.
        /// </value>
        public int Rows
        {
            get { return model.Rows; }
            set
            {
                model.Rows = value;
                NotifyPropertyChanged("Rows");
            }
        }

        /// <summary>
        /// Gets or sets the cols.
        /// </summary>
        /// <value>
        /// The cols.
        /// </value>
        public int Cols
        {
            get { return model.Cols; }
            set
            {
                model.Cols = value;
                NotifyPropertyChanged("Cols");
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get { return model.Name; }
            set
            {
                model.Name = value;
                NotifyPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiplayerSettingsViewModel"/> class.
        /// </summary>
        public MultiplayerSettingsViewModel()
        {
            model = new MultiplayerSettingsModel();
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged(e.PropertyName);
            };
        }

        /// <summary>
        /// Joins the game.
        /// </summary>
        /// <param name="selectedIndex">Index of the selected.</param>
        /// <param name="serverSocket">The server socket.</param>
        /// <returns></returns>
        public Maze JoinGame(int selectedIndex, out TcpClient serverSocket)
        {
            serverSocket = null;
            if (selectedIndex >= Games.Count || selectedIndex < 0)
                return null;
            return model.JoinGame(Games[selectedIndex], out serverSocket);
        }

        /// <summary>
        /// Starts the game.
        /// </summary>
        /// <param name="serverSocket">The server socket.</param>
        /// <param name="mName">Name of the maze.</param>
        /// <param name="mRows">The maze rows.</param>
        /// <param name="mCols">The maze cols.</param>
        /// <returns></returns>
        public Maze StartGame(out TcpClient serverSocket, string mName, int mRows, int mCols)
        {
            return model.StartGame(out serverSocket, mName, mRows, mCols);
        }

        public void CloseGame(string name)
        {
            model.CloseGame(name);
        }
    }
}