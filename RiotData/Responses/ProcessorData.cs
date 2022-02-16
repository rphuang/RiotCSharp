namespace RiotData
{
    /// <summary>
    /// defines processor perf data
    /// </summary>
    public class ProcessorData
    {
        /// <summary>
        /// % idle
        /// </summary>
        public float Idle { get; set; }

        /// <summary>
        /// % SystemUsage
        /// </summary>
        public float SystemUsage { get; set; }

        /// <summary>
        /// % total usage
        /// </summary>
        public float Usage { get; set; }

        /// <summary>
        /// % UserUsage
        /// </summary>
        public float UserUsage { get; set; }

        /// <summary>
        /// the cpu cores contained; only valid for the root level cpu node
        /// </summary>
        public IDictionary<string, ProcessorData> Cores { get; set; }
    }
}
