using System;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Security;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        // === RSA ===
        Console.WriteLine("=== RSA: Шифрование и расшифровка ===");
        var rsaWatch = Stopwatch.StartNew();

        using (var rsa = RSA.Create())
        {
            var publicKey = rsa.ExportParameters(false);
            var privateKey = rsa.ExportParameters(true);

            string original = "Привет, RSA!";
            byte[] data = Encoding.UTF8.GetBytes(original);
            byte[] encrypted = rsa.Encrypt(data, RSAEncryptionPadding.Pkcs1);
            byte[] decrypted = rsa.Decrypt(encrypted, RSAEncryptionPadding.Pkcs1);

            Console.WriteLine($"Исходное сообщение: {original}");
            Console.WriteLine($"Зашифрованное сообщение (в Base64): {Convert.ToBase64String(encrypted)}");
            Console.WriteLine($"Расшифрованное сообщение: {Encoding.UTF8.GetString(decrypted)}");
        }

        rsaWatch.Stop();
        Console.WriteLine($"Время выполнения RSA: {rsaWatch.ElapsedMilliseconds} мс");

        // === Диффи — Хеллман ===
        Console.WriteLine("\n=== Диффи — Хеллман: Обмен ключами ===");
        var diffieWatch = Stopwatch.StartNew();

        using (var alice = new ECDiffieHellmanCng())
        using (var bob = new ECDiffieHellmanCng())
        {
            alice.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
            alice.HashAlgorithm = CngAlgorithm.Sha256;
            bob.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
            bob.HashAlgorithm = CngAlgorithm.Sha256;

            byte[] aliceKey = alice.DeriveKeyMaterial(bob.PublicKey);
            byte[] bobKey = bob.DeriveKeyMaterial(alice.PublicKey);

            Console.WriteLine("Совпадают ли полученные ключи: " + aliceKey.SequenceEqual(bobKey));
        }

        diffieWatch.Stop();
        Console.WriteLine($"Время выполнения Диффи — Хеллмана: {diffieWatch.ElapsedMilliseconds} мс");

        // === Эль-Гамаль ===
        Console.WriteLine("\n=== Эль-Гамаль: Шифрование и расшифровка (BouncyCastle) ===");
        var elgamalWatch = Stopwatch.StartNew();

        ElGamalParametersGenerator generator = new ElGamalParametersGenerator();
        generator.Init(512, 20, new SecureRandom());
        ElGamalParameters elgamalParameters = generator.GenerateParameters();

        ElGamalKeyGenerationParameters elgamalKeyGenParams = new ElGamalKeyGenerationParameters(new SecureRandom(), elgamalParameters);
        ElGamalKeyPairGenerator elgamalKeyGen = new ElGamalKeyPairGenerator();
        elgamalKeyGen.Init(elgamalKeyGenParams);
        AsymmetricCipherKeyPair elgamalKeyPair = elgamalKeyGen.GenerateKeyPair();

        ElGamalPublicKeyParameters elgamalPublicKey = (ElGamalPublicKeyParameters)elgamalKeyPair.Public;
        ElGamalPrivateKeyParameters elgamalPrivateKey = (ElGamalPrivateKeyParameters)elgamalKeyPair.Private;

        string elgamalMessage = "Привет, Эль-Гамаль!";
        byte[] elgamalInput = Encoding.UTF8.GetBytes(elgamalMessage);

        ElGamalEngine elgamalEngine = new ElGamalEngine();
        elgamalEngine.Init(true, new ParametersWithRandom(elgamalPublicKey, new SecureRandom()));
        byte[] elgamalEncrypted = elgamalEngine.ProcessBlock(elgamalInput, 0, elgamalInput.Length);

        Console.WriteLine($"Зашифрованное сообщение (в Base64): {Convert.ToBase64String(elgamalEncrypted)}");

        elgamalEngine.Init(false, elgamalPrivateKey);
        byte[] elgamalDecrypted = elgamalEngine.ProcessBlock(elgamalEncrypted, 0, elgamalEncrypted.Length);
        string elgamalDecryptedMessage = Encoding.UTF8.GetString(elgamalDecrypted);

        Console.WriteLine($"Расшифрованное сообщение: {elgamalDecryptedMessage}");

        elgamalWatch.Stop();
        Console.WriteLine($"Время выполнения Эль-Гамаля: {elgamalWatch.ElapsedMilliseconds} мс");

    }
}
