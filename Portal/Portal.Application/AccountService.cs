using System.Security.Cryptography;
using System.Text;

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
