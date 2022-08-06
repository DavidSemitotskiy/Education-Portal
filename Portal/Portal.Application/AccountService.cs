using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Application
{
    internal class AccountService
    {
        public string GetHashPassword(string password)
        {
            MD5 mD5 = MD5.Create();
            var hashPassword = mD5.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashPassword);
        }
    }
}
