using System;
using System.Text;
using System.Security.Cryptography;

public static class SHA256Encoder
{
    public static string GetStringHash(string str)
    {
        var strbd = new StringBuilder();
        using (SHA256 sha256 = SHA256.Create())
        {
            var data = sha256.ComputeHash(Encoding.UTF8.GetBytes(str));
            for (var i = 0; i < data.Length; i++)
            {
                strbd.Append(data[i].ToString("x2"));
            }
        }
        return strbd.ToString().ToUpper();
    }
}

