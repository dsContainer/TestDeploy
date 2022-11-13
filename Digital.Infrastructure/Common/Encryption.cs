using System.Security.Cryptography;
using System.Text;

namespace Digital.Infrastructure.Common
{
    public class Encryption
    {
        public static string GenerateMD5(string yourString) => string.Join("", MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(yourString)).Select(s => s.ToString("x2")));
    }
}
