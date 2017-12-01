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
            // Cleaning
            CleanupProjectiles();

            // Spawning
            SpawnShips();
            SpawnProjectiles();

            // Motion
            ComputeShipMotion();
            ComputeProjectileMotion();
            ComputeWrapping();

            // Collision
            ComputeCollision();

            // Notify listeners of world updated
            // TODO
        }

        /// <summary>
        /// Cleans dead projectiles.
        /// </summary>
        private void CleanupProjectiles()
        {

        }

        /// <summary>
        /// Spawns any new or respawning ships in an empty location
        /// </summary>
        private void SpawnShips()
        {

        }

        /// <summary>
        /// Spawns any new projectils due to firing commands.
        /// </summary>
        private void SpawnProjectiles()
        {
            
        }

        /// <summary>
        /// Computes how ships should move, based on gravity, thrust commands, and turning commands.
        /// </summary>
        private void ComputeShipMotion()
        {
            
        }

        /// <summary>
        /// Computes how projectiles should move, based only on a constant velocity.
        /// If a projectile goes out of bounds, mark it as "dead."
        /// </summary>
        private void ComputeProjectileMotion()
        {

        }

        /// <summary>
        /// Wraps any out-of-bounds ships to the other side of the world.
        /// </summary>
        private void ComputeWrapping()
        {
            
        }

        /// <summary>
        /// Determines where and when projectiles collide with ships and stars.
        /// Decreases the health of ships where needed.
        /// Marks collided projectiles as "dead."
        /// If a ship dies as result of the collision, a point is added to the ship who owned the projectile.
        /// </summary>
        private void ComputeCollision()
        {
            
        }

    }
}
