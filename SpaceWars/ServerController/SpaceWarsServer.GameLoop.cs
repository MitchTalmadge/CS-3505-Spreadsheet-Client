using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWars
{
    /// <summary>
    /// This section of the server handles the game loop and its logic.
    /// </summary>
    public partial class SpaceWarsServer
    {
        /// <summary>
        /// The game loop for this server.
        /// </summary>
        private GameLoop _gameLoop;

        /// <summary>
        /// Starts the game loop for the server in another thread.
        /// </summary>
        private void StartGameLoopAsync()
        {
            _gameLoop = new GameLoop(Configuration.MsPerFrame, OnTick);
        }

        /// <summary>
        /// Stops the game loop at the end of the current tick.
        /// </summary>
        private void StopGameLoop()
        {
            _gameLoop.Running = false;
        }

        /// <summary>
        /// Called every tick of the game loop.
        /// </summary>
        private void OnTick()
        {
            Console.Out.WriteLine("Loop!");
        }
    }
}
