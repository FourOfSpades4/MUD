using MUD;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace MUD
{
    public class RSAEncryptionPublic : RSAEncryption {
        public static RSAEncryptionPublic Instance { get; private set; }

        public void Initialize() {
            Instance = this;
        }

        public void SetPublicKey(string key)
        {
            XmlSerializer xs = new XmlSerializer(typeof(RSAParameters));
            using (var reader = new StringReader(key))
            {
                _publickey = (RSAParameters) xs.Deserialize(reader);
            }
        }
        
        public string Encrypt(string plaintext)
        {
            RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
            csp.ImportParameters(_publickey);
            byte[] data = Encoding.Unicode.GetBytes(plaintext);
            byte[] cypher = csp.Encrypt(data, false);
            return Convert.ToBase64String(cypher);
        }
    }
}