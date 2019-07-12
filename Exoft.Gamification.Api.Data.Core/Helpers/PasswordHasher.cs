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
            using (SHA512 algorithm = SHA512.Create())
            {
                byte[] data = algorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                return sBuilder.ToString();
            }
        }
    }
}
