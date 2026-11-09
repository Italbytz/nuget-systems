using System.Security.Cryptography;

namespace Crypto.Tests;

[TestClass]
public class AsymTests
{
    private const string Plaintext = "Hello World!";

    [TestMethod]
    public void TestRSA()
    {
        using var rsa = RSA.Create(2048);
        var publicKey = rsa.ExportParameters(false);
        var privateKey = rsa.ExportParameters(true);

        var encryptedData = RSACSPSample.RSAEncrypt(Plaintext, publicKey, doOaepPadding: false);
        var decryptedData = RSACSPSample.RSADecrypt(encryptedData, privateKey, doOaepPadding: false);

        Assert.AreEqual(Plaintext, decryptedData);
    }

    [TestMethod]
    public void TestRSAWithOAEP()
    {
        using var rsa = RSA.Create(2048);
        var publicKey = rsa.ExportParameters(false);
        var privateKey = rsa.ExportParameters(true);

        var encryptedData = RSACSPSample.RSAEncrypt(Plaintext, publicKey, doOaepPadding: true);
        var decryptedData = RSACSPSample.RSADecrypt(encryptedData, privateKey, doOaepPadding: true);

        Assert.AreEqual(Plaintext, decryptedData);
    }
}
