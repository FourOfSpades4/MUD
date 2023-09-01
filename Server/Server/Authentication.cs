using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MUD.SQL;

namespace MUD.Encryption
{
    internal class Authentication
    {
        public static Authentication instance = new Authentication();
        
        // SHA256 Hash to Hash Passwords
        public static string Hash(string str)
        {
            return Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(str)));
        }

        // Query Database for username and password verification
        public static bool VerifyCredentials(string user, string pass)
        {
            return Database.instance.VerifyPlayerCredentials(user, Hash(pass));
        }

        // Generate Random Token
        public static string GetToken()
        {
            using (var cryptoProvider = new RNGCryptoServiceProvider())
            {
                byte[] bytes = new byte[64];
                cryptoProvider.GetBytes(bytes);

                return Convert.ToBase64String(bytes);
            }
        }
    }
}
