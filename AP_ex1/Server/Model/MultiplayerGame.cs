using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeLib;
using System.Net.Sockets;
using System.IO;

namespace Server
{
    /// <summary>
    /// Class representing a multiplayer maze game.
    /// </summary>
    public class MultiplayerGame
    {
        /// <summary>
        /// Reference to the Maze object. Should not be null.
        /// </summary>
        private Maze maze;

        /// <summary>
        /// References to the 2 players. player1 should never be null, player2 may be null until added.
        /// </summary>
        private TcpClient player1, player2;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="maze">Maze reference, NOT NULL!</param>
        /// <param name="player1">First player refernce, NOT NULL!</param>
        /// <param name="player2">Second player reference, should be null.</param>
        public MultiplayerGame(Maze maze, TcpClient player1, TcpClient player2 = null)
        {
            this.maze = maze;
            this.player1 = player1;
            this.player2 = player2;
        }

        /// <summary>
        /// Returns the maze.
        /// </summary>
        public Maze Maze { get => maze; }

        /// <summary>
        /// Returns the first player.
        /// </summary>
        public TcpClient Player1 { get => player1; }

        /// <summary>
        /// Returns the second player, and can set it if it was null before.
        /// </summary>
        public TcpClient Player2
        {
            get => player2;
            set
            {
                if (player2 == null)
                {
                    player2 = value;
                    if (!IsJoinable())
                    {
                        using (NetworkStream stream = player1.GetStream())
                        using (BinaryWriter writer = new BinaryWriter(stream))
                        {
                            writer.Write(maze.ToJSON());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Return the other player.
        /// </summary>
        /// <param name="player">One of the players.</param>
        /// <returns>The other player.</returns>
        public TcpClient GetOtherPlayer(TcpClient player)
        {
            if (player == null)
                return null;
            if (player1 == player)
                return player2;
            if (player2 == player)
                return player1;
            return null;
        }

        /// <summary>
        /// Returns whether the game can be joined.
        /// </summary>
        /// <returns>True if there is an open spot in the game, False otherwise (full).</returns>
        public bool IsJoinable()
        {
            return player1 == null || player2 == null;
        }

        /// <summary>
        /// Returns the name of the maze.
        /// </summary>
        /// <returns>The name of the maze.</returns>
        public string GetMazeName() { return maze.Name; }
    }
}
