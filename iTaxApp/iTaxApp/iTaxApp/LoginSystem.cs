
using System.Security.Cryptography;
using System.Text;

namespace Core
{
    public static class LoginSystem
    {
        /// <summary>
        /// Method to calculate a 32-character long MD5 hash using cryptography.
        /// </summary>
        /// <param name="input"> any string </param>
        /// <returns> MD5 hash string </returns>
        public static string CalculateMD5Hash(string input)

        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();

        }
    }
}