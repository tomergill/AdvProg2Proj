using MazeLib;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MazeWebApplication.Models
{
    /// <summary>
    /// Class represents a game of maze.
    /// </summary>
    public class MazeGame
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MazeGame"/> class.
        /// </summary>
        /// <param name="m">The maze.</param>
        /// <param name="id">The identifier of the user.</param>
        public MazeGame(Maze m, string id)
        {
            MazePlayed = m;
            Player1Id = id;
            Player2Id = null;
        }

        /// <summary>
        /// Gets or sets the maze played.
        /// </summary>
        /// <value>
        /// The maze played.
        /// </value>
        public Maze MazePlayed { get; set; }

        /// <summary>
        /// Gets or sets the first player identifier.
        /// </summary>
        /// <value>
        /// The first player identifier.
        /// </value>
        public string Player1Id { get; set; }

        /// <summary>
        /// Gets or sets the second player identifier.
        /// </summary>
        /// <value>
        /// The second player identifier.
        /// </value>
        public string Player2Id { get; set; }

        /// <summary>
        /// Gets the other player identifier.
        /// </summary>
        /// <param name="id">The identifier of the user.</param>
        /// <returns>The other player id</returns>
        public string GetOtherPlayerId(string id)
        {
            if (Player1Id == id)
                return Player2Id;
            if (Player2Id == id)
                return Player1Id;
            return null;
        }
    }
}