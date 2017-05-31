using Lykke.Common.Entities.Security;

namespace Lykke.Common.Service.MerchantAuth.Business.Interfaces
{
    public interface ISecurityHelper
    {
        SecurityErrorType CheckRequest(string strToSign, string merchantId, string sign, string publicKey,
            string apiKey);
    }
}
