using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MazeLib;
using MazeGeneratorLib;
using System.Collections.Concurrent;

namespace MazeWebApplication.Models
{
    /// <summary>
    /// Manages the multiplayer games.
    /// </summary>
    /// <seealso cref="MazeWebApplication.Models.IMultiplayerManager" />
    public class MultiplayerManager : IMultiplayerManager
    {
        /// <summary>
        /// The games' dictionary (key is name)
        /// </summary>
        private static IDictionary<string, MazeGame> games = new ConcurrentDictionary<string, MazeGame>();

        /// <summary>
        /// Gets the other player identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public string GetOtherPlayerId(string id, string name)
        {
            if (id == null || !games.ContainsKey(name))
                return null;
            //MazeGame mg = games.Values.Where((game, x) => game.Player1Id == id || game.Player2Id == id).FirstOrDefault();
            return games[name].GetOtherPlayerId(id);
        }

        /// <summary>
        /// Joins a game.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier of the user joining.</param>
        /// <returns></returns>
        public MazeGame JoinGame(string name, string id)
        {
            if (!games.ContainsKey(name) || games[name].Player2Id != null)
                return null; //game is full or doesn't exist
            games[name].Player2Id = id;
            return games[name];
        }

        /// <summary>
        /// Lists all the open games not created by the user with id.
        /// </summary>
        /// <param name="id">The identifier of the user.</param>
        /// <returns>all the open games</returns>
        public IEnumerable<string> ListGames(string id)
        {
            return games.Where(
                    (game, x) => game.Value.Player2Id == null && game.Value.Player1Id != id
                ).Select((game, x) => game.Key).ToList();
        }

        /// <summary>
        /// Starts a game.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="rows">The rows.</param>
        /// <param name="cols">The cols.</param>
        /// <param name="id">The identifier of the user opened the game.</param>
        /// <returns></returns>
        public bool StartGame(string name, int rows, int cols, string id)
        {
            if (games.ContainsKey(name) || rows > 100 || rows < 2 || cols < 2 || cols > 100 || id == null)
                return false; //bad request

            IMazeGenerator generator = new DFSMazeGenerator();
            Maze m = generator.Generate(rows, cols);
            m.Name = name;

            games[name] = new MazeGame(m, id);
            return true;
        }

        /// <summary>
        /// Closes the game.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>The id of the other player</returns>
        public string CloseGame(string name, string id)
        {
            if (!games.ContainsKey(name) || id == null)
                return null;
            string otherId = games[name].GetOtherPlayerId(id);
            games.Remove(name);
            return otherId;
        }
    }
}