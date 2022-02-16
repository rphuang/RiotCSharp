namespace RiotData
{
    /// <summary>
    /// The response of the IO data for Arduino and Raspberry Pi
    /// </summary>
    public class GpioResponse : Response
    {
        /// <summary>
        /// The pin of the IO
        /// </summary>
        public string Pin { get; set; }
        /// <summary>
        /// The value for the IO
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// The mode (IN/OUT) for the IO
        /// </summary>
        public string Property { get; set; }
    }

}
