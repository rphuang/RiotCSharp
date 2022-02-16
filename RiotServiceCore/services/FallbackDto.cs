using ServiceStack;

namespace RiotService
{
    [FallbackRoute("/{PathInfo*}")]
    internal class FallbackDto
    {
        public string PathInfo { get; set; }
    }
}
