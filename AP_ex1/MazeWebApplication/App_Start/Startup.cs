using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(MazeWebApplication.App_Start.Startup))]

namespace MazeWebApplication.App_Start
{
    /// <summary>
    /// Startup class for signalR and OWIN
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configurations the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
