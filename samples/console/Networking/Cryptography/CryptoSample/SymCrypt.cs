using System.Security.Cryptography;
using System.Text;

namespace Crypto;

public class SymCrypt
{
    private readonly SymmetricAlgorithm _symAlg;

    public byte[] Key => _symAlg.Key;
    public byte[] IV => _symAlg.IV;

    public SymCrypt(SymmetricAlgorithm symAlg, string? strKey = null, byte[]? iv = null)
    {
        _symAlg = symAlg;

        if (strKey != null)
        {
            _symAlg.Key = Encoding.UTF8.GetBytes(strKey);
        }
        else
        {
            _symAlg.GenerateKey();
        }

        if (iv != null)
        {
            _symAlg.IV = iv;
        }
        else
        {
            _symAlg.GenerateIV();
        }
    }

    public string EncryptData(string strData)
    {
        byte[] inputByteArray = Encoding.UTF8.GetBytes(strData);
        using var memoryStream = new MemoryStream();
        using var encryptor = _symAlg.CreateEncryptor(_symAlg.Key, _symAlg.IV);
        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
        {
            cryptoStream.Write(inputByteArray, 0, inputByteArray.Length);
            cryptoStream.FlushFinalBlock();
        }

        byte[] cipher = memoryStream.ToArray();
        return Convert.ToBase64String(cipher);
    }

    public string DecryptData(string strData)
    {
        byte[] inputByteArray = Convert.FromBase64String(strData);
        using var memoryStream = new MemoryStream();
        using var decryptor = _symAlg.CreateDecryptor(_symAlg.Key, _symAlg.IV);
        using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Write))
        {
            cryptoStream.Write(inputByteArray, 0, inputByteArray.Length);
            cryptoStream.FlushFinalBlock();
        }

        return Encoding.UTF8.GetString(memoryStream.ToArray());
    }
}
