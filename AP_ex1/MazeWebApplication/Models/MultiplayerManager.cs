using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MazeLib;
using MazeGeneratorLib;

namespace MazeWebApplication.Models
{
    public class MultiplayerManager : IMultiplayerManager
    {
        private static IDictionary<string, MazeGame> games = new Dictionary<string, MazeGame>();

        public string GetOtherPlayerId(string name, String id)
        {
            if (id == null || !games.ContainsKey(name))
                return null;
            //MazeGame mg = games.Values.Where((game, x) => game.Player1Id == id || game.Player2Id == id).FirstOrDefault();
            return games[name].GetOtherPlayerId(id);
        }

        public MazeGame JoinGame(string id, string name)
        {
            if (!games.ContainsKey(name) || games[name].Player2Id != null)
                return null; //game is full
            games[name].Player2Id = id;
            return games[name];
        }

        public IEnumerable<string> ListGames()
        {
            return ((Dictionary<string, MazeGame>)games.Where(
                    (game, x) => game.Value.Player2Id == null
                )).Keys.ToList();
        }

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