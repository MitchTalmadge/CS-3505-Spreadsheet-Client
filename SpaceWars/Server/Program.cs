using System;
using NLog;

namespace SpaceWars
{
    /// <summary>
    /// The SpaceWars Server Program.
    /// This class is only the "view" of the server; the console.
    /// The controller logic is in a separate project.
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    internal class Program
    {
        /// <summary>
        /// The logger for this program.
        /// </summary>
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The server controller instance attached to this view.
        /// </summary>
        private static readonly SpaceWarsServer Server = new SpaceWarsServer();

        /// <summary>
        /// Initializes this static console window.
        /// The console is a singleton, and does not need state; 
        /// therefore every part of it remains static.
        /// </summary>
        private static void Main(string[] args)
        {
            Logger.Log(LogLevel.Info, "Server Started Successfully.");

            // Prevent closing of console.
            while (true)
            {
                Console.Read();
            }
        }
    }
}