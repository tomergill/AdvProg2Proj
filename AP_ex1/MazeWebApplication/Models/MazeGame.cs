using MazeLib;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MazeWebApplication.Models
{
    public class MazeGame
    {
        public MazeGame(Maze m, string id)
        {
            MazePlayed = m;
            Player1Id = id;
            Player2Id = null;
        }

        public Maze MazePlayed { get; set; }
        public string Player1Id { get; set; }
        public string Player2Id { get; set; }

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