using System.Collections.Generic;

namespace AridityTeam.Pak
{
    /// <summary>
    /// Provides an entry class that stores information about this chunk.
    /// </summary>
    public sealed class PakChunk
    {
        /// <summary>
        /// The "supposed" path to the file.
        /// </summary>
        public string FilePath { get; set; } = string.Empty;

        /// <summary>
        /// Where the data starts.
        /// </summary>
        public long Offset { get; set; } = 0L;

        /// <summary>
        /// The estimate size of this chunk.
        /// </summary>
        public long OriginalSize { get; set; } = 0L;

        /// <summary>
        /// The actual data of the chunk.
        /// </summary>
        public byte[] Data { get; set; } = [];

        /// <summary>
        /// A collection of each blocks in the chunk.
        /// </summary>
        public List<PakBlock> Blocks = [];
    }
}
