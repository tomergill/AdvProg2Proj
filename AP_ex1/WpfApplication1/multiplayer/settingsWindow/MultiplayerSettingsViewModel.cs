using MazeLib;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public Maze JoinGame(int selectedIndex)
        {
            if (selectedIndex >= Games.Count)
                return null;
            return model.JoinGame(Games[selectedIndex]);
        }

        public Maze StartGame()
        {
            return model.StartGame();
        }
    }
}
