using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeLib;
using System.Net.Sockets;
using SearchAlgorithmsLib;

namespace Server
{
    /// <summary>
    /// Interface for the model part of the MVC server.
    /// </summary>
    public interface IModel
    {
        /// <summary>
        /// Generates a Maze using the requested parameters and saves it.
        /// </summary>
        /// <param name="name">Name of the maze</param>
        /// <param name="rows">Number of rows. rows > 0</param>
        /// <param name="cols">Number of columns. cols > 0</param>
        /// <returns>The newly created Maze.</returns>
        Maze GenerateMaze(string name, int rows, int cols);

        /// <summary>
        /// Returns the requested maze.
        /// </summary>
        /// <param name="name">Name of the maze.</param>
        /// <returns>Requested maze, or null if doesn't exist.</returns>
        Maze GetMazeByName(string name);

        /// <summary>
        /// 
        /// </summary>
        /// <returns>List of all the games that can be joined to.</returns>
        List<string> GetOpenGamesList();

        /// <summary>
        /// Adds a new multiplayer game.
        /// </summary>
        /// <param name="player">First player of the game.</param>
        /// <param name="maze">Maze to play in.</param>
        MultiplayerGame AddMultiplayerGame(TcpClient player, string name, int rows, int cols);

        /// <summary>
        /// Joins the mutiplayer game requested.
        /// </summary>
        /// <param name="name">Name of the maze to join.</param>
        /// <param name="newPlayer">New player to add to the game.</param>
        /// <returns>The multiplayer game if exists, otherwise null.</returns>
        MultiplayerGame JoinMultiplayerGame(string name, TcpClient newPlayer);

        /// <summary>
        /// Gets the game the player currently in.
        /// </summary>
        /// <param name="player">Player playing the game.</param>
        /// <returns>The multiplayer gmae if the player plays one, otherwise null.</returns>
        MultiplayerGame GetMultiplayerGameByPlayer(TcpClient player);

        /// <summary>
        /// Closes the game, and returns it (it's information).
        /// </summary>
        /// <param name="name">Name of the game to shut down.</param>
        /// <returns>The requested game, or null if doesn't exist.</returns>
        MultiplayerGame CloseGame(string name);

        SolutionWithNodesEvaluated<Position> SolveMaze(string name, int algoId);
    }
}
