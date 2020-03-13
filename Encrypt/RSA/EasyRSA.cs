using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace uspto.Encrypt.RSA
{
    public static class EasyRSA
    {
        /// <summary>
        /// Create a new public and private RSA key.
        /// </summary>
        /// <param name="keySize">Keysize of the keys</param>
        /// <returns>The created RSA keys</returns>
        public static EasyRSAKey CreateKey(int keySize = 4096)
        {
            if (keySize <= 0) throw new ArgumentException("Could not create key: Invalid KeySize.");

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(keySize);

            byte[] privateKey = rsa.ExportCspBlob(true);
            byte[] publicKey = rsa.ExportCspBlob(false);

            return new EasyRSAKey() { PrivateKey = privateKey, PublicKey = publicKey };
        }
    }

    /// <summary>
    /// Struct with the private and public RSA keys.
    /// </summary>
    public struct EasyRSAKey
    {
        public byte[] PrivateKey;
        public byte[] PublicKey;
    }
}
