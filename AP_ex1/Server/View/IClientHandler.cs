using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    /// <summary>
    /// Interface for client handling.
    /// </summary>
    public interface IClientHandler
    {
        /// <summary>
        /// Handles the client.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="controller">The controller.</param>
        void HandleClient(TcpClient client, IController controller);

        /// <summary>
        /// Stops this instance.
        /// </summary>
        void Stop();
    }
}
