namespace RiotService
{
    /// <summary>
    /// type of service
    /// </summary>
    public enum ServiceType
    {
        System,         // system resources
        Arduino,        // Arduino connected via USB port
        PiGpio,         // GPIO on Raspberry Pi
        RiotService,    // remote RIOT service node 
    }

    /// <summary>
    /// the information defines a service node
    /// </summary>
    public class NodeInfo
    {
        /// <summary>
        /// the Name/ID for the node. 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// the comma separated routing paths for this node
        /// </summary>
        public string Routes { get; set; }

        /// <summary>
        /// The comma-delimited list of HTTP methods (verbs) supported by the route. example: "GET, PUT"
        /// </summary>
        public string Methods { get; set; }

        /// <summary>
        /// type of the service
        /// </summary>
        public ServiceType ServiceType { get; set; }

        /// <summary>
        /// the node's target address. examples: com4 for arduino, 192.168.0.23:8000 for RIOT service
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// baud rate for arduino node
        /// </summary>
        public int Baudrate { get; set; } = 9600;

        /// <summary>
        /// specify the user:password for the target RIOT service account
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// dictionary to store all service nodes
        /// </summary>
        public static Dictionary<string, NodeInfo> ServiceNodeInfoDict { get; private set; } = new Dictionary<string, NodeInfo>(StringComparer.OrdinalIgnoreCase);
    }
}
