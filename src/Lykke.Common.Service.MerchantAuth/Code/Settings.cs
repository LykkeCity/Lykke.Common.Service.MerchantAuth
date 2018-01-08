using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lykke.Common.Service.MerchantAuth.Code
{
    public class Settings
    {
        public ServiceMerchantAuthSettings ServiceMerchantAuth { get; set; }
        public BalancesServiceClientSettings BalancesServiceClient { get; set; }
    }

    public class ServiceMerchantAuthSettings
    {
        public DbSettings Db { get; set; }
    }

    public class DbSettings
    {
        public string AuthConnString { get; set; }

    }

    public class BalancesServiceClientSettings
    {
        public string ServiceUrl { get; set; }
    }
}
