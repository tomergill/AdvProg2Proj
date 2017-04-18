using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeLib;
using MazeGeneratorLib;
using System.Net.Sockets;
using SearchAlgorithmsLib;

namespace Server
{
    /// <summary>
    /// Model of the MVC server.
    /// </summary>
    public class Model : IModel
    {
        /// <summary>
        /// Dictionary that saves the mazes by thier name.
        /// </summary>
        private Dictionary<string, Maze> singleplayerMazes;

        /// <summary>
        /// List that keeps names of Mazes that are open to join.
        /// </summary>
        private List<MultiplayerGame> multiplayerGames;

        private Dictionary<string, SolutionWithNodesEvaluated<Position>> solutions;

        /// <summary>
        /// An object that generates mazes.
        /// </summary>
        private IMazeGenerator generator;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="generator">Maze Generator to be used by the Model.</param>
        public Model(IMazeGenerator generator)
        {
            singleplayerMazes = new Dictionary<string, Maze>();
            this.generator = generator;
            multiplayerGames = new List<MultiplayerGame>();
            solutions = new Dictionary<string, SolutionWithNodesEvaluated<Position>>();
        }

        /// <summary>
        /// Adds a new multiplayer game.
        /// </summary>
        /// <param name="player">First player of the game.</param>
        /// <param name="maze">Maze to play in.</param>
        public MultiplayerGame AddMultiplayerGame(TcpClient player, string name, int rows, int cols)
        {
            MultiplayerGame game = null;
            if (null == GetMultiplayerGameByName(name))
            {
                game = new MultiplayerGame(player);
                game.Maze = generator.Generate(rows, cols);
                game.Maze.Name = name;
                multiplayerGames.Add(game);
            }
            return game;
        }

        /// <summary>
        /// Generates a Maze using the requested parameters and saves it.
        /// </summary>
        /// <param name="name">Name of the maze</param>
        /// <param name="rows">Number of rows. rows > 0</param>
        /// <param name="cols">Number of columns. cols > 0</param>
        /// <returns>The newly created Maze.</returns>
        public Maze GenerateMaze(string name, int rows, int cols)
        {
            if (singleplayerMazes.ContainsKey(name))
                return singleplayerMazes[name];
            Maze m = generator.Generate(rows, cols);
            m.Name = name;
            singleplayerMazes.Add(name, m);
            return m;
        }

        /// <summary>
        /// Returns the requested maze.
        /// </summary>
        /// <param name="name">Name of the maze.</param>
        /// <returns>Requested maze, or null if doesn't exist.</returns>
        public Maze GetMazeByName(string name)
        {
            if (!singleplayerMazes.ContainsKey(name))
                return null;
            return singleplayerMazes[name];
        }

        /// <summary>
        /// Gets the game the player currently in.
        /// </summary>
        /// <param name="player">Player playing the game.</param>
        /// <returns>The multiplayer gmae if the player plays one, otherwise null.</returns>
        public MultiplayerGame GetMultiplayerGameByPlayer(TcpClient player)
        {
            foreach (MultiplayerGame game in multiplayerGames)
                if (game.Player1 == player || game.Player2 == player)
                    return game;
            return null;
        }

        /// <summary>
        /// Gets the game with the specified game
        /// </summary>
        /// <param name="name">Name of the maze.</param>
        /// <returns>The multiplayer gmae if there is one by this name, otherwise null.</returns>
        private MultiplayerGame GetMultiplayerGameByName(string name)
        {
            foreach (MultiplayerGame game in multiplayerGames)
                if (game.GetMazeName() == name)
                    return game;
            return null;
        }

        /// <summary>
        /// Returns List of all the games that can be joined to.
        /// </summary>
        /// <returns>List of all the games that can be joined to.</returns>
        public List<string> GetOpenGamesList()
        {
            List<string> list = new List<string>();
            foreach (MultiplayerGame game in multiplayerGames)
            {
                if (game.IsJoinable())
                    list.Add(game.GetMazeName());
            }
            return list;
        }

        /// <summary>
        /// Joins the mutiplayer game requested.
        /// </summary>
        /// <param name="name">Name of the maze to join.</param>
        /// <param name="newPlayer">New player to add to the game.</param>
        /// <returns>The multiplayer game if exists, otherwise null.</returns>
        public MultiplayerGame JoinMultiplayerGame(string name, TcpClient newPlayer)
        {
            MultiplayerGame game = GetMultiplayerGameByName(name);
            if (game == null || newPlayer == null)
                return null;
            game.Player2 = newPlayer;
            return game;
        }

        /// <summary>
        /// Closes the game, and returns it (it's information).
        /// </summary>
        /// <param name="name">Name of the game to shut down.</param>
        /// <returns>The requested game, or null if doesn't exist.</returns>
        public MultiplayerGame CloseGame(string name)
        {
            MultiplayerGame game = GetMultiplayerGameByName(name);
            if (game == null)
                return null;
            multiplayerGames.Remove(game);
            return game;
        }

        public SolutionWithNodesEvaluated<Position> SolveMaze(string name, int algoId)
        {
            if (solutions.ContainsKey(name))
                return solutions[name];
            Maze maze = GetMazeByName(name);
            if (maze == null)
                return null;
            SolutionWithNodesEvaluated<Position> sol;
            if (algoId == 0)
            {
                BfsAlgorithm<Position> bfs = new BfsAlgorithm<Position>();
                Solution<Position> temp = bfs.Search(new ObjectAdapter(maze));
                solutions.Add(name, new SolutionWithNodesEvaluated<Position>(temp, bfs.GetNumberOfNodesEvaluated()));
                return solutions["name"];
            }
            else
            {
                DfsAlgorithm<Position> dfs = new DfsAlgorithm<Position>();
                Solution<Position> temp = dfs.Search(new ObjectAdapter(maze));
                solutions.Add(name, new SolutionWithNodesEvaluated<Position>(temp, dfs.GetNumberOfNodesEvaluated()));
                return solutions["name"];
            }
        }
    }
}
