using NLog;
using SpaceWars.Properties;

namespace SpaceWars
{
    /// <summary>
    /// The Scoreboard Server listens on port 80
    /// and listens for web clients, serving the scoreboard from the database.
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    internal class ScoreboardServer
    {
        /// <summary>
        /// The logger for this program.
        /// </summary>
        private static readonly ILogger Logger = LogManager.GetLogger("Scoreboard Server");

        /// <summary>
        /// The server controller instance attached to this view.
        /// </summary>
        private static ScoreboardServerController _scoreboardServerController;

        /// <summary>
        /// Initializes the scoreboard server controller and logger.
        /// </summary>
        internal ScoreboardServer()
        {
            _scoreboardServerController = new ScoreboardServerController();

            Logger.Log(LogLevel.Info, Resources.ScoreServer_Log_ServerConnected);

            InitializeLoggingListeners();
        }

        /// <summary>
        /// Shuts down the scoreboard server.
        /// </summary>
        internal void Shutdown()
        {
            _scoreboardServerController.Disconnect();
        }

        /// <summary>
        /// Adds any event listeners needed for logging.
        /// </summary>
        private static void InitializeLoggingListeners()
        {
            // Log when the server disconnects.
            _scoreboardServerController.ServerDisconnected +=
                () => Logger.Log(LogLevel.Info, Resources.ScoreServer_Log_ServerDisconnected);

            // Log when a client connects.
            _scoreboardServerController.ClientConnected += () => Logger.Log(LogLevel.Info, Resources.ScoreServer_Log_ClientConnected);

            // Log when a client fails to connect.
            _scoreboardServerController.ClientConnectFailed +=
                () => Logger.Log(LogLevel.Warn, Resources.ScoreServer_Log_ClientConnectFailed);

            // Log when a client disconnects.
            _scoreboardServerController.ClientDisconnected +=
                () => Logger.Log(LogLevel.Info, Resources.ScoreServer_Log_ClientDisconnected);
        }

    }
}
