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
    public class GamesHub : Hub
    {
        private static IMultiplayerManager manager = new MultiplayerManager();

        public bool StartGame(string name, int rows, int cols)
        {
            return manager.StartGame(name, rows, cols, Context.ConnectionId);
        }

        public JObject JoinGame(string name)
        {
            MazeGame m = manager.JoinGame(name, Context.ConnectionId);
            if (m == null)
                return null;
            Clients.Client(m.GetOtherPlayerId(Context.ConnectionId)).startGame(JObject.Parse(m.MazePlayed.ToJSON()));
            return JObject.Parse(m.MazePlayed.ToJSON());
        }

        public IEnumerable<string> GetGames()
        {
            return manager.ListGames();
        }

        public void PlayMove(string name, int direction)
        {
            string otherId = manager.GetOtherPlayerId(Context.ConnectionId, name);
            if (otherId != null)
                Clients.Client(otherId).play(direction);
        }

        public void CloseGame(string name)
        {
            manager.CloseGame(name, Context.ConnectionId);
        }
    }
}