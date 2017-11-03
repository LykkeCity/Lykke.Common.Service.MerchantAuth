using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lykke.AzureRepositories;
using Lykke.Common.Service.MerchantAuth.Business;
using Lykke.Common.Service.MerchantAuth.Business.Interfaces;
using Lykke.Common.Service.MerchantAuth.Code;
using Lykke.Core.Log;
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
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
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
            var connectionString = Configuration.GetValue<string>("ConnectionString");
#if DEBUG
            var generalSettings = SettingsReader.SettingsReader.ReadGeneralSettings<Settings>(new Uri(connectionString));
#else
            var generalSettings = SettingsReader.SettingsReader.ReadGeneralSettings<Settings>(new Uri(connectionString));
#endif
            var settings = generalSettings.ServiceMerchantAuth;

            services.AddSingleton(settings);
            services.AddSingleton<ISecurityHelper>(new SecurityHelper());
            services.RegisterRepositories(settings.Db.AuthConnString, (ILog)null);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
          
            app.UseMvc();
        }
    }
}
