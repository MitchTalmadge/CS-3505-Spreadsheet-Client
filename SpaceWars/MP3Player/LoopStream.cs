using NAudio.Wave;

namespace SpaceWars
{
    /// <inheritdoc />
    /// <summary>
    /// A custom WaveStream that loops the audio once it has been fully read.
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    internal class LoopStream : WaveStream
    {
        /// <summary>
        /// The stream from which to read the audio.
        /// </summary>
        private readonly WaveStream _inputStream;

        /// <summary>
        /// Creates a looping audio stream that reads from the given input stream.
        /// </summary>
        /// <param name="inputStream">The audio stream to read from.</param>
        internal LoopStream(WaveStream inputStream)
        {
            _inputStream = inputStream;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var totalBytesRead = 0;

            while (totalBytesRead < count)
            {
                // Try to read bytes from the audio.
                var bytesRead = _inputStream.Read(buffer, offset + totalBytesRead, count - totalBytesRead);
                if (bytesRead == 0) // No bytes were read, so we are at the end.
                {
                    // Reset the position of the input stream, causing this to loop.
                    _inputStream.Position = 0;
                }

                totalBytesRead += bytesRead;
            }

            return totalBytesRead;
        }


        public override WaveFormat WaveFormat => _inputStream.WaveFormat;
        public override long Length => _inputStream.Length;

        public override long Position
        {
            get => _inputStream.Position;
            set => _inputStream.Position = value;
        }
    }
}