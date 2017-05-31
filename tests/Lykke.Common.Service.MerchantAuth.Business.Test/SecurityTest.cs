using System;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Lykke.Common.Entities.Security;
using Lykke.Common.Service.MerchantAuth.Business.Interfaces;
using Lykke.Common.Service.MerchantAuth.Business.Test.Integrations;
using Xunit;

namespace Lykke.Common.Service.MerchantAuth.Business.Test
{
    public class SecurityTest 
    {
        private readonly WebSecurityHelper _securityHelper;
        public SecurityTest()
        {
            //_securityHelper = new SecurityHelper();
            _securityHelper = new WebSecurityHelper();
        }

        [Fact]
        public void CheckIncorrectMerchant()
        {
            var result = _securityHelper.CheckRequest(new MerchantAuthRequest()
            {
                MerchantId = "100",
                StringToSign = "TestStringToSign",
                Sign = String.Empty
            });

            Assert.Equal(SecurityErrorType.MerchantUnknown, result);
        }

        [Fact]
        public void CheckSignEmpty()
        {
            var result = _securityHelper.CheckRequest(new MerchantAuthRequest
            {
                MerchantId = "1",
                StringToSign = "TestStringToSign",
                Sign = String.Empty
            });

            Assert.Equal(SecurityErrorType.SignEmpty, result);
        }

        [Fact]
        public void CheckSignIncorrect()
        {
            var result = _securityHelper.CheckRequest(new MerchantAuthRequest
            {
                MerchantId = "1",
                StringToSign = "TestStringToSign",
                Sign = "test"
            });

            Assert.Equal(SecurityErrorType.SignIncorrect, result);
        }

       

        [Fact]
        public void CheckSignCorrect()
        {
            var keyApi = "LYKKEAPIKEYEXAMPLE1";
            string strToSign = $"{keyApi}TestStringToSign";
            X509Certificate2 certificate;
            using (var store = new X509Store(StoreName.My, StoreLocation.LocalMachine))
            {
                store.Open(OpenFlags.ReadOnly);
                certificate = store.Certificates.Cast<X509Certificate2>().First(c => c.SubjectName.Name.IndexOf("CN=Merchant1", StringComparison.Ordinal) >= 0);
            }

            var csp = certificate.GetRSAPrivateKey();
            var sign = Convert.ToBase64String(csp.SignData(Encoding.UTF8.GetBytes(strToSign), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1));

            var result = _securityHelper.CheckRequest(new MerchantAuthRequest
            {
                MerchantId = "1",
                StringToSign = "TestStringToSign",
                Sign = sign
            });

            Assert.Equal(SecurityErrorType.Ok, result);
        }

      
    }
}
