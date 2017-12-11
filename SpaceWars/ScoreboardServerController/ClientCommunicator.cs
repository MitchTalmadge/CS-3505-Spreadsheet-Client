using System;
using System.Text.RegularExpressions;
using Networking;
using SpaceWars.Properties;

namespace SpaceWars
{
    /// <summary>
    /// This class keeps track of a single client and handles all required communication to and from that client.
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    internal class ClientCommunicator
    {
        /// <summary>
        /// The regex pattern for matching URL paths.
        /// </summary>
        private static readonly Regex PathMatchRegex = new Regex(@"^GET ([^?]+)(\?([^=]+)=(.+))? HTTP\/1.1");

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
            // Only accept GET requests
            if (!data.StartsWith("GET"))
            {
                SendResponse(Resources.html_path_options);
                _state.Disconnect();
                return;
            }

            // Get path accessed
            var matches = PathMatchRegex.Matches(data);
            if (matches.Count == 0 || !matches[0].Groups[1].Success)
            {
                // A path could not be found in the request.
                SendResponse(Resources.html_path_options);
                _state.Disconnect();
                return;
            }
            var path = matches[0].Groups[1].Value;

            // Get key and value if applicable.
            var key = matches[0].Groups[3].Success ? matches[0].Groups[3].Value : null;
            var value = matches[0].Groups[4].Success ? matches[0].Groups[4].Value : null;

            // Serve pages based on path.
            switch (path)
            {
                case "/scores":
                    SendResponse(Resources.html_scores);
                    break;
                case "/game":
                    if (key == "id")
                        SendResponse(Resources.html_game);
                    else
                        SendResponse(Resources.html_path_options);
                    break;
                case "/games":
                    if (key == "player")
                        SendResponse(Resources.html_games);
                    else
                        SendResponse(Resources.html_path_options);
                    break;
                default:
                    SendResponse(Resources.html_path_options);
                    break;
            }

            _state.Disconnect();
        }

        /// <summary>
        /// Sends an http response packet to the client, with the given data.
        /// </summary>
        /// <param name="data">The data of the response.</param>
        private void SendResponse(string data)
        {
            AbstractNetworking.Send(
                _state,
                Resources.Scoreboard_HTTP_Response_Prefix + data);
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