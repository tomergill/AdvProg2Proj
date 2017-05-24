using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1.multiplayer
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
    }
}
