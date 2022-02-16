namespace RiotData
{
    /// <summary>
    /// defines the DTO for bridging between RIOT services
    /// </summary>
    public class RiotBridgeDto
    {
        /// <summary>
        /// the request path to be forwarded to the target RIOT service
        /// </summary>
        public string RiotRequest { get; set; }
    }
}
