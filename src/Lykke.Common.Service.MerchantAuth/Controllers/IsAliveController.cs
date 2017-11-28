using System;
using System.Net;
using Lykke.Common.Service.MerchantAuth.Business.Interfaces;
using Lykke.Common.Service.MerchantAuth.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Common.Service.MerchantAuth.Controllers
{
    [Route("api/[controller]")]
    public class IsAliveController : Controller
    {
        private readonly IHealthService _healthService;

        public IsAliveController(IHealthService healthService)
        {
            _healthService = healthService;
        }

        /// <summary>
        /// Checks service is alive
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IsAliveResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        public IActionResult Get()
        {
            //var healthViloationMessage = _healthService.GetHealthViolationMessage();
            //if (healthViloationMessage != null)
            //{
            //    return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse
            //    {
            //        ErrorMessage = $"Job is unhealthy: {healthViloationMessage}"
            //    });
            //}

            // NOTE: Feel free to extend IsAliveResponse, to display job-specific health status
            return Ok(new IsAliveResponse
            {
                Version = Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default.Application.ApplicationVersion,
                Env = Environment.GetEnvironmentVariable("ENV_INFO"),

                // NOTE: Health status information example: 
                LastBbHandlerStartedMoment = _healthService.LastMaServiceStartedMoment,
                LastBbHandlerDuration = _healthService.MaxHealthyMaServiceDuration,
                MaxHealthyFooDuration = _healthService.MaxHealthyMaServiceDuration
            });
        }
    }
}
