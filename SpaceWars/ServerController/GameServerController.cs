using System;
using System.Collections.Generic;
using Networking;
using System.Collections.Concurrent;
using MySql.Data.MySqlClient;

namespace SpaceWars
{
    /// <summary>
    /// The main controller for the SpaceWars game server.
    /// Uses the networking library to maintain a connection with multiple clients
    /// and update the world state, notifying clients of changes in state.
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    public partial class GameServerController
    {
        /// <summary>
        /// The configuration for this server.
        /// </summary>
        internal GameServerConfiguration Configuration { get; }

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
        /// A list of connected client communicators.
        /// </summary>
        private readonly IDictionary<int, ClientCommunicator> _clients = new ConcurrentDictionary<int, ClientCommunicator>();

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
        /// Tracks the starting time (or ticks) when a Server/game starts.
        /// Used to compute total game time to save to the database. 
        /// </summary>
        private int StartTicks;

        /// <summary>
        /// Tracks the ending time (or ticks) when a Server/game ends in the Disconnect method.
        /// Used to compute total game time to save to the database. 
        /// </summary>
        private int EndTicks;

        /// <summary>
        /// Creates a new game server controller that will listen for clients.
        /// </summary>
        public GameServerController(GameServerConfiguration configuration)
        {
            StartTicks = Environment.TickCount;
            Configuration = configuration;
            AcceptConnectionsAsync();
            StartGameLoopAsync();
        }

        /// <summary>
        /// Starts the process of accepting client connections in a separate thread.
        /// </summary>
        private void AcceptConnectionsAsync()
        {
            _tcpState = ServerNetworking.AwaitClientConnections(11000, ClientConnectionEstablished, ClientConnectionFailed);
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

            // Create a ship when the client sends their nickname.
            communicator.NicknameReceived += nickname =>
            {
                var ship = new Ship(communicator.Id, nickname);
                _world.PutComponent(ship);
            };

            // Handle the case where the client disconnects.
            communicator.Disconnected += () =>
            {
                // Remove the client's ship.
                _world.GetComponent<Ship>(communicator.Id).Health = 0;

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
        /// Disconnects from the TcpState that accepts client connections.
        /// This server instance may not be used after calling this method.
        /// Calls the method that writes data to the Game's MySQL database. 
        /// </summary>
        public void Disconnect()
        {
            EndTicks = Environment.TickCount;
            _tcpState?.StopAcceptingClientConnections();
            StopGameLoop();
            ServerDisconnected?.Invoke();
            writeToDatabase();
        }

        /// <summary>
        /// Writes each game's total time duration to the database, as well
        /// as each player's maximum score and projectile accuracy.
        /// </summary>
        private void writeToDatabase()
        {
            int gameTime = StartTicks - EndTicks;

            // Open a connection
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    // Open a connection
                    conn.Open();

                    //Command writing Game information (total run time) to database
                    MySqlCommand gameCommand = conn.CreateCommand();
                    gameCommand.CommandText = $"INSERT INTO Games (Runtime) VALUES ({gameTime.ToString()})";

                    // Execute the command and cycle through the DataReader object
                    using (MySqlDataReader reader = gameCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine(reader);
                        }
                    }

                    //Command writing all player information for each player in each client
                    foreach (var ship in _world.GetComponents<Ship>())
                    {
                        MySqlCommand playersCommand = conn.CreateCommand();
                        playersCommand.CommandText = $"INSERT INTO Players (ShipID, GameID, Score, Accuracy, Name) " +
                            $"VALUES ({ship.Id.ToString()}, {"GET GAMEID FROM OTHER TABLE"}, {ship.Score.ToString()}," +
                            $"{ship.getAccuracy().ToString()}, {ship.Name})";

                        // Execute the command and cycle through the DataReader object
                        using (MySqlDataReader reader = playersCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine(reader);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}