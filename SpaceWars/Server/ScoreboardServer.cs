using NLog;
using SpaceWars.Properties;

namespace SpaceWars
{
    /// <summary>
    /// The Scoreboard Server listens on port 80
    /// and listens for web clients, serving the scoreboard from the database.
    /// </summary>
    internal class ScoreboardServer
    {
        /// <summary>
        /// The logger for this program.
        /// </summary>
        private static readonly ILogger Logger = LogManager.GetLogger("Scoreboard Server");

        /// <summary>
        /// Initializes this static console window.
        /// The console is a singleton, and does not need state,
        /// therefore, every part of it remains static.
        /// </summary>
        internal ScoreboardServer()
        {
            Logger.Log(LogLevel.Info, Resources.ScoreServer_Log_ServerConnected);

            InitializeLoggingListeners();
        }

        /// <summary>
        /// Adds any event listeners needed for logging.
        /// </summary>
        private static void InitializeLoggingListeners()
        {
        }

    }
}
