using System;
using SpaceWars.Properties;

namespace SpaceWars
{
    /// <summary>
    /// The SpaceWars Server Program.
    /// Starts both a game server and scoreboard server.
    /// This class is only the "view" of the server; the console.
    /// The controller logic is in a separate project.
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    internal class Server
    {
        /// <summary>
        /// The game server instance.
        /// </summary>
        private static GameServer _gameServer;

        /// <summary>
        /// The scoreboard server instance.
        /// </summary>
        private static ScoreboardServer _scoreboardServer;

        /// <summary>
        /// Starts the game server and scoreboard server.
        /// </summary>
        private static void Main(string[] args)
        {
            _gameServer = new GameServer();
            _scoreboardServer = new ScoreboardServer();

            KeepConsoleOpen();
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
                var input = Console.ReadLine();

                // Stop the server when the user types "stop".
                if (input == "stop")
                {
                    _gameServer.Shutdown();
                    _scoreboardServer.Shutdown();
                    
                    Console.WriteLine(Resources.Server_Stopped_ExitMessage);

                    Console.Read();

                    return;
                }
            }
        }
    }
}