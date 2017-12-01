using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpaceWars
{
    /// <summary>
    /// This class represents a general purpose game loop that maintains a constant FPS.
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    public class GameLoop
    {
        /// <summary>
        /// This delegate is invoked on each tick of the game loop.
        /// </summary>
        public delegate void Tick();

        /// <summary>
        /// The tick callback.
        /// </summary>
        private readonly Tick _tick;

        /// <summary>
        /// This is true if the game loop is currently running.
        /// Set this to false to stop the loop.
        /// </summary>
        public bool Running { get; set; }

        /// <summary>
        /// The number of milliseconds per frame. 
        /// </summary>
        private readonly int _msPerFrame;

        /// <summary>
        /// Creates and starts a new game loop with a constant FPS.
        /// The loop runs on a separate thread.
        /// </summary>
        /// <param name="msPerFrame">The number of milliseconds to spend per frame.</param>
        /// <param name="tick">The callback invoked on each tick of the loop (each frame).</param>
        public GameLoop(int msPerFrame, Tick tick)
        {
            _msPerFrame = msPerFrame;
            _tick = tick;

            // Start the loop on a separate thread.
            new Thread(Loop).Start();
        }

        /// <summary>
        /// Begins and maintains the loop.
        /// </summary>
        private void Loop()
        {
            Running = true;

            // This will hold when the last tick finished.
            var lastTime = Environment.TickCount;

            while (Running)
            {
                // Notify callback (do work)
                _tick.Invoke();

                // Determine how much time is left during this frame, which we will "sleep away."
                var currentTime = Environment.TickCount;
                var finishTime = lastTime + _msPerFrame;
                var sleepTime = finishTime - currentTime;

                // Sleep if needed.
                if (sleepTime >= 0)
                {
                    Thread.Sleep(sleepTime);
                }

                // Record when this tick finished.
                lastTime = Environment.TickCount;
            }
        }

    }
}
