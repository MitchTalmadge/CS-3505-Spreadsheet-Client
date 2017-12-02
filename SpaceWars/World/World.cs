using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceWars
{
    /// <summary>
    /// This class represents the entire game world, including all game components (ships, stars, etc.)
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    public class World
    {
        /// <summary>
        /// The dimensions of the game world (both sides use same length).
        /// </summary>
        public int Size { get; }

        /// <summary>
        /// Contains dictionaries that map game components to their ids. 
        /// </summary>
        private readonly IDictionary<Type, Dictionary<int, GameComponent>> _gameComponents =
            new Dictionary<Type, Dictionary<int, GameComponent>>();

        /// <summary>
        /// Creates a World instance with the given size and player id.
        /// </summary>
        /// <param name="size">The dimensions of the game world (both sides use same length).</param>
        public World(int size)
        {
            Size = size;
            InitializeGameComponentsDictionary();
        }

        /// <summary>
        /// Initializes the dictionary that holds all game components.
        /// Scans for subclasses of the GameComponent class and creates a mapping for each subclass.
        /// </summary>
        private void InitializeGameComponentsDictionary()
        {
            // Find all subclasses of GameComponent
            var subclasses = typeof(GameComponent).Assembly.GetTypes()
                .Where(type => type.IsSubclassOf(typeof(GameComponent)));

            // Create a dictionary mapping component ids to instances for each subclass.
            foreach (var subclass in subclasses)
            {
                _gameComponents[subclass] = new Dictionary<int, GameComponent>();
            }
        }

        /// <summary>
        /// Adds or replaces an existing game component with the provided game component, where the component's id determines equality.
        /// </summary>
        /// <param name="component">The game component to update.</param>
        public void UpdateComponent(GameComponent component)
        {
            var dict = _gameComponents[component.GetType()];
            dict[component.Id] = component;
        }

        /// <summary>
        /// Removes the provided game component from the world, if it exists.
        /// </summary>
        /// <param name="component">The game component to remove.</param>
        public void RemoveComponent(GameComponent component)
        {
            var dict = _gameComponents[component.GetType()];
            dict.Remove(component.Id);
        }

        /// <summary>
        /// Retrieves an IEnumerable of game components of a given type from this world.
        /// </summary>
        /// <typeparam name="T">The type of game component to enumerate over.</typeparam>
        /// <returns>An IEnumerable containing only components of the given type.</returns>
        public IEnumerable<T> GetComponents<T>() where T : GameComponent
        {
            return _gameComponents[typeof(T)].Values.OfType<T>();
        }

        /// <summary>
        /// Retrieves a GameComponent given the type and ID. 
        /// </summary>
        /// <typeparam name="T">The type of game component to get.</typeparam>
        /// <param name="ID">The ID of the game component to remove.</param>
        /// <returns>The specified GameComponent.</returns>
        public GameComponent GetComponent<T>(int ID) where T : GameComponent
        {
            if (_gameComponents[typeof(T)].TryGetValue(ID, out var component))
            {
                return component;
            }
            return null;
        }

        /// <summary>
        /// Finds a location in the world not occupied by a ship or another star.
        /// </summary>
        /// <returns>A Vector2D representing a location where a ship may safely spawn.</returns>
        public Vector2D FindShipSpawnLocation(double starCollisionRadius, double shipCollisionRadius)
        {
            var random = new Random();
            // This is how far ships and stars must be apart. The star collision radius is multipled by 2 to give a "buffer".
            var minShipDistanceToStar = starCollisionRadius * 2 + shipCollisionRadius;

            // Attempt to find a location 100 times.
            for (var i = 0; i < 100; i++)
            {
                // Pick a random location
                var randX = random.NextDouble() * Size - Size / 2d;
                var randY = random.NextDouble() * Size - Size / 2d;
                var spawnVector = new Vector2D(randX, randY);

                // Check if the location is occupied by stars
                var stars = GetComponents<Star>().ToArray();

                // If there are no stars, we can spawn anywhere.
                if (stars.Length == 0)
                    return spawnVector;

                // Otherwise, check for collisions.
                foreach (var star in GetComponents<Star>())
                {
                    // Compute the distance between the vectors.
                    var distanceVector = star.Location - spawnVector;

                    // Make sure the star and ship collision radius "bubbles" do not collide.
                    if (distanceVector.Length() < minShipDistanceToStar)
                        continue;

                    return spawnVector;
                }
            }

            // If we failed to find an empty space, we return the upper left corner as a fallback.
            return new Vector2D(-Size / 2d, -Size / 2d);
        }
    }
}