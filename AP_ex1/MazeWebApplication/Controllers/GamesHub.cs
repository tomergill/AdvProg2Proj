using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using MazeWebApplication.Models;
using MazeLib;

namespace MazeWebApplication
{
    public class GamesHub : Hub
    {
        private static IMultiplayerManager manager = new MultiplayerManager();

        public bool StartGame(string name, int rows, int cols)
        {
            return manager.StartGame(name, rows, cols, Context.ConnectionId);
        }

        public Maze JoinGame(string name)
        {
            MazeGame m = manager.JoinGame(name, Context.ConnectionId);
            if (m == null)
                return null;
            Clients.Client(m.GetOtherPlayerId(Context.ConnectionId)).startGame(m.MazePlayed);
            return m.MazePlayed;
        }
    }
}