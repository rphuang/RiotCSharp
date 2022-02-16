namespace RiotData
{
    /// <summary>
    /// system memory data in MB
    /// </summary>
    public class MemoryData : Response
    {
        /// <summary>
        /// total memory
        /// </summary>
        public float Total { get; set; }

        /// <summary>
        /// Available memory
        /// </summary>
        public float Available { get; set; }

        /// <summary>
        /// free memory
        /// </summary>
        public float Free { get; set; }

        /// <summary>
        /// used memory
        /// </summary>
        public float Used { get; set; }

        /// <summary>
        /// system used memory
        /// </summary>
        public float SystemUsed { get; set; }

        /// <summary>
        /// Cached memory
        /// </summary>
        public float Cached { get; set; }

        /// <summary>
        /// percentage used
        /// </summary>
        public float UsedPercent { get; set; }
    }
}
