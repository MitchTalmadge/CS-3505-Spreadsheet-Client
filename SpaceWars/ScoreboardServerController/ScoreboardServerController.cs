using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Networking;
using MySql.Data.MySqlClient;
using System.Text;

namespace SpaceWars
{
    /// <summary>
    /// The main controller for the SpaceWars scoreboard server.
    /// Uses the networking library to maintain a connection with multiple clients
    /// and output an html webpage that shows scores based on the accessed endpoint.
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    public class ScoreboardServerController
    {
        /// <summary>
        /// The connection string.
        /// The databse our Game Server uses is cs3500_u0980890
        /// </summary>
        public const string connectionString = "server=atr.eng.utah.edu;database=cs3500_u0980890;" +
                                               "uid=cs3500_u0980890;password=burntchickennugget";

        /// <summary>
        /// The TcpState that the server is using to accept client connections.
        /// </summary>
        private TcpState _tcpState;

        /// <summary>
        /// Called when a client connects to the server.
        /// Useful for logging purposes.
        /// </summary>
        public event Action ClientConnected;

        /// <summary>
        /// Called when a client fails to connect to the server.
        /// Useful for logging purposes.
        /// </summary>
        public event Action ClientConnectFailed;

        /// <summary>
        /// Called when a client disconnects from the server.
        /// Useful for logging purposes.
        /// </summary>
        public event Action ClientDisconnected;

        /// <summary>
        /// Called when this server stops listening for clients.
        /// Useful for logging purposes.
        /// </summary>
        public event Action ServerDisconnected;

        /// <summary>
        /// A list of connected client communicators.
        /// </summary>
        private readonly IDictionary<int, ClientCommunicator> _clients =
            new ConcurrentDictionary<int, ClientCommunicator>();

        /// <summary>
        /// Creates a new scoreboard server controller that will listen for clients.
        /// </summary>
        public ScoreboardServerController()
        {
            AcceptConnectionsAsync();
        }

        /// <summary>
        /// Starts the process of accepting client connections in a separate thread.
        /// </summary>
        private void AcceptConnectionsAsync()
        {
            _tcpState = ServerNetworking.AwaitClientConnections(80, ClientConnectionEstablished,
                ClientConnectionFailed);
        }

        /// <summary>
        /// Called when a client establishes a connection with the server.
        /// </summary>
        /// <param name="state">The client's socket state.</param>
        private void ClientConnectionEstablished(SocketState state)
        {
            // Add a new client communicator.
            var communicator = new ClientCommunicator(this, state);
            _clients.Add(communicator.Id, communicator);

            // Handle the case where the client disconnects.
            communicator.Disconnected += () =>
            {
                // Remove the client communicator.
                _clients.Remove(communicator.Id);

                // Notify listeners.
                ClientDisconnected?.Invoke();
            };

            // Start the listening process.
            communicator.BeginListeningAsync();

            // Notify listeners of a newly connected client.
            ClientConnected?.Invoke();
        }

        /// <summary>
        /// Called when the server cannot connect to a client.
        /// </summary>
        /// <param name="reason">The reason that the connection failed.</param>
        private void ClientConnectionFailed(string reason)
        {
            ClientConnectFailed?.Invoke();
        }

        /// <summary>
        /// Generates an html table containing all player scores for all games.
        /// This should include a row for every player in every game. 
        /// 
        /// Each row should contain the ID of the game in which the score took place, the duration of the game, 
        /// the name of the player, the player's score, and the player's accuracy.
        /// 
        /// The HTML table should have one row for each player in each game in the database and one column for 
        /// each of the above required pieces of information. 
        /// 
        /// The table should be ordered by top score.
        /// </summary>
        /// <returns>The table in html format.</returns>
        internal string GenerateScoresTable()
        {
            //Stringbuilder holding data the query returns
            StringBuilder data = new StringBuilder();

            // Open a connection
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    // Open a connection
                    conn.Open();

                    //Command writing Game information (total run time) to database
                    MySqlCommand selectCommand = conn.CreateCommand();
                    selectCommand.CommandText = $"SELECT * FROM Games, Players;";

                    // Execute the command and cycle through the DataReader object
                    using (MySqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        data.Append(
                            "<table><tr><th>Game ID</th><th>Game Duration</th><th>Player Name</th><th>Score</th>" +
                            "<th>Accuracy</th><tr>");
                        while (reader.Read())
                        {
                            data.Append(
                                $"<tr><th>{reader["GameID"].ToString()}</th><th>{reader["Runtime"].ToString()}</th>" +
                                $"<th>{reader["Name"].ToString()}</th><th>{reader["Score"].ToString()}</th><th>" +
                                $"{reader["Accuracy"].ToString()}</th></tr>");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return data.ToString();
        }

        /// <summary>
        /// Generates an html table containing all player scores for a given game.
        /// This should include a row for every player in the particular game. 
        /// 
        /// Each row should contain the ID of the game in which the score took place, the duration of the game, 
        /// the name of the player, the player's score, and the player's accuracy.
        /// 
        /// The HTML table should have one row for each player in the game and one column for 
        /// each of the above required pieces of information.
        ///  
        /// The table should be ordered by top score.
        /// </summary>
        /// <param name="gameId">The ID of the game to generate a table for.</param>
        /// <returns>The table in html format.</returns>
        internal string GenerateGameTable(int gameId)
        {
            //Stringbuilder holding data the query returns
            var data = new StringBuilder();

            // Open a connection
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    // Open a connection
                    conn.Open();

                    //Command getting all players in all games' information, 5 columns total 
                    MySqlCommand selectCommand = conn.CreateCommand();
                    selectCommand.CommandText =
                        $"SELECT * FROM Games, Players WHERE Games.GameID = Players.GameID AND Games.GameID = {gameId};";

                    // Execute the command and cycle create a string to create an html table
                    using (MySqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        data.Append(
                            "<table border=1>" +
                            "<tr>" +
                            "<th>Game ID</th>" +
                            "<th>Game Duration</th>" +
                            "<th>Player Name</th>" +
                            "<th>Score</th>" +
                            "<th>Accuracy</th>" +
                            "<tr>");
                        while (reader.Read())
                        {
                            data.Append("<tr>" +
                                        $"<td>{reader["GameID"].ToString()}</td>" +
                                        $"<td>{reader["Runtime"].ToString()}</td>" +
                                        $"<td>{reader["Name"].ToString()}</td>" +
                                        $"<td>{reader["Score"].ToString()}</td>" +
                                        $"<td>{reader["Accuracy"].ToString()}</td>" +
                                        "</tr>");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                data.Append("</table>");
            }
            return data.ToString();
        }

        /// <summary>
        /// Generates an html table containing scores for all games played by the given player.
        /// This should include a row for every game played by the player. 
        /// 
        /// Each row should contain the ID of the game in which the score took place, the duration of the game, 
        /// the name of the player, the player's score, and the player's accuracy.
        /// 
        /// The HTML table should have one row for each game the player scored in, and one column for 
        /// each of the above required pieces of information. 
        /// 
        /// The table should be ordered by top score.
        /// </summary>
        /// <param name="playerName">The name of the player.</param>
        /// <returns>The table in html format.</returns>
        internal string GenerateGamesTable(string playerName)
        {
            //Stringbuilder holding data the query returns
            StringBuilder data = new StringBuilder();

            // Open a connection
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    // Open a connection
                    conn.Open();

                    //Command getting all players in all games' information, 5 columns total 
                    MySqlCommand selectCommand = conn.CreateCommand();
                    selectCommand.CommandText = $"SELECT * FROM Games, Players WHERE Games.GameID = Players.GameID;";

                    // Execute the command and cycle create a string to create an html table
                    using (MySqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        data.Append(
                            "<table><tr><th>Game ID</th><th>Game Duration</th><th>Player Name</th><th>Score</th>" +
                            "<th>Accuracy</th><tr>");
                        while (reader.Read())
                        {
                            data.Append(
                                $"<tr><th>{reader["GameID"].ToString()}</th><th>{reader["Runtime"].ToString()}</th>" +
                                $"<th>{reader["Name"].ToString()}</th><th>{reader["Score"].ToString()}</th><th>" +
                                $"{reader["Accuracy"].ToString()}</th></tr>");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return data.ToString();
        }

        /// <summary>
        /// Disconnects from the TcpState that accepts client connections.
        /// This server instance may not be used after calling this method.
        /// </summary>
        public void Disconnect()
        {
            _tcpState?.StopAcceptingClientConnections();
            ServerDisconnected?.Invoke();
        }
    }
}