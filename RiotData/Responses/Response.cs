namespace RiotData
{
    /// <summary>
    /// the base class for responses and for simple responses with no other content
    /// </summary>
    public class Response
    {
        /// <summary>
        /// The status code for the response. optional if the code is 200
        /// </summary>
        public int? StatusCode { get; set; }

        /// <summary>
        /// The error message when status code is not 200
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
