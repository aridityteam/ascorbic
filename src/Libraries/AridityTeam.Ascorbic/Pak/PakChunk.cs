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
        /// The estimate compressed size of the chunk.
        /// </summary>
        public long Length { get; set; } = 0L;

        /// <summary>
        /// The actual data of the chunk.
        /// </summary>
        public byte[] Data { get; set; } = [];
    }
}
