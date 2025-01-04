using System;
using System.Text;

namespace Business.Utilities;

public static class GenerateToken
{
    public static string Generate(this string str)
    {
        string guid = Guid.NewGuid().ToString();
        return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{str}:{guid}"));
    }
}
