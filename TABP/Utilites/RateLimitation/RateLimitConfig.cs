using System.Threading.RateLimiting;

namespace TABP.Utilites.RateLimitation
{
    public class RateLimitConfig
    {
        public required int MaxRequests { get; set; }
        public required double WindowDurationInSeconds { get; set; }
        public required QueueProcessingOrder RequestQueueOrder { get; set; }
        public required int MaxQueueSize { get; set; }
    }
}
