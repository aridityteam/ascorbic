namespace AridityTeam.Pak
{
    /// <summary>
    /// Provides an entry class that stores information about this block.
    /// </summary>
    public sealed class PakBlock
    {
        /// <summary>
        /// Where the data starts.
        /// </summary>
        public long Offset { get; set; } = 0L;

        /// <summary>
        /// The compressed size of the block.
        /// </summary>
        public int Length { get; set; } = 0;

        /// <summary>
        /// The uncompressed size of the block.
        /// </summary>
        public int Size { get; set; } = 0;
    }
}
