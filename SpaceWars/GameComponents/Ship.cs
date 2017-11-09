using System;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace SpaceWars
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a spaceship.
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    public class Ship : GameComponent
    {
        /// <summary>
        /// The Id of this ship.
        /// </summary>
        [JsonProperty("ship")] private int _ship;

        protected override int GetId()
        {
            return _ship;
        }

        /// <summary>
        /// The name of this ship (aka the player's name).
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; }

        /// <summary>
        /// Determines if the ship is currently thrusting forward.
        /// This is used to change appearance of the ship by adding exhaust.
        /// </summary>
        [JsonProperty("thrust")] private bool _thrusting;

        /// <summary>
        /// The hitpoints remaining for this ship.
        /// Ranges from 0 to 5, where 5 is full health and 0 is temporarily dead (waiting for respawn).
        /// </summary>
        [JsonProperty("hp")]
        public int Health { get; }

        /// <summary>
        /// The score of this ship.
        /// </summary>
        [JsonProperty("score")]
        public int Score { get; }
    }
}