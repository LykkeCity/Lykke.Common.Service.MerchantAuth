using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureStorage;
using AzureStorage.Tables;
using Lykke.Common.Service.MerchantAuth.Business.Models;
using Lykke.Pay.Common.Entities.Interfaces;

namespace Lykke.Common.Service.MerchantAuth.Business
{
    public class MerchantStaffRepository
    {
        private readonly INoSQLTableStorage<MerchantStaff> _tableStorage;

        public MerchantStaffRepository(INoSQLTableStorage<MerchantStaff> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<List<MerchantStaff>> GetMerchantStaffByMerchantId(string merchantId)
        {
            return (await _tableStorage.GetDataAsync(merchantId)).ToList();
        }

        public async Task<MerchantStaff> GetMerchantStaffByEmail(string email)
        {
            var results = await _tableStorage.GetDataAsync();
            return results.FirstOrDefault(
                r => r.MerchantStaffEmail.EndsWith(email, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<bool> SaveMerchantStaff(IMerchantStaff staff)
        {
            try
            {
                var mStaff = new MerchantStaff(staff);
                await _tableStorage.InsertOrReplaceAsync(mStaff);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteMerchantStaff(IMerchantStaff staff)
        {
            try
            {
                var mStaff = new MerchantStaff(staff);
                await _tableStorage.DeleteAsync(mStaff);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
