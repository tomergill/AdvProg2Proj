using MazeLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MazeWebApplication.Models
{
    /// <summary>
    /// An interface for a manager of multiplayer games.
    /// </summary>
    public interface IMultiplayerManager
    {
        /// <summary>
        /// Lists all the open games not created by the user with id.
        /// </summary>
        /// <param name="id">The identifier of the user.</param>
        /// <returns>all the open games</returns>
        IEnumerable<string> ListGames(string id);

        /// <summary>
        /// Starts a game.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="rows">The rows.</param>
        /// <param name="cols">The cols.</param>
        /// <param name="id">The identifier of the user opened the game.</param>
        /// <returns></returns>
        bool StartGame(string name, int rows, int cols, string id);

        /// <summary>
        /// Joins a game.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier of the user joining.</param>
        /// <returns></returns>
        MazeGame JoinGame(string name, string id);

        /// <summary>
        /// Gets the other player identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        string GetOtherPlayerId(string id, string name); //for play

        /// <summary>
        /// Closes the game.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>The id of the other player</returns>
        string CloseGame(string name, string id);
    }
}
