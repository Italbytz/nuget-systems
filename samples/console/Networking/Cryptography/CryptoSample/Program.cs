using System.Security.Cryptography;
using System.Text;

namespace Crypto;

internal static class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("=== Kryptographie in C# (System.Security.Cryptography) ===\n");

        const string plaintext = "Vertrauliche Nachricht für Rechnernetze 2026!";
        Console.WriteLine($"Original-Klartext: \"{plaintext}\"\n");

        // 1. Symmetrische Verschlüsselung (AES-256)
        Console.WriteLine("--- 1. Symmetrische Verschlüsselung (AES) ---");
        using var aesAlg = Aes.Create();
        var aes = new SymCrypt(aesAlg);
        Console.WriteLine($"Key (Hex): {Convert.ToHexString(aes.Key)} ({aes.Key.Length * 8} Bit)");
        Console.WriteLine($"IV  (Hex): {Convert.ToHexString(aes.IV)} ({aes.IV.Length * 8} Bit)");

        string cipherText = aes.EncryptData(plaintext);
        Console.WriteLine($"Verschlüsselt (Base64): {cipherText}");

        string decrypted = aes.DecryptData(cipherText);
        Console.WriteLine($"Entschlüsselt:          \"{decrypted}\"");
        Console.WriteLine($"Übereinstimmung:        {plaintext == decrypted}\n");

        // 2. Asymmetrische Verschlüsselung (RSA-2048)
        Console.WriteLine("--- 2. Asymmetrische Verschlüsselung (RSA) ---");
        using var rsa = RSA.Create(2048);
        var publicKey = rsa.ExportParameters(false); // nur öffentlicher Schlüssel (e, n)
        var privateKey = rsa.ExportParameters(true); // vollständiges Schlüsselpaar (d, e, n, p, q)

        Console.WriteLine("RSA-2048 Schlüsselpaar generiert.");
        Console.WriteLine($"Modulus (n, 2048 Bit Hex): {Convert.ToHexString(publicKey.Modulus!)[..32]}...");

        byte[] rsaEncrypted = RSACSPSample.RSAEncrypt(plaintext, publicKey, doOaepPadding: true);
        Console.WriteLine($"RSA-verschlüsselt (Hex): {Convert.ToHexString(rsaEncrypted)[..48]}... ({rsaEncrypted.Length} Bytes)");

        string rsaDecrypted = RSACSPSample.RSADecrypt(rsaEncrypted, privateKey, doOaepPadding: true);
        Console.WriteLine($"RSA-entschlüsselt:       \"{rsaDecrypted}\"");
        Console.WriteLine($"Übereinstimmung:         {plaintext == rsaDecrypted}\n");

        // 3. Kryptographische Hashfunktion (SHA-256)
        Console.WriteLine("--- 3. Hashfunktion (SHA-256) ---");
        byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(plaintext));
        Console.WriteLine($"SHA-256 Hash (Hex): {Convert.ToHexString(hash)} ({hash.Length * 8} Bit)");
    }
}
