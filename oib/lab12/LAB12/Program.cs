using System;
using System.Security.Cryptography;

class MainProgram
{
    static void Main(string[] args)
    {
        string surname = "Харченко Евгений Игоревич";
        string alteredSurname = "Харченко Игорь Игоревич";

        CryptographyAES aes = new CryptographyAES();
        HashingSHA1 sha1 = new HashingSHA1();
        DigitalSignature ds = new DigitalSignature();
        FileManager fileManager = new FileManager();

        byte[] key = new byte[16];
        byte[] iv = new byte[16];

        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(iv);

        // Сохранение ключей
        fileManager.SaveToFile("Key.txt", Convert.ToBase64String(key));
        fileManager.SaveToFile("IV.txt", Convert.ToBase64String(iv));

        // Сохранение ключей в HEX-формате
        fileManager.SaveToFile("Key.hex", BitConverter.ToString(key).Replace("-", ""));
        fileManager.SaveToFile("IV.hex", BitConverter.ToString(iv).Replace("-", ""));

        // Шифрование фамилии
        string encryptedSurname = aes.Encrypt(surname, key, iv);
        fileManager.SaveToFile("EncryptedSurname.txt", encryptedSurname);

        // Сохранение зашифрованных данных в HEX-формате
        byte[] encryptedBytes = Convert.FromBase64String(encryptedSurname);
        fileManager.SaveToFile("EncryptedSurname.hex", BitConverter.ToString(encryptedBytes).Replace("-", ""));

        // Дешифрование фамилии
        string decryptedSurname = aes.Decrypt(encryptedSurname, key, iv);
        Console.WriteLine($"Decrypted Surname: {decryptedSurname}");

        // Хеширование фамилии
        string hash = sha1.ComputeHash(surname);
        fileManager.SaveToFile("Hash.txt", hash);

        // Симуляция изменения хэша
        string alteredHash = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF"; // Неправильный хэш

        // Чтение хэша из файла
        string readHash = fileManager.ReadFromFile("Hash.txt");

        // Проверка оригинального сообщения и хэша
        bool isOriginalValid = ds.VerifyMessage(surname, surname, hash, readHash);
        Console.WriteLine($"Original Message and Hash valid: {isOriginalValid}");

        // Проверка изменённого сообщения
        bool isAlteredMessageValid = ds.VerifyMessage(surname, alteredSurname, hash, readHash);
        Console.WriteLine($"Altered Message Valid: {isAlteredMessageValid}");

        // Проверка изменённого хэша
        bool isAlteredHashValid = ds.VerifyMessage(surname, surname, hash, alteredHash);
        Console.WriteLine($"Altered Hash Valid: {isAlteredHashValid}");
    }
}