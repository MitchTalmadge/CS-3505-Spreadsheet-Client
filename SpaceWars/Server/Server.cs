using System;
using System.IO;
using NLog;
using Properties;
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
        /// The path to the server's properties file.
        /// </summary>
        private const string PropertiesFilePath = "server_properties.xml";

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
            InitializeProperties();
            
            _spaceWarsServer = new SpaceWarsServer();
            Logger.Log(LogLevel.Info, Resources.Server_Log_ServerConnected);

            InitializeLoggingListeners();

            KeepConsoleOpen();
        }

        /// <summary>
        /// Initializes the properties file and checks for errors.
        /// Also checks for missing properties and writes their default values.
        /// </summary>
        private static void InitializeProperties()
        {
            try
            {
                var properties = new PropertiesFile(PropertiesFilePath);
            }
            catch (IOException)
            {
                // Log error and quit.
                Logger.Log(LogLevel.Fatal, Resources.Server_Log_PropertiesLoadFailed);
                Console.Read();
                Environment.Exit(-1);
            }
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