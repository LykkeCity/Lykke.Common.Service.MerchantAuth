using System;

namespace Lykke.Common.Service.MerchantAuth.Models
{
    public class IsAliveResponse
    {
        public string Version { get; set; }
        public string Env { get; set; }

        // NOTE: Health status information example: 
        public DateTime LastBbHandlerStartedMoment { get; set; }
        public TimeSpan LastBbHandlerDuration { get; set; }
        public TimeSpan MaxHealthyFooDuration { get; set; }
    }
}