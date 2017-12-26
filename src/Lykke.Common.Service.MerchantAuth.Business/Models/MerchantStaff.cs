using AzureStorage.Tables;
using Lykke.Pay.Common.Entities.Interfaces;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Common.Service.MerchantAuth.Business.Models
{
    public class MerchantStaff : TableEntity, IMerchantStaff
    {

        public MerchantStaff()
        {
            
        }

        public MerchantStaff(IMerchantStaff staff)
        {
            PartitionKey = staff.MerchantId;
            RowKey = staff.MerchantStaffEmail;
            MerchantStaffFirstName = staff.MerchantStaffFirstName;
            MerchantStaffLastName = staff.MerchantStaffLastName;
            MerchantStaffPassword = staff.MerchantStaffPassword;
            LwId = staff.LwId;
            ETag = "*";
        }


        public string MerchantId {
            get => PartitionKey;
            set => PartitionKey = value;
        }

        public string MerchantStaffEmail
        {
            get => RowKey;
            set => RowKey = value;
        }

        public string MerchantStaffFirstName { get; set; }
        public string MerchantStaffLastName { get; set; }
        public string MerchantStaffPassword { get; set; }
        public string LwId { get; set; }
    }
}
