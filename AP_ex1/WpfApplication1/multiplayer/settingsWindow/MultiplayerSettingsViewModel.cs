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
    class MultiplayerSettingsViewModel : ViewModel
    {
        private MultiplayerSettingsModel model;

        public List<String> Games
        {
            get { return model.Games; }
        }

        public int Rows
        {
            get { return model.Rows; }
            set
            {
                model.Rows = value;
                NotifyPropertyChanged("Rows");
            }
        }

        public int Cols
        {
            get { return model.Cols; }
            set
            {
                model.Cols = value;
                NotifyPropertyChanged("Cols");
            }
        }

        public string Name
        {
            get { return model.Name; }
            set
            {
                model.Name = value;
                NotifyPropertyChanged("Name");
            }
        }

        public MultiplayerSettingsViewModel()
        {
            model = new MultiplayerSettingsModel();
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged(e.PropertyName);
            };
        }

        public Maze JoinGame(int selectedIndex, out TcpClient serverSocket)
        {
            serverSocket = null;
            if (selectedIndex >= Games.Count || selectedIndex < 0)
                return null;
            return model.JoinGame(Games[selectedIndex], out serverSocket);
        }

        public Maze StartGame(out TcpClient serverSocket)
        {
            return model.StartGame(out serverSocket);
        }
    }
}