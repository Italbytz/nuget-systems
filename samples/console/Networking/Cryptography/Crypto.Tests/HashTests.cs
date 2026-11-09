using System.Security.Cryptography;
using System.Text;

namespace Crypto.Tests;

[TestClass]
public class HashTests
{
    private const string Plaintext = "Data to compute hash for";
    private const string ExpectedSha1Hash = "46-CD-85-07-8D-27-32-1D-76-15-28-0F-F8-34-82-A5-DD-09-64-60";

    [TestMethod]
    public void TestSHA1()
    {
        byte[] data = Encoding.UTF8.GetBytes(Plaintext);
        byte[] result = SHA1.HashData(data);
        Assert.AreEqual(ExpectedSha1Hash, BitConverter.ToString(result));
    }

    [TestMethod]
    public void TestSHA256()
    {
        byte[] data = Encoding.UTF8.GetBytes("Hello World!");
        byte[] result = SHA256.HashData(data);
        string hex = Convert.ToHexString(result);
        Assert.AreEqual(64, hex.Length); // 256 Bit = 32 Bytes = 64 Hex-Zeichen
    }
}
