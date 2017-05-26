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
    class MultiplayerViewModel : ViewModel
    {
        private MultiplayerModel model;

        public String VMPlayerPos
        {
            get { return model.PlayerPos; }
            set
            {
                model.PlayerPos = value;
                NotifyPropertyChanged("VMPlayerPos");
            }

        }

        public String VMOtherPos
        {
            get { return model.OtherPos; }
            set
            {
                model.OtherPos = value;
                NotifyPropertyChanged("VMOtherPos");
            }

        }

        public int VMMazeRows
        {
            get { return model.MazeRows; }
        }

        public int VMMazeCols
        {
            get { return model.MazeCols; }
        }

        public String VMGoalPos
        {
            get { return model.GoalPos; }
        }

        public String VMInitPos
        {
            get { return model.InitPos; }
        }

        public String VMMazeRepr
        {
            get { return model.MazeRepr; }
        }

        public bool VMStop
        {
            get { return model.Stop; }
        }

        public MultiplayerViewModel(Maze m, TcpClient serverSocket, MultiplayerModel.losingDelegate l)
        {
            model = new MultiplayerModel(m, serverSocket, l);
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM" + e.PropertyName);
            };
        }

        public bool MakeAMove(Direction direction)
        {
            return model.MakeAMove(true, direction);
        }

        public void CloseGame() { model.CloseGame(); }
    }
}
