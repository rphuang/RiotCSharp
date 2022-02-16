
namespace RiotData
{
    //[Route("/gpio")]
    //[Route("/gpio/{Name}")]
    //[Route("/gpio/{Name}/{Prop*}")]
    /// <summary>
    /// defines the DTO for requesting IO data for Arduino and Raspberry Pi 
    /// </summary>
    public class GpioDto
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
        /// The value of the property for post/put (from json body).
        /// </summary>
        public string Value { get; set; }
    }
}
