using DarkRift;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using MUD.Encryption;

namespace MUD
{
    namespace Encryption
    {
        public class RSAEncryption
        {
            private static RSACryptoServiceProvider csp = new RSACryptoServiceProvider(2048);
            private RSAParameters _privatekey;
            protected RSAParameters _publickey;

            public RSAEncryption()
            {
                _privatekey = csp.ExportParameters(true);
                _publickey = csp.ExportParameters(false);
            }

            public string GetPublicKey()
            {
                StringWriter sw = new StringWriter();
                XmlSerializer xs = new XmlSerializer(typeof(RSAParameters));
                xs.Serialize(sw, _publickey);
                return sw.ToString();
            }

            public string Decrypt(string cypher)
            {
                byte[] data = Convert.FromBase64String(cypher);
                csp.ImportParameters(_privatekey);
                byte[] plaintext = csp.Decrypt(data, false);
                return Encoding.Unicode.GetString(plaintext);
            }
        }
    }

    namespace Net
    {
        public class Verification : IDarkRiftSerializable
        {
            public string key;

            public void Deserialize(DeserializeEvent e)
            {
                key = e.Reader.ReadString();
            }

            public void Serialize(SerializeEvent e)
            {
                e.Writer.Write(key);
            }
        }

        public class Login : IDarkRiftSerializable
        {
            public string username;
            public string password;

            public void Deserialize(DeserializeEvent e)
            {
                username = e.Reader.ReadString();
                password = e.Reader.ReadString();
            }

            public void Decrypt(RSAEncryption rsa)
            {
                password = rsa.Decrypt(password);
            }

            public void Serialize(SerializeEvent e)
            {
                e.Writer.Write(username);
                e.Writer.Write(password);
            }
        }
    }
}
