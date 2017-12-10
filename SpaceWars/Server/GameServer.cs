using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Properties;
using SpaceWars.Properties;

namespace SpaceWars
{
    /// <summary>
    /// The Game Server listens on port 11000
    /// and listens for game clients, updating the world according to its state.
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    internal class GameServer
    {
        /// <summary>
        /// The logger for this program.
        /// </summary>
        private static readonly ILogger Logger = LogManager.GetLogger("Game Server");

        /// <summary>
        /// The path to the server's properties file.
        /// </summary>
        private const string PropertiesFilePath = "server_properties.xml";

        /// <summary>
        /// The server controller instance attached to this view.
        /// </summary>
        private static GameServerController _gameServerController;

        /// <summary>
        /// Initializes the game server controller and logger.
        /// </summary>
        internal GameServer()
        {
            var configuration = InitializeProperties();

            // Configure and create server instance.
            _gameServerController = new GameServerController(configuration);

            Logger.Log(LogLevel.Info, Resources.GameServer_Log_ServerConnected);

            InitializeLoggingListeners();
        }

        /// <summary>
        /// Initializes the properties file and checks for errors.
        /// Also checks for missing properties and writes their default values.
        /// </summary>
        private static GameServerConfiguration InitializeProperties()
        {
            // If there is no properties file, generate one.
            if (!File.Exists(PropertiesFilePath))
            {
                // Create a new properties file.
                var properties = new PropertiesFile(PropertiesFilePath);

                // Create a config with defaults.
                var config = new GameServerConfiguration();

                // Write defaults to properties file.
                properties.SetProperties(config.ToProperties());

                return config;
            }

            // A properties file exists, so try to read it.
            try
            {
                // Read the properties file.
                var properties = new PropertiesFile(PropertiesFilePath);

                // Create a config with defaults.
                var config = new GameServerConfiguration();

                // Populate the config with the properties file.
                config.FromProperties(properties.GetAllProperties());

                // Write the config back to the properties file, in case any defaults were inferred.
                properties.SetProperties(config.ToProperties());

                return config;
            }
            catch (IOException)
            {
                // Could not load properties file.
                // Log error and quit.
                Logger.Log(LogLevel.Fatal, Resources.GameServer_Log_PropertiesLoadFailed);
                Console.Read();
                Environment.Exit(-1);
            }

            return null;
        }

        /// <summary>
        /// Adds any event listeners needed for logging.
        /// </summary>
        private static void InitializeLoggingListeners()
        {
            // Log when the server disconnects.
            _gameServerController.ServerDisconnected +=
                () => Logger.Log(LogLevel.Info, Resources.GameServer_Log_ServerDisconnected);

            // Log when a client connects.
            _gameServerController.ClientConnected += () => Logger.Log(LogLevel.Info, Resources.GameServer_Log_ClientConnected);

            // Log when a client fails to connect.
            _gameServerController.ClientConnectFailed +=
                () => Logger.Log(LogLevel.Warn, Resources.GameServer_Log_ClientConnectFailed);

            // Log when a client disconnects.
            _gameServerController.ClientDisconnected +=
                () => Logger.Log(LogLevel.Info, Resources.GameServer_Log_ClientDisconnected);
        }
    }
}
