using System.Security.Cryptography;

namespace Crypto.Tests;

[TestClass]
public class SymTests
{
    private const string Plaintext = "Hello World!";

    [TestMethod]
    public void TestAES()
    {
        using var aesAlg = Aes.Create();
        var aes = new SymCrypt(aesAlg);
        var encrypted = aes.EncryptData(Plaintext);
        var decrypted = aes.DecryptData(encrypted);
        Assert.AreEqual(Plaintext, decrypted);
    }

    [TestMethod]
    public void TestDES()
    {
        using var desAlg = DES.Create();
        var des = new SymCrypt(desAlg);
        var encrypted = des.EncryptData(Plaintext);
        var decrypted = des.DecryptData(encrypted);
        Assert.AreEqual(Plaintext, decrypted);
    }

    [TestMethod]
    public void TestTripleDES()
    {
        using var tripleDesAlg = TripleDES.Create();
        var tripleDes = new SymCrypt(tripleDesAlg);
        var encrypted = tripleDes.EncryptData(Plaintext);
        var decrypted = tripleDes.DecryptData(encrypted);
        Assert.AreEqual(Plaintext, decrypted);
    }
}
