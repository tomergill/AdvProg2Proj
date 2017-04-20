using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGeneratorLib;
using System.Net.Sockets;
using System.IO;

namespace Server
{
    /// <summary>
    /// Controller of the MVC server.
    /// </summary>
    public class Controller : IController
    {
        /// <summary>
        /// Dictionary that keeps commands by their names.
        /// </summary>
        private Dictionary<string, ICommand> commands;

        /// <summary>
        /// Model to be changed/read by the controller.
        /// </summary>
        private IModel model;

        /// <summary>
        /// Ctor.
        /// </summary>
        public Controller(IModel model)
        {
            this.model = model;
            commands = new Dictionary<string, ICommand>
            {
                { "generate", new GenerateMazeCommand(model) },
                { "solve", new SolveMazeCommand(model) },
                { "start", new StartMultiplayerGameCommand(model) },
                { "list", new ListCommand(model) },
                { "join", new JoinGameCommand(model) },
                { "play", new PlayMoveCommand(model) },
                { "close", new CloseGameCommand(model) }
            };
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="commandLine">Command line received from client.</param>
        /// <param name="client">Socket to client.</param>
        /// <returns>Output of the command, or "Command not found".</returns>
        public string ExecuteCommand(string commandLine, out bool shouldCloseConnection, TcpClient client, BinaryWriter writer)
        {
            string[] arr = commandLine.Split(' ');
            string commandKey = arr[0];
            if (!commands.ContainsKey(commandKey))
            {
                shouldCloseConnection = true;
                return "ERROR";
            }
            string[] args = arr.Skip(1).ToArray();
            ICommand command = commands[commandKey];
            return command.Execute(args, out shouldCloseConnection, client, writer);
        }
    }
}
