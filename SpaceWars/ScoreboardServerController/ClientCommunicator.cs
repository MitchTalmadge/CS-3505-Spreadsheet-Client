using System;
using Networking;

namespace SpaceWars
{
    /// <summary>
    /// This class keeps track of a single client and handles all required communication to and from that client.
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    internal class ClientCommunicator
    {
        /// <summary>
        /// This static counter ensures that all new instances have a unique id.
        /// </summary>
        private static int _idCounter;

        /// <summary>
        /// The Id of this client communicator.
        /// Used to match up communicators with ships.
        /// </summary>
        public int Id { get; } = _idCounter++;

        /// <summary>
        /// The SpaceWars server instance.
        /// </summary>
        private readonly ScoreboardServerController _scoreboardServerController;

        /// <summary>
        /// The client's SocketState.
        /// </summary>
        private readonly SocketState _state;

        /// <summary>
        /// This event is invoked when the client disconnects.
        /// </summary>
        public event Action Disconnected;

        /// <summary>
        /// Creates an instance from a connected client SocketState.
        /// </summary>
        /// <param name="scoreboardServerController">The scoreboard server controller instance.</param>
        /// <param name="state">The client's SocketState.</param>
        public ClientCommunicator(ScoreboardServerController scoreboardServerController, SocketState state)
        {
            _scoreboardServerController = scoreboardServerController;

            _state = state;

            // Listen for socket state events.
            _state.DataReceived += OnDataReceived;
            _state.Disconnected += OnDisconnected;
        }

        /// <summary>
        /// Asynchronously begins listening for client data.
        /// </summary>
        public void BeginListeningAsync()
        {
            AbstractNetworking.GetData(_state);
        }

        /// <summary>
        /// Called when data is received from the client.
        /// </summary>
        /// <param name="data">The data from the client.</param>
        private void OnDataReceived(string data)
        {
            Console.Out.WriteLine(data);

            AbstractNetworking.GetData(_state);
        }

        /// <summary>
        /// Called when the client disconnects.
        /// </summary>
        private void OnDisconnected()
        {
            // Unsubscribe from event listeners
            _state.DataReceived -= OnDataReceived;
            _state.Disconnected -= OnDisconnected;

            Disconnected?.Invoke();
        }
    }
}