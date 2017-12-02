using System;

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
        /// The world for this server.
        /// </summary>
        private World _world;

        /// <summary>
        /// Invoked when the world has been updated, after every tick.
        /// </summary>
        internal event Action<World> WorldUpdated;

        /// <summary>
        /// Starts the game loop for the server in another thread.
        /// </summary>
        private void StartGameLoopAsync()
        {
            _world = new World(Configuration.WorldSize);

            // Add all stars from configuration to the world.
            foreach (var star in Configuration.Stars)
            {
                _world.PutComponent(star);
            }

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
            WorldUpdated?.Invoke(_world);
        }

        /// <summary>
        /// Cleans dead projectiles.
        /// </summary>
        private void CleanupProjectiles()
        {
            //Checks all projectiles if they're active/alive and all dead ones are removed
            foreach (var proj in _world.GetComponents<Projectile>())
            {
                if (!proj.Active)
                {
                    _world.RemoveComponent(proj);
                }
            }
        }

        /// <summary>
        /// Spawns any new or respawning ships in an empty location
        /// </summary>
        private void SpawnShips()
        {
            // Check every ship.
            foreach (var ship in _world.GetComponents<Ship>())
            {
                // Ignore alive ships
                if (ship.Health > 0)
                    continue;

                // Check if the ship is waiting to respawn.
                if (ship.RespawnFrames > 0)
                {
                    // Decrease the frames counter by 1.
                    ship.RespawnFrames--;

                    continue;
                }

                // Check if the ship has a connected client
                if (!_clients.TryGetValue(ship.Id, out var _))
                    continue;

                // Respawn the ship, since it is dead but its frame counter is 0.

                // Compute a spawn location for the ship.
                var spawnLocation = _world.FindShipSpawnLocation(Configuration.StarCollisionRadius, Configuration.ShipCollisionRadius);
                ship.Location = spawnLocation;

                // Compute a random direction for the ship.
                var random = new Random();
                var spawnDirection = new Vector2D((random.NextDouble() * 1 - 0.5) * 2, (random.NextDouble() * 1 - 0.5) * 2);
                spawnDirection.Normalize();
                ship.Direction = spawnDirection;

                // Restore the ship's health.
                ship.Health = Configuration.ShipHitpoints;
            }
        }

        /// <summary>
        /// Spawns any new projectiles due to firing commands.
        /// </summary>
        private void SpawnProjectiles()
        {
            // Check every ship.
            foreach (var ship in _world.GetComponents<Ship>())
            {
                // Don't spawn projectiles for dead ships.
                if (ship.Health == 0)
                    continue;

                // Decrease cooldown counter if needed.
                if (ship.ProjectileCooldown > 0)
                {
                    ship.ProjectileCooldown--;
                    continue;
                }

                // Check if firing
                var clientCommunicator = _clients[ship.Id];
                if(clientCommunicator.ClientCommands[Ship.Command.Fire])
                {
                    ship.ProjectileCooldown = Configuration.FramesPerShot;
                    var projectile = new Projectile(ship.Id)
                    {
                        Direction = ship.Direction,
                        Location = ship.Location,
                        Velocity = ship.Direction * Configuration.ProjectileSpeed
                    };

                    _world.PutComponent(projectile);
                }
            }
        }

        /// <summary>
        /// Computes how ships should move, based on gravity, thrust commands, and turning commands.
        /// </summary>
        private void ComputeShipMotion()
        {
            // Compute for each ship
            foreach (var ship in _world.GetComponents<Ship>())
            {
                // Don't compute dead ships.
                if (ship.Health == 0)
                    continue;

                // Turn if needed.
                var clientCommunicator = _clients[ship.Id];
                if (clientCommunicator.ClientCommands[Ship.Command.Left])
                    ship.Direction.Rotate(-Configuration.ShipTurningRate);
                else if (clientCommunicator.ClientCommands[Ship.Command.Right])
                    ship.Direction.Rotate(Configuration.ShipTurningRate);

                //TODO: Compute acceleration 
                //TODO: Add acceleration to ship velocity
                
                // Apply ship's velocity to location
                ship.Location += ship.Velocity;
            }
        }

        /// <summary>
        /// Computes how projectiles should move, based only on a constant velocity.
        /// If a projectile goes out of bounds, mark it as "dead."
        /// </summary>
        private void ComputeProjectileMotion()
        {
            double bounds = _world.Size / 2d;
            //Computing motion and bound checking for each Projectile
            foreach (var proj in _world.GetComponents<Projectile>())
            {
                //new location based on projectile's velocity
                proj.Location += proj.Velocity;

                //If a projectile is out of the world's bounds it's marked as not Active
                double x = proj.Location.GetX();
                double y = proj.Location.GetY();
                if (x > bounds || x < -bounds || y > bounds || y < -bounds)
                {
                    proj.Active = false;
                }
            }
        }

        /// <summary>
        /// Wraps any out-of-bounds ships to the other side of the world.
        /// </summary>
        private void ComputeWrapping()
        {
            // Check each ship
            foreach (var ship in _world.GetComponents<Ship>())
            {
                // Don't compute dead ships.
                if (ship.Health == 0)
                    continue;

                // How far on either axis (in either direction) that the ship may travel.
                var bounds = _world.Size / 2d;

                // Ship location
                var x = ship.Location.GetX();
                var y = ship.Location.GetY();

                // If a ship is out of bounds on the X-axis, set its X to the edge of the other side.
                if(x > bounds)
                    ship.Location = new Vector2D(-bounds, y);
                else if(x < -bounds)
                    ship.Location = new Vector2D(bounds, y);

                // If a ship is out of bounds on the Y-axis, set its Y to the edge of the other side.
                if (y > bounds)
                    ship.Location = new Vector2D(x, -bounds);
                else if (y < -bounds)
                    ship.Location = new Vector2D(x, bounds);
            }
        }

        /// <summary>
        /// Determines where and when projectiles collide with ships and stars.
        /// Decreases the health of ships when a collision occurs, marks collided projectiles as "dead."
        /// If a ship dies as result of the collision, a point is added to the ship who owned the projectile.
        /// </summary>
        private void ComputeCollision()
        {
            //Ship and projectile collisions
            foreach (var ship in _world.GetComponents<Ship>())
            {
                // Don't compute dead ships.
                if (ship.Health == 0)
                    continue;

                foreach (var proj in _world.GetComponents<Projectile>()) 
                {
                    if (ship == _world.GetComponent<Ship>(proj.OwnerShipId))
                    {
                        continue;
                    }

                    //If the distance between a ship and projectile is less than the ship's radius a collision occurs
                    Vector2D distanceVector = ship.Location - proj.Location;
                    if (distanceVector.Length() < Configuration.ShipCollisionRadius)
                    {
                        proj.Active = false;
                        ship.Health--;

                        //Dead ships are set to respawn in a certain amount of frames and a point is given to the ship that killed it
                        if (ship.Health == 0)
                        {
                            ship.RespawnFrames = Configuration.RespawnRate;

                            Ship winner = _world.GetComponent<Ship>(proj.OwnerShipId);
                            winner.Score++;
                        }
                    }
                }
            }

            //Star and projectile collisions, the star is unaffected, the projectile dies
            foreach (var star in _world.GetComponents<Star>())
            {
                foreach (var proj in _world.GetComponents<Projectile>())
                {
                    //If the distance between a star and projectile is less than the star's radius a collision occurs
                    Vector2D distanceVector = star.Location - proj.Location;
                    if (distanceVector.Length() < Configuration.StarCollisionRadius)
                    {
                        proj.Active = false;
                    }
                }
            }
        }
    }
}
