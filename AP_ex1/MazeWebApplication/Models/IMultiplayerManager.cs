﻿using MazeLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MazeWebApplication.Models
{
    public interface IMultiplayerManager
    {
        IEnumerable<string> ListGames(string id);
        bool StartGame(string name, int rows, int cols, string id);
        MazeGame JoinGame(string name, string id);
        //void Play(string name, Direction dir, int id);
        string GetOtherPlayerId(string id, string name); //for play
        string CloseGame(string name, string id);
    }
}