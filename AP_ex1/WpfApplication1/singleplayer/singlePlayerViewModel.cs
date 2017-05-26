using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MazeLib;
using System.ComponentModel;
using System.Threading;
using System.Windows.Threading;

namespace WpfApplication1
{
    class singlePlayerViewModel : ViewModel
    {
        private singlePlayerModel SPM;
        private Boolean keepSolving = true;

        public singlePlayerViewModel(String mazeName, int rows, int cols)
        {
            SPM = new singlePlayerModel(mazeName, rows, cols);
            if (SPM.GetMazeOK)
            {
                SPM.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
                {
                    NotifyPropertyChanged("VM" + e.PropertyName);
                };
            }
        }

        public Maze VMMaze
        {
            get { return SPM.Maze; }
        }

        public String VMPlayerPos
        {
            get { return SPM.PlayerPos; }
            set
            {
                SPM.PlayerPos = value;
                NotifyPropertyChanged("VMPlayerPos");
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
            get { return SPM.MazeRepr; }
        }

        public Boolean VMgetEndPointReached
        {
            get { return SPM.GetEndPointReached; }
        }

        public Boolean VMMazeOk
        {
            get { return SPM.GetMazeOK; }
        }

        public Boolean GetKeepSolving
        {
            get { return this.keepSolving; }
            set { this.keepSolving = value; }
        }

        public void Restart()
        {
            SPM.Restart();
            NotifyPropertyChanged("VMPlayerPos");
        }

        public void SolveMe()
        {
            Direction wayNum;
            String solveWay = SPM.GetSolveWay;
            int lengthOfSol = solveWay.Length;
            for (int i = 0; i < lengthOfSol && this.GetKeepSolving; i++)
            {

                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                {

                    wayNum = (Direction)char.GetNumericValue(solveWay[i]);
                    switch (wayNum)
                    {
                        case Direction.Left:
                            GoLeft();
                            break;
                        case Direction.Right:
                            GoRight();
                            break;
                        case Direction.Up:
                            GoUp();
                            break;
                        case Direction.Down:
                            GoDown();
                            break;
                        default:
                            break;
                    }
                    Thread.Sleep(750);
                }));
            }

        }

        public void GoLeft()
        {
            SPM.GoLeft();
            NotifyPropertyChanged("VMPlayerPos");
        }

        public void GoRight()
        {
            SPM.GoRight();
            NotifyPropertyChanged("VMPlayerPos");
        }

        public void GoUp()
        {
            SPM.GoUp();
            NotifyPropertyChanged("VMPlayerPos");
        }

        public void GoDown()
        {
            SPM.GoDown();
            NotifyPropertyChanged("VMPlayerPos");
        }
    }
}
