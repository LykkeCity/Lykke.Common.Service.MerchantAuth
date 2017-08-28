using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Common.Entities.Security;
using Lykke.Common.Service.MerchantAuth.Business.Interfaces;
using Lykke.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Lykke.Common.Service.MerchantAuth.Controllers
{
    [Route("api/[controller]")]
    public class MerchantController : Controller
    {
        private readonly IMerchantRepository _merchantRepository;

        public MerchantController(IMerchantRepository merchantRepository)
        {
            _merchantRepository = merchantRepository;
        }

        [HttpGet("clientId/{merchantId}")]
        public async Task<IActionResult> Get(string merchantId)
        {
            var merchant = await _merchantRepository.GetAsync(merchantId);
            return Content(merchant.LykkeWalletKey);
        }

    }
}
