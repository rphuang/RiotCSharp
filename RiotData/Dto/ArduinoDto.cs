namespace RiotData
{
    /// <summary>
    /// defines the DTO for arduino IO
    /// </summary>
    public class ArduinoDto
    {
        /// <summary>
        /// The name of the IO - the pin number
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The property of the IO - value or mode. Default is value.
        /// </summary>
        public string Prop { get; set; }

        /// <summary>
        /// The value of the property for post/put.
        /// </summary>
        public string Value { get; set; }
    }
}
