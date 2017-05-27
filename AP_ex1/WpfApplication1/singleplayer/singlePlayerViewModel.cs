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
    /// <summary>
    /// single player view model
    /// </summary>
    class singlePlayerViewModel : ViewModel
    {
        //having a model
        private singlePlayerModel SPM;
        // boolean to check if solving maze is still required
        private Boolean keepSolving = true;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="mazeName"></param>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        public singlePlayerViewModel(String mazeName, int rows, int cols)
        {
            //initializing model
            SPM = new singlePlayerModel(mazeName, rows, cols);
            //if maze recieved, notifying view
            if (SPM.GetMazeOK)
            {
                SPM.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
                {
                    NotifyPropertyChanged("VM" + e.PropertyName);
                };
            }
        }

        /// <summary>
        /// retriving maze from model
        /// </summary>
        public Maze VMMaze
        {
            get { return SPM.Maze; }
        }

        /// <summary>
        /// retriving player position from model, and notifying model if changed
        /// </summary>
        public String VMPlayerPos
        {
            get { return SPM.PlayerPos; }
            set
            {
                SPM.PlayerPos = value;
                NotifyPropertyChanged("VMPlayerPos");
            }

        }

        /// <summary>
        /// retriving maze rows from model
        /// </summary>
        public int VMMazeRows
        {
            get { return SPM.MazeRows; }
        }

        /// <summary>
        /// retriving maze cols from model
        /// </summary>
        public int VMMazeCols
        {
            get { return SPM.MazeCols; }
        }

        /// <summary>
        /// retriving maze goal position from model
        /// </summary>
        public String VMGoalPos
        {
            get { return SPM.GoalPos; }
        }

        /// <summary>
        /// retriving maze initial position from model
        /// </summary>
        public String VMInitPos
        {
            get { return SPM.InitPos; }
        }

        /// <summary>
        /// retriving maze string representation from model
        /// </summary>
        public String VMMazeRepr
        {
            get { return SPM.MazeRepr; }
        }

        /// <summary>
        /// checking in model if goal position was reached
        /// </summary>
        public Boolean VMgetEndPointReached
        {
            get { return SPM.GetEndPointReached; }
        }

        /// <summary>
        /// checking in model if maze was generated successfully
        /// </summary>
        public Boolean VMMazeOk
        {
            get { return SPM.GetMazeOK; }
        }

        /// <summary>
        /// getter and setter for keep solving boolean
        /// </summary>
        public Boolean GetKeepSolving
        {
            get { return this.keepSolving; }
            set { this.keepSolving = value; }
        }

        /// <summary>
        /// sending messege to model to restart
        /// </summary>
        public void Restart()
        {
            SPM.Restart();
            NotifyPropertyChanged("VMPlayerPos");
        }

        /// <summary>
        /// sending messege to model to get solution, after that,
        /// notifying view and showing the solution to user
        /// </summary>
        public void SolveMe()
        {
            Direction wayNum;
            String solveWay = SPM.GetSolveWay;
            int lengthOfSol = solveWay.Length;
            /*runnig on solution length, notice that run would stop if view will 
             notify --> keepSolving == false */
            for (int i = 0; i < lengthOfSol && this.GetKeepSolving; i++)
            {
                //creating thread
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
                    //sleeping 1 second
                    Thread.Sleep(1000);
                }));
            }

        }

        /// <summary>
        /// notifying model to move left
        /// </summary>
        public void GoLeft()
        {
            SPM.GoLeft();
            NotifyPropertyChanged("VMPlayerPos");
        }

        /// <summary>
        /// notifying model to move right
        /// </summary>
        public void GoRight()
        {
            SPM.GoRight();
            NotifyPropertyChanged("VMPlayerPos");
        }

        /// <summary>
        /// notifying model to move up
        /// </summary>
        public void GoUp()
        {
            SPM.GoUp();
            NotifyPropertyChanged("VMPlayerPos");
        }

        /// <summary>
        /// notifying model to move down
        /// </summary>
        public void GoDown()
        {
            SPM.GoDown();
            NotifyPropertyChanged("VMPlayerPos");
        }
    }
}
