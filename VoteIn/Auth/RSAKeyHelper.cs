using System.Security.Cryptography;

namespace VoteIn.Auth
{
    public class RSAKeyHelper
    {
        /// <summary>
        /// Generates the key.
        /// </summary>
        /// <returns></returns>
        public static RSAParameters GenerateKey()
        {
            using (var key = new RSACryptoServiceProvider(2048))
            {
                return key.ExportParameters(true);
            }
        }
    }
}
