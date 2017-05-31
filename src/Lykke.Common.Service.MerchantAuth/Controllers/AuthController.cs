using System.Threading.Tasks;
using Lykke.Common.Entities.Security;
using Lykke.Common.Service.MerchantAuth.Business.Interfaces;
using Lykke.Core;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Common.Service.MerchantAuth.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IMerchantRepository _merchantRepository;
        private readonly ISecurityHelper _securityHelper;

        public AuthController(IMerchantRepository merchantRepository, ISecurityHelper securityHelper)
        {
            _merchantRepository = merchantRepository;
            _securityHelper = securityHelper;
        }

        // POST api/auth
        [HttpPost]
        public async Task<SecurityErrorType> Post([FromBody]MerchantAuthRequest request)
        {
            if (request == null)
            {
                return SecurityErrorType.MerchantUnknown;
            }

            var merchant = await _merchantRepository.GetAsync(request.MerchantId);

            if (merchant == null)
            {
                return SecurityErrorType.MerchantUnknown;
            }

            return _securityHelper.CheckRequest(request.StringToSign, merchant.MerchantId, request.Sign,
                merchant.PublicKey, merchant.ApiKey);

        }
        
    }
}
