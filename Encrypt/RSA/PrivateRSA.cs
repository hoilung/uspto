using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace uspto.Encrypt.RSA
{
    public class PrivateRSA 
    {
        /// <summary>
        /// Create class with an already set up provider.
        /// </summary>
        /// <param name="provider">The RSA provider</param>
        public PrivateRSA(RSACryptoServiceProvider provider)
            => this.provider = provider ?? throw new ArgumentNullException("Invalid provider, provider is null.");
        /// <summary>
        /// Create class with a key and a custom Provider.
        /// </summary>
        /// <param name="provider">The RSA provider</param>
        /// <param name="key">Private RSA key</param>
        public PrivateRSA(RSACryptoServiceProvider provider, byte[] key)
        {
            this.provider = provider ?? throw new ArgumentNullException("Invalid provider, provider is null.");

            if (key == null) throw new ArgumentException("Invalid key, key is null.");
            this.provider.ImportCspBlob(key);
        }
        /// <summary>
        /// Create class with a key.
        /// </summary>
        /// <param name="key">Private RSA key</param>
        public PrivateRSA(byte[] key)
        {
            this.provider = new RSACryptoServiceProvider();
            this.provider.ImportCspBlob(key);
        }

        /// <summary>
        /// Provider for decrypting or signing data.
        /// </summary>
        private readonly RSACryptoServiceProvider provider;

        /// <summary>
        /// Decrypt a string and encode with UTF8.
        /// </summary>
        /// <param name="text">Text to decrypt</param>
        /// <returns>Decrypted text(string encoded with UTF8)</returns>
        public string Decrypt(string text)
            => Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(text)));
        /// <summary>
        /// Decrypt a string.
        /// </summary>
        /// <param name="text">Text to decrypt</param>
        /// <param name="encoder">Encoding used to convert byte[] to string</param>
        /// <returns>Decrypted text(string encoded with Encoder)</returns>
        public string Decrypt(string text, Encoding encoder)
            => encoder.GetString(Decrypt(Convert.FromBase64String(text)));
        /// <summary>
        /// Decrypt a byte[].
        /// </summary>
        /// <param name="data">Data to decrypt</param>
        /// <returns>Decrypted data</returns>
        public byte[] Decrypt(byte[] data)
            => provider.Decrypt(data, false);

        /// <summary>
        /// Sign data and use SHA256 as hashingalgorithm.
        /// Hash is needed for long data.
        /// </summary>
        /// <param name="data">Data to sing</param>
        /// <returns>Signed data</returns>
        public byte[] Sign(byte[] data)
            => Sign(data, SHA256.Create());
        /// <summary>
        /// Sign data and use a custom hashingalgorithm.
        /// </summary>
        /// <param name="data">Data to sign</param>
        /// <param name="algorithm">Hashing algorithm used to create hash</param>
        /// <returns>Signed data</returns>
        public byte[] Sign(byte[] data, HashAlgorithm algorithm)
            => provider.SignData(data, algorithm);

        /// <summary>
        /// Return the current private key.
        /// </summary>
        /// <returns>Private RSA key</returns>
        public byte[] GetKey()
            => provider.ExportCspBlob(true);
    }
}
