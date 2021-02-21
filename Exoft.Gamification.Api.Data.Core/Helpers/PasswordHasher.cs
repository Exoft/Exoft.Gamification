using Exoft.Gamification.Api.Data.Core.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Exoft.Gamification.Api.Data.Core.Helpers
{
    public class PasswordHasher : IPasswordHasher
    {
        public bool Compare(string password, string hashedPassword)
        {
            return GetHash(password) == hashedPassword;
        }

        public string GetHash(string input)
        {
            using (var algorithm = SHA512.Create())
            {
                var data = algorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

                var sBuilder = new StringBuilder();
                foreach (var c in data)
                {
                    sBuilder.Append(c.ToString("x2"));
                }

                return sBuilder.ToString();
            }
        }
    }
}
