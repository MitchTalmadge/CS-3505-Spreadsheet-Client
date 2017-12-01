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
        private readonly IDictionary<Type, Dictionary<int, GameComponent>> _gameComponents = new Dictionary<Type, Dictionary<int, GameComponent>>();

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
    }
}