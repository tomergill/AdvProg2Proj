using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    /// <summary>
    /// Interface for controller of MVC server.
    /// </summary>
    public interface IController
    {
        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="commandLine">The command line.</param>
        /// <param name="shouldCloseConnection">if set to <c>true</c> [should close connection].</param>
        /// <param name="client">The client.</param>
        /// <param name="writer">The writer of the client.</param>
        /// <returns></returns>
        string ExecuteCommand(string commandLine, out bool shouldCloseConnection, TcpClient client, BinaryWriter writer);
    }
}
