using System;
using Lykke.Common.Service.MerchantAuth.Business.Interfaces;

namespace Lykke.Common.Service.MerchantAuth.Business
{
    public class HealthService : IHealthService
    {
        // NOTE: These are example properties
        public DateTime LastMaServiceStartedMoment { get; private set; }
        public TimeSpan LastMaServiceDuration { get; private set; }
        public TimeSpan MaxHealthyMaServiceDuration { get; }

        // NOTE: These are example properties
        private bool WasLastMaServiceFailed { get; set; }
        private bool WasLastMaServiceCompleted { get; set; }
        private bool WasClientsMaServiceEverStarted { get; set; }

        // NOTE: When you change parameters, don't forget to look in to JobModule

        public HealthService(TimeSpan maxHealthyMaServiceDuration)
        {
            MaxHealthyMaServiceDuration = maxHealthyMaServiceDuration;
           
        }

        // NOTE: This method probably would stay in the real job, but will be modified
        public string GetHealthViolationMessage()
        {
            if (WasLastMaServiceFailed)
            {
                return "Last MaService was failed";
            }

            if (!WasLastMaServiceCompleted && !WasLastMaServiceFailed && !WasClientsMaServiceEverStarted)
            {
                return "Waiting for first MaService execution started";
            }

            if (!WasLastMaServiceCompleted && !WasLastMaServiceFailed && WasClientsMaServiceEverStarted)
            {
                return $"Waiting {DateTime.UtcNow - LastMaServiceStartedMoment} for first MaService execution completed";
            }

            if (LastMaServiceDuration > MaxHealthyMaServiceDuration)
            {
                return $"Last MaService was lasted for {LastMaServiceDuration}, which is too long";
            }
            return null;
        }

        // NOTE: These are example methods
        public void TraceMaServiceStarted()
        {
            LastMaServiceStartedMoment = DateTime.UtcNow;
            WasClientsMaServiceEverStarted = true;
        }

        public void TraceMaServiceCompleted()
        {
            LastMaServiceDuration = DateTime.UtcNow - LastMaServiceStartedMoment;
            WasLastMaServiceCompleted = true;
            WasLastMaServiceFailed = false;
        }

        public void TraceMaServiceFailed()
        {
            WasLastMaServiceCompleted = false;
            WasLastMaServiceFailed = true;
        }

        public void TraceBooStarted()
        {
            // TODO: See PrService
        }

        public void TraceBooCompleted()
        {
            // TODO: See PrService
        }

        public void TraceBooFailed()
        {
            // TODO: See PrService
        }
    }
}