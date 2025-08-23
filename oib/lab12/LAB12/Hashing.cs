using System.Security.Cryptography;
using System.Text;

public class HashingSHA1
{
    public string ComputeHash(string text)
    {
        using (SHA1 sha1 = SHA1.Create())
        {
            byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(text));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}