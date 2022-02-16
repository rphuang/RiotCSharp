using System.Collections.Generic;

namespace RiotData
{
    /// <summary>
    /// The composite pattern
    /// </summary>
    public class CompositeResponse : Response
    {
        /// <summary>
        /// the sub-responses contained in this composite
        /// </summary>
        public IList<Response> SubResponses { get; set; }
    }
}
