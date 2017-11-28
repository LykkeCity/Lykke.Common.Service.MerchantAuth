using System;

namespace Lykke.Common.Service.MerchantAuth.Business.Interfaces
{
    public interface IHealthService
    {
        // NOTE: These are example properties
        DateTime LastMaServiceStartedMoment { get; }
        TimeSpan LastMaServiceDuration { get; }
        TimeSpan MaxHealthyMaServiceDuration { get; }

        // NOTE: This method probably would stay in the real job, but will be modified
        string GetHealthViolationMessage();

        // NOTE: These are example methods
        void TraceMaServiceStarted();
        void TraceMaServiceCompleted();
        void TraceMaServiceFailed();
       
    }
}