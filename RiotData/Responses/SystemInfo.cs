namespace RiotData
{
    /// <summary>
    /// response data for system info
    /// </summary>
    public class SystemInfo : Response
    {
        /// <summary>
        /// the machine name
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// the OS version
        /// </summary>
        public string OSVersion { get; set; }

        /// <summary>
        /// the storage drives
        /// </summary>
        public string LogicalDrives { get; set; }

        /// <summary>
        /// the Processor Architecture
        /// </summary>
        public string ProcessorArchitecture { get; set; }

        /// <summary>
        /// the Processor Model
        /// </summary>
        public string ProcessorModel { get; set; }

        /// <summary>
        /// the Processor Level
        /// </summary>
        public string ProcessorLevel { get; set; }

        /// <summary>
        /// the Processor Revision
        /// </summary>
        public string ProcessorRevision { get; set; }

        /// <summary>
        /// the number of processors
        /// </summary>
        public int ProcessorCount { get; set; }

        /// <summary>
        /// the .Net Version
        /// </summary>
        public string DotNetVersion { get; set; }
    }
}
