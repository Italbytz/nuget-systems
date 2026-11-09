using System.Security.Cryptography;
using System.Text;

namespace Crypto;

public static class RSACSPSample
{
    public static byte[] RSAEncrypt(string strData, RSAParameters rsaKeyInfo, bool doOaepPadding = false)
    {
        using var rsa = RSA.Create();
        rsa.ImportParameters(rsaKeyInfo);
        var padding = doOaepPadding ? RSAEncryptionPadding.OaepSHA256 : RSAEncryptionPadding.Pkcs1;
        return rsa.Encrypt(Encoding.UTF8.GetBytes(strData), padding);
    }

    public static string RSADecrypt(byte[] dataToDecrypt, RSAParameters rsaKeyInfo, bool doOaepPadding = false)
    {
        using var rsa = RSA.Create();
        rsa.ImportParameters(rsaKeyInfo);
        var padding = doOaepPadding ? RSAEncryptionPadding.OaepSHA256 : RSAEncryptionPadding.Pkcs1;
        var decryptedData = rsa.Decrypt(dataToDecrypt, padding);
        return Encoding.UTF8.GetString(decryptedData);
    }
}
