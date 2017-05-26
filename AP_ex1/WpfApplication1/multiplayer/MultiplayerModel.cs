using MazeLib;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    /// <summary>
    /// The model for the Multiplayer window.
    /// </summary>
    /// <seealso cref="WpfApplication1.Model" />
    class MultiplayerModel : Model
    {
        /// <summary>
        /// The maze.
        /// </summary>
        private Maze maze;

        /// <summary>
        /// The player's and other player's positions.
        /// </summary>
        private Position playerPos, otherPos;

        /// <summary>
        /// The server socket.
        /// </summary>
        private TcpClient serverSocket;

        /// <summary>
        /// Represents wether the game have stopped.
        /// </summary>
        private bool stop;

        /// <summary>
        /// The task reading from teh socket.
        /// </summary>
        private Task read;

        /// <summary>
        /// Gets or sets the player position.
        /// </summary>
        /// <value>
        /// The player position.
        /// </value>
        /// <exception cref="System.InvalidOperationException">Format must be \"number,number\"</exception>
        public String PlayerPos
        {
            get { return playerPos.Row + "," + playerPos.Col; }
            set
            {
                String[] splitStr = value.Split(',');
                if (splitStr.Length != 2)
                    throw new InvalidOperationException("Format must be \"number,number\"");
                playerPos.Row = int.Parse(splitStr[0]);
                playerPos.Col = int.Parse(splitStr[1]);
                NotifyPropertyChanged("PlayerPos");
            }
        }

        /// <summary>
        /// Gets or sets the other position.
        /// </summary>
        /// <value>
        /// The other position.
        /// </value>
        /// <exception cref="System.InvalidOperationException">Format must be \"number,number\"</exception>
        public String OtherPos
        {
            get { return otherPos.Row + "," + otherPos.Col; }
            set
            {
                String[] splitStr = value.Split(',');
                if (splitStr.Length != 2)
                    throw new InvalidOperationException("Format must be \"number,number\"");
                otherPos.Row = int.Parse(splitStr[0]);
                otherPos.Col = int.Parse(splitStr[1]);
                NotifyPropertyChanged("OtherPos");
            }
        }

        /// <summary>
        /// Gets the maze rows.
        /// </summary>
        /// <value>
        /// The maze rows.
        /// </value>
        public int MazeRows
        {
            get { return maze.Rows; }
        }

        /// <summary>
        /// Gets the maze cols.
        /// </summary>
        /// <value>
        /// The maze cols.
        /// </value>
        public int MazeCols
        {
            get { return maze.Cols; }
        }

        /// <summary>
        /// Gets the goal position.
        /// </summary>
        /// <value>
        /// The goal position.
        /// </value>
        public String GoalPos
        {
            get { return maze.GoalPos.Row + "," + maze.GoalPos.Col; }
        }

        /// <summary>
        /// Gets the initialize position.
        /// </summary>
        /// <value>
        /// The initialize position.
        /// </value>
        public String InitPos
        {
            get { return maze.InitialPos.Row + "," + maze.InitialPos.Col; }
        }

        /// <summary>
        /// Gets the maze repr.
        /// </summary>
        /// <value>
        /// The maze repr.
        /// </value>
        public String MazeRepr
        {
            get
            {
                string mazeRepo = "";

                for (int i = 0; i < maze.Rows; i++)
                {
                    for (int j = 0; j < maze.Cols; j++)
                    {
                        //if ((i != maze.InitialPos.Row && j != maze.InitialPos.Col) ||
                        //    (i != maze.GoalPos.Row && j != maze.GoalPos.Col))
                        //{
                        if (maze[i, j] == CellType.Wall)
                            mazeRepo += "1";
                        else
                            mazeRepo += "0";
                        if (i != maze.Rows - 1 || j != maze.Cols - 1)
                            mazeRepo += ",";
                        //}
                    }
                }
                return mazeRepo;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="MultiplayerModel"/> is stop.
        /// </summary>
        /// <value>
        ///   <c>true</c> if game stopped; otherwise, <c>false</c>.
        /// </value>
        public bool Stop
        {
            get => stop;
            set { stop = value; NotifyPropertyChanged("Stop"); }
        }

        /// <summary>
        /// Function that meant to be activated when player loses.
        /// </summary>
        public delegate void losingDelegate();

        /// <summary>
        /// Occurs when player lost.
        /// </summary>
        private event losingDelegate LosingEvent;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiplayerModel" /> class.
        /// </summary>
        /// <param name="m">The maze.</param>
        /// <param name="serverSocket">The server socket.</param>
        /// <param name="l">The losing function to bew activated when (and if ;)) player lost.</param>
        public MultiplayerModel(Maze m, TcpClient serverSocket, losingDelegate l)
        {
            stop = false;
            maze = m;
            playerPos = new Position(maze.InitialPos.Row, maze.InitialPos.Col);
            otherPos = new Position(maze.InitialPos.Row, maze.InitialPos.Col);
            this.serverSocket = serverSocket;

            read = new Task(delegate ()
            {
                NetworkStream stream = serverSocket.GetStream();
                BinaryReader reader = new BinaryReader(stream);
                BinaryWriter writer = new BinaryWriter(stream);
                string input;
                while (!Stop)
                {
                    if (Stop)
                    {
                        return;
                    }
                    try
                    {
                        input = reader.ReadString();
                    }
                    catch (Exception)
                    {

                        continue;
                    }
                    if (input == "")
                        continue;
                    if (input.StartsWith("ERROR"))
                    {
                        //Console.WriteLine("ERROR");
                        continue;
                    }
                    else if (input == "CLOSED " + maze.Name)
                    {
                        Stop = true;
                        //Console.WriteLine(input/*"CLOSED " + maze.Name*/);
                        stream.Dispose();
                        serverSocket.Close();
                        return;
                    }
                    else //play move
                    {
                        JObject move = JObject.Parse(input);
                        if (MakeAMove(false, DirectionFromString(move["Direction"].ToString()))) //other won
                        {
                            LosingEvent();
                        }
                    }
                }
            });
            read.Start();

            LosingEvent += l;
        }

        /// <summary>
        /// Makes a move and Notifies that the property has changed.
        /// If <paramref name="thisPlayer"/> is true (meaning this is the player that moves) the function also sends an update to the server.
        /// </summary>
        /// <param name="thisPlayer">if set to <c>true</c> [this player].</param>
        /// <param name="direction">The direction.</param>
        /// <returns>True if said player won, false otherwise.</returns>
        public bool MakeAMove(bool thisPlayer, Direction direction)
        {
            string dir = "";
            if (thisPlayer)
            {
                switch (direction)
                {
                    case Direction.Right:
                        if (playerPos.Col + 1 < maze.Cols && maze[playerPos.Row, playerPos.Col + 1] == CellType.Free)
                        {
                            playerPos.Col += 1;
                            dir = "right";
                        }
                        break;
                    case Direction.Left:
                        if (playerPos.Col - 1 >= 0 && maze[playerPos.Row, playerPos.Col - 1] == CellType.Free)
                        {
                            playerPos.Col -= 1;
                            dir = "left";
                        }
                        break;
                    case Direction.Down:
                        if (playerPos.Row + 1 < maze.Rows && maze[playerPos.Row + 1, playerPos.Col] == CellType.Free)
                        {
                            playerPos.Row += 1;
                            dir = "down";
                        }
                        break;
                    case Direction.Up:
                        if (playerPos.Row - 1 >= 0 && maze[playerPos.Row - 1, playerPos.Col] == CellType.Free)
                        {
                            playerPos.Row -= 1;
                            dir = "up";
                        }
                        break;
                    default:
                        break;
                }

                BinaryWriter writer = new BinaryWriter(serverSocket.GetStream());
                try
                {
                    writer.Write("play " + dir);
                }
                catch (Exception)
                {

                    throw;
                }

                NotifyPropertyChanged("PlayerPos");

                //if (!Stop)
                //    Stop = (playerPos.Row == maze.GoalPos.Row && playerPos.Col == maze.GoalPos.Col);

                return (playerPos.Row == maze.GoalPos.Row && playerPos.Col == maze.GoalPos.Col);
            }
            //otherplayer
            switch (direction)
            {
                case Direction.Right:
                    if (otherPos.Col + 1 < maze.Cols && maze[otherPos.Row, otherPos.Col + 1] == CellType.Free)
                    {
                        otherPos.Col += 1;
                        dir = "right";
                    }
                    break;
                case Direction.Left:
                    if (otherPos.Col - 1 >= 0 && maze[otherPos.Row, otherPos.Col - 1] == CellType.Free)
                    {
                        otherPos.Col -= 1;
                        dir = "left";
                    }
                    break;
                case Direction.Down:
                    if (otherPos.Row + 1 < maze.Rows && maze[otherPos.Row + 1, otherPos.Col] == CellType.Free)
                    {
                        otherPos.Row += 1;
                        dir = "down";
                    }
                    break;
                case Direction.Up:
                    if (otherPos.Row - 1 >= 0 && maze[otherPos.Row - 1, otherPos.Col] == CellType.Free)
                    {
                        otherPos.Row -= 1;
                        dir = "up";
                    }
                    break;
                default:
                    break;
            }

            NotifyPropertyChanged("OtherPos");

            //if (!Stop)
            //    Stop = (otherPos.Row == maze.GoalPos.Row && otherPos.Col == maze.GoalPos.Col);

            return (otherPos.Row == maze.GoalPos.Row && otherPos.Col == maze.GoalPos.Col);
        }

        /// <summary>
        /// Gets a Direction Enum from a string.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>"left" returns Left, "right" returns Right, "up" returns Up and "down" returns Down. Other wise returns Unknown</returns>
        /// <seealso cref="Direction"/>
        private Direction DirectionFromString(string s)
        {
            switch (s)
            {
                case "left":
                    return Direction.Left;
                case "right":
                    return Direction.Right;
                case "up":
                    return Direction.Up;
                case "down":
                    return Direction.Down;
                default:
                    return Direction.Unknown;
            }
        }

        /// <summary>
        /// Closes the game: notifies the server and closes the socket and it's stream.
        /// </summary>
        public void CloseGame()
        {
            if (!Stop)
            {
                Stop = true;

                try
                {
                    BinaryWriter writer = new BinaryWriter(serverSocket.GetStream());
                    writer.Write("close " + maze.Name);
                }
                catch (Exception)
                {
                    //nothing
                }
            }
        }
    }
}
