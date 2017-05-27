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
    /// The view-model of the Multiplayer window.
    /// </summary>
    /// <seealso cref="WpfApplication1.ViewModel" />
    class MultiplayerViewModel : ViewModel
    {
        /// <summary>
        /// The model.
        /// </summary>
        private MultiplayerModel model;

        /// <summary>
        /// Gets or sets the model's player position.
        /// </summary>
        /// <value>
        /// The player's position.
        /// </value>
        public String VMPlayerPos
        {
            get { return model.PlayerPos; }
            set
            {
                model.PlayerPos = value;
                NotifyPropertyChanged("VMPlayerPos");
            }

        }

        /// <summary>
        /// Gets or sets the model's other position.
        /// </summary>
        /// <value>
        /// The other player's position.
        /// </value>
        public String VMOtherPos
        {
            get { return model.OtherPos; }
            set
            {
                model.OtherPos = value;
                NotifyPropertyChanged("VMOtherPos");
            }

        }

        /// <summary>
        /// Gets the maze rows.
        /// </summary>
        /// <value>
        /// The maze rows.
        /// </value>
        public int VMMazeRows
        {
            get { return model.MazeRows; }
        }

        /// <summary>
        /// Gets the maze columns.
        /// </summary>
        /// <value>
        /// The maze cols.
        /// </value>
        public int VMMazeCols
        {
            get { return model.MazeCols; }
        }

        /// <summary>
        /// Gets the goal position.
        /// </summary>
        /// <value>
        /// The goal position.
        /// </value>
        public String VMGoalPos
        {
            get { return model.GoalPos; }
        }

        /// <summary>
        /// Gets the initial position.
        /// </summary>
        /// <value>
        /// The initial position.
        /// </value>
        public String VMInitPos
        {
            get { return model.InitPos; }
        }

        /// <summary>
        /// Gets the maze string representation.
        /// </summary>
        /// <value>
        /// The maze string representation.
        /// </value>
        public String VMMazeRepr
        {
            get { return model.MazeRepr; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether game stopped.
        /// </summary>
        /// <value>
        ///   <c>true</c> if game stopped; otherwise, <c>false</c>.
        /// </value>
        public bool VMStop
        {
            get { return model.Stop; }
            set { model.Stop = value; NotifyPropertyChanged("VMStop"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiplayerViewModel"/> class.
        /// </summary>
        /// <param name="m">The maze.</param>
        /// <param name="serverSocket">The server socket.</param>
        /// <param name="l">The losing function.</param>
        public MultiplayerViewModel(Maze m, TcpClient serverSocket, MultiplayerModel.losingDelegate l)
        {
            model = new MultiplayerModel(m, serverSocket, l);
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM" + e.PropertyName);
            };
        }

        /// <summary>
        /// Makes a move, as the player.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <returns>True if player won, flase otherwise.</returns>
        public bool MakeAMove(Direction direction)
        {
            return model.MakeAMove(true, direction);
        }

        /// <summary>
        /// Closes the game.
        /// </summary>
        public void CloseGame() { model.CloseGame(); }
    }
}
