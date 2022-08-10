using System.Security.Cryptography;
using System.Text;

namespace Portal.Application
{
    internal class AccountService
    {
        public static string GetHashPassword(string password)
        {
            var mD5 = MD5.Create();
            var hashPassword = mD5.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashPassword);
        }
    }
}
