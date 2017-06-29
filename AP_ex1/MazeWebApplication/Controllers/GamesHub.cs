using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using MazeWebApplication.Models;
using MazeLib;
using Newtonsoft.Json.Linq;

namespace MazeWebApplication
{
    /// <summary>
    /// Hub for managing multiplayer maze games.
    /// </summary>
    /// <seealso cref="Microsoft.AspNet.SignalR.Hub" />
    public class GamesHub : Hub
    {
        /// <summary>
        /// The manager of the games.
        /// </summary>
        private static IMultiplayerManager manager = new MultiplayerManager();

        /// <summary>
        /// Starts a game.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="rows">The rows.</param>
        /// <param name="cols">The cols.</param>
        /// <returns>true is success, false otherwise</returns>
        public bool StartGame(string name, int rows, int cols)
        {
            return manager.StartGame(name, rows, cols, Context.ConnectionId);
        }

        /// <summary>
        /// Joins the game.
        /// </summary>
        /// <param name="name">The name of the game.</param>
        /// <returns>A JSON representation of the maze.</returns>
        public JObject JoinGame(string name)
        {
            MazeGame m = manager.JoinGame(name, Context.ConnectionId);
            if (m == null)
                return null;

            /* start game in other client */
            Clients.Client(m.GetOtherPlayerId(Context.ConnectionId)).startGame(JObject.Parse(m.MazePlayed.ToJSON()));
            return JObject.Parse(m.MazePlayed.ToJSON());
        }

        /// <summary>
        /// Gets the games.
        /// </summary>
        /// <returns>A list of names of open games that wasn't created by the user.</returns>
        public IEnumerable<string> GetGames()
        {
            return manager.ListGames(Context.ConnectionId);
        }

        /// <summary>
        /// Plays the move.
        /// </summary>
        /// <param name="name">The name of the game.</param>
        /// <param name="direction">The direction of the move. Value is the event.which that was fired in the client.</param>
        public void PlayMove(string name, int direction)
        {
            string otherId = manager.GetOtherPlayerId(Context.ConnectionId, name);
            if (otherId != null)
                Clients.Client(otherId).play(direction);
        }

        /// <summary>
        /// Closes the game.
        /// </summary>
        /// <param name="name">The name of the game.</param>
        public void CloseGame(string name)
        {
            manager.CloseGame(name, Context.ConnectionId);
        }
    }
}