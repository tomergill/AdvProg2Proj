using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    /// <summary>
    /// Abstract class that implements the ICommand interface, and holds a reference to the Model.
    /// </summary>
    public abstract class AbstractCommand : ICommand
    {
        /// <summary>
        /// Model the command may change or read from.
        /// </summary>
        protected IModel model;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="model">Model the command may change or read from.</param>
        protected AbstractCommand(IModel model)
        {
            this.model = model;
        }

        /// <summary>
        /// Executes the command using the arguments.
        /// </summary>
        /// <param name="args">Arguments for the command.</param>
        /// <param name="client">TcpClient that can be used to send data to.</param>
        /// <returns></returns>
        public abstract string Execute(string[] args, out bool shouldCloseConnection, TcpClient client = null);
    }
}
