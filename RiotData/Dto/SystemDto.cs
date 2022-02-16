namespace RiotData
{
    /// <summary>
    /// defines the DTO for request to get system: Info, CPU, Memory, Storage
    /// </summary>
    public class SystemDto
    {
        /// <summary>
        /// system category: one of Info, CPU, Memory, Storage
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// details - all paths exclude category
        /// </summary>
        public string Details { get; set; }
    }
}
