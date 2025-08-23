using System;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main()
    {
        // Сообщение для подписи
        string message = "Это сообщение должно быть подписано.";
        Console.WriteLine($"Сообщение: {message}");

        // Преобразуем сообщение в байты
        byte[] messageBytes = Encoding.UTF8.GetBytes(message);

        // Генерация ключей RSA
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        {
            // Экспорт открытого и закрытого ключа
            string publicKeyXml = rsa.ToXmlString(false); // только открытый
            string privateKeyXml = rsa.ToXmlString(true); // и открытый, и закрытый

            // Подпись сообщения
            byte[] signature;
            using (RSACryptoServiceProvider rsaForSigning = new RSACryptoServiceProvider())
            {
                rsaForSigning.FromXmlString(privateKeyXml);
                signature = rsaForSigning.SignData(messageBytes, new SHA256CryptoServiceProvider());
            }

            Console.WriteLine($"\nЦифровая подпись (в Base64): {Convert.ToBase64String(signature)}");

            // Проверка подписи
            bool isVerified;
            using (RSACryptoServiceProvider rsaForVerify = new RSACryptoServiceProvider())
            {
                rsaForVerify.FromXmlString(publicKeyXml);
                isVerified = rsaForVerify.VerifyData(messageBytes, new SHA256CryptoServiceProvider(), signature);
            }

            Console.WriteLine(isVerified
                ? "\nПодпись подтверждена: сообщение подлинное и не изменялось."
                : "\nПодпись недействительна: сообщение было изменено или ключ неверный.");
        }
    }
}
