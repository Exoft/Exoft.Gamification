using Exoft.Gamification.Api.Data.Core.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Exoft.Gamification.Api.Data.Core.Helpers
{
    public class MD5Hash : IMD5Hash
    {
        public string GetMD5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

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
