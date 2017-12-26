using System.Linq;
using System.Threading.Tasks;
using Lykke.Common.Service.MerchantAuth.Business;
using Lykke.Common.Service.MerchantAuth.Models;
using Lykke.Core;
using Lykke.Pay.Common.Entities.Entities;
using Lykke.Pay.Common.Entities.Interfaces;
using Lykke.Service.Balances.Client;
using Microsoft.AspNetCore.Mvc;
using MerchantStaff = Lykke.Common.Service.MerchantAuth.Business.Models.MerchantStaff;

namespace Lykke.Common.Service.MerchantAuth.Controllers
{
    [Route("api/[controller]")]
    public class MerchantController : Controller
    {
        private readonly IMerchantRepository _merchantRepository;
        private readonly MerchantStaffRepository _merchantStaffRepository;
        private readonly IBalancesClient _balanceClient;

        public MerchantController(IMerchantRepository merchantRepository, MerchantStaffRepository merchantStaffRepository, IBalancesClient balanceClient)
        {
            _merchantRepository = merchantRepository;
            _merchantStaffRepository = merchantStaffRepository;
            _balanceClient = balanceClient;
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


        [HttpGet("balance/{staffId}")]
        public async Task<IMerchantBalance> GetMerchantBalance(string staffId)
        {
            if (string.IsNullOrEmpty(staffId))
            {
                return null;
            }

            var staff = await _merchantStaffRepository.GetMerchantStaffByEmail(staffId);
            if (staff == null)
            {
                return null;
            }

            var merchant = await _merchantRepository.GetAsync(staff.MerchantId);

            return await GetBalanceById(string.IsNullOrEmpty(merchant?.LwId) ? staff.LwId : merchant.LwId);


        }

        private async Task<IMerchantBalance> GetBalanceById(string lwId)
        {
            if (string.IsNullOrEmpty(lwId))
            {
                return null;
            }

            var balance = await _balanceClient.GetClientBalances(lwId);
            var result = new MerchantBalance
            {
                Asserts = (from b in balance
                    select new MerchantAssertBalance
                    {
                        Assert = b.AssetId,
                        Value = b.Balance
                    }).ToList()
            };
            return result;
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
