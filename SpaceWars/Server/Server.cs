using System;
using NLog;
using SpaceWars.Properties;

namespace SpaceWars
{
    /// <summary>
    /// The SpaceWars Server Program.
    /// This class is only the "view" of the server; the console.
    /// The controller logic is in a separate project.
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    internal class Server
    {
        /// <summary>
        /// The logger for this program.
        /// </summary>
        private static readonly ILogger Logger = LogManager.GetLogger("Space Wars");

        /// <summary>
        /// The server controller instance attached to this view.
        /// </summary>
        private static SpaceWarsServer _spaceWarsServer;

        /// <summary>
        /// Initializes this static console window.
        /// The console is a singleton, and does not need state,
        /// therefore, every part of it remains static.
        /// </summary>
        private static void Main(string[] args)
        {
            _spaceWarsServer = new SpaceWarsServer();
            Logger.Log(LogLevel.Info, Resources.Server_Log_ServerConnected);

            InitializeLoggingListeners();

            KeepConsoleOpen();
        }

        /// <summary>
        /// Adds any event listeners needed for logging.
        /// </summary>
        private static void InitializeLoggingListeners()
        {
            // Log when the server disconnects.
            _spaceWarsServer.ServerDisconnected += () => Logger.Log(LogLevel.Info, Resources.Server_Log_ServerDisconnected);

            // Log when a client connects.
            _spaceWarsServer.ClientConnected += () => Logger.Log(LogLevel.Info, Resources.Server_Log_ClientConnected);

            // Log when a client disconnects.
            _spaceWarsServer.ClientDisconnected +=
                () => Logger.Log(LogLevel.Info, Resources.Server_Log_ClientDisconnected);
        }

        /// <summary>
        /// Starts an infinite loop that keeps the console window from closing.
        /// This method will block the current thread forever.
        /// </summary>
        private static void KeepConsoleOpen()
        {
            // Read forever
            while (true)
            {
                Console.Read();
            }
        }
    }
}