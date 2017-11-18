using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;

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
        /// The ID of the current player.
        /// </summary>
        public int PlayerId { get; }

        /// <summary>
        /// Contains dictionaries that map game components to their ids. 
        /// </summary>
        private readonly IServiceContainer _gameComponents = new ServiceContainer();

        /// <summary>
        /// Creates a World instance with the given size and player id.
        /// </summary>
        /// <param name="size">The dimensions of the game world (both sides use same length).</param>
        /// <param name="playerId">The ID of the current player.</param>
        public World(int size, int playerId)
        {
            Size = size;
            PlayerId = playerId;

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
            var method = GetType().GetMethod("AddGameComponentsTypeMapping", BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var subclass in subclasses)
            {
                method?.MakeGenericMethod(subclass).Invoke(this, null);
            }
        }

        private void AddGameComponentsTypeMapping<T>() where T : GameComponent
        {
            _gameComponents.AddService(typeof(Dictionary<int, T>), new Dictionary<int, T>());
        }

        /// <summary>
        /// Adds or replaces an existing game component with the provided game component, where the component's id determines equality.
        /// </summary>
        /// <param name="component">The game component to update.</param>
        public void UpdateComponent(GameComponent component)
        {
            var method = GetType().GetMethod("GetComponentDictionary", BindingFlags.NonPublic | BindingFlags.Instance);
            dynamic dict = method?.MakeGenericMethod(component.GetType()).Invoke(this, null);
            if (dict != null)
                dict[component.Id] = component;
        }

        /// <summary>
        /// Removes the provided game component from the world, if it exists.
        /// </summary>
        /// <param name="component">The game component to remove.</param>
        public void RemoveComponent(GameComponent component)
        {
            var method = GetType().GetMethod("GetComponentDictionary", BindingFlags.NonPublic | BindingFlags.Instance);
            dynamic dict = method?.MakeGenericMethod(component.GetType()).Invoke(this, null);
            if (dict != null)
                dict.Remove(component.Id);
        }

        private Dictionary<int, T> GetComponentDictionary<T>() where T : GameComponent
        {
            return (Dictionary<int, T>) _gameComponents.GetService(typeof(Dictionary<int, T>));
        }

        public IEnumerable<T> GetComponents<T>() where T : GameComponent
        {
            return ((Dictionary<int, T>) _gameComponents.GetService(typeof(Dictionary<int, T>))).Values.ToArray();
        }
    }
}