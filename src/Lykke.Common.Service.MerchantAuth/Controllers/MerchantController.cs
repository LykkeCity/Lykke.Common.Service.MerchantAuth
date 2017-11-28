using System.Threading.Tasks;
using Lykke.Common.Service.MerchantAuth.Business;
using Lykke.Common.Service.MerchantAuth.Models;
using Lykke.Core;
using Lykke.Pay.Common.Entities.Entities;
using Lykke.Pay.Common.Entities.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Common.Service.MerchantAuth.Controllers
{
    [Route("api/[controller]")]
    public class MerchantController : Controller
    {
        private readonly IMerchantRepository _merchantRepository;
        private readonly MerchantStaffRepository _merchantStaffRepository;

        public MerchantController(IMerchantRepository merchantRepository, MerchantStaffRepository merchantStaffRepository)
        {
            _merchantRepository = merchantRepository;
            _merchantStaffRepository = merchantStaffRepository;
        }

        [HttpGet("clientId/{merchantId}")]
        public async Task<IActionResult> Get(string merchantId)
        {
            var merchant = await _merchantRepository.GetAsync(merchantId);
            return Json(merchant);
        }


        [HttpPost("staffSignIn")]
        public async Task<IMerchantStaff> StaffSignInPost([FromBody]MerchantStaffSignInRequest request)
        {
            if (string.IsNullOrEmpty(request?.Login) || request.Password == null)
            {
                return null;
            }

            var staff = await _merchantStaffRepository.GetMerchantStaffByEmail(request.Login);
            if (!request.Password.Equals(staff?.MerchantStaffPassword))
            {
                return null;
            }

            return staff;
            
        }

        [HttpPost("staffSignOn")]
        public async Task<IMerchantStaff> StaffSignOnPost([FromBody]MerchantStaff request)
        {
            if (await _merchantStaffRepository.SaveMerchantStaff(request))
            {
                return request;
            }
            return null;
        }
    }
}
