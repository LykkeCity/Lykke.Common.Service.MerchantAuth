using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage.Tables;
using Lykke.AzureRepositories;
using Lykke.Common.Service.MerchantAuth.Business;
using Lykke.Common.Service.MerchantAuth.Business.Interfaces;
using Lykke.Common.Service.MerchantAuth.Business.Models;
using Lykke.Common.Service.MerchantAuth.Code;
using Lykke.Core.Log;
using Lykke.Service.Balances.Client;
using Lykke.SettingsReader;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Lykke.Common.Service.MerchantAuth
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            BuildConfiguration(services);
            // Add framework services.
            services.AddMvc();
        }

        private void BuildConfiguration(IServiceCollection services)
        {
            //var connectionString = Configuration.GetValue<string>("ConnectionString");
            var generalSettings = Configuration.LoadSettings<Settings>();
            

            var settings = generalSettings.Nested(n=>n.ServiceMerchantAuth);

            services.AddSingleton(settings.CurrentValue);
            services.AddSingleton<ISecurityHelper>(new SecurityHelper());
            services.AddSingleton<IHealthService>(new HealthService(TimeSpan.FromSeconds(30)));
            services.RegisterRepositories(settings.CurrentValue.Db.AuthConnString, (ILog)null);


            services.AddSingleton(new MerchantStaffRepository
                    (AzureTableStorage<MerchantStaff>.Create(settings.Nested(s=>s.Db.AuthConnString), "MerchantsStaff", null)));

            services.AddSingleton<IBalancesClient>(new BalancesClient(generalSettings.Nested(s => s.BalancesServiceClient.ServiceUrl).CurrentValue, null));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
          
            app.UseMvc();
        }
    }
}
