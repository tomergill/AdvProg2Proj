using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MazeLib;

namespace WpfApplication1
{
    class singlePlayerViewModel : ViewModel
    {
        private singlePlayerModel SPM;
        
        public singlePlayerViewModel(String mazeName, int rows, int cols)
        {
            SPM = new singlePlayerModel(mazeName, rows, cols);
        }

        public Maze VMMaze
        {
            get { return SPM.Maze; }
        }

        public String VMPlayerPos
        {
            get { return SPM.PlayerPos; }
            set {
                SPM.PlayerPos = value;
                NotifyPropertyChanged("PlayerPos");
            }
            
        }

        public int VMMazeRows
        {
            get { return SPM.MazeRows; }
        }

        public int VMMazeCols
        {
            get { return SPM.MazeCols; }
        }

        public String VMGoalPos
        {
            get { return SPM.GoalPos; }
        }

        public String VMInitPos
        {
            get { return SPM.InitPos; }
        }

        public String VMMazeRepr
        {
            get {return SPM.MazeRepr; }
        }
    }
}
