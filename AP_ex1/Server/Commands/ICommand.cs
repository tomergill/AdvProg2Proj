using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    /// <summary>
    /// Interface for commands the server receives from the client.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Executes the command using the arguments.
        /// </summary>
        /// <param name="args">Arguments for the command.</param>
        /// <param name="client">TcpClient that can be used to send data to.</param>
        /// <returns></returns>
        string Execute(string[] args, out bool shouldCloseConnection, TcpClient client = null);
    }
}
