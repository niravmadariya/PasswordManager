using System.Security.Cryptography;
using System.Text;

namespace PasswordManager
{
    public sealed class CryptoService
    {
        public const int SaltSize = 32;           // 256-bit salt
        public const int KeySizeBytes = 32;       // 256-bit key
        public const int Iterations = 150_000;    // adjust per performance

        public byte[] GenerateRandomSalt()
        {
            var salt = new byte[SaltSize];
            RandomNumberGenerator.Fill(salt);
            return salt;
        }
        private static byte[] PBKDF2(byte[] password, byte[] salt, int iterations, int keySize, HashAlgorithmName hashAlgorithm)
        {
            return Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, hashAlgorithm, keySize);
        }

        public byte[] DeriveKey(string masterPassword, byte[] salt, int iterations)
        {
            var pwdBytes = Encoding.UTF8.GetBytes(masterPassword);
            return PBKDF2(pwdBytes, salt, iterations, KeySizeBytes, HashAlgorithmName.SHA256);
        }

        public (byte[] cipher, byte[] iv) EncryptString(string plain, byte[] key)
        {
            using var aes = Aes.Create();
            aes.KeySize = 256;
            aes.Key = key;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.GenerateIV();

            using var ms = new MemoryStream();
            using (var crypto = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                var bytes = Encoding.UTF8.GetBytes(plain);
                crypto.Write(bytes, 0, bytes.Length);
            }

            return (ms.ToArray(), aes.IV);
        }

        public string DecryptString(byte[] cipher, byte[] iv, byte[] key)
        {
            using var aes = Aes.Create();
            aes.KeySize = 256;
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var ms = new MemoryStream();
            using (var crypto = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
            {
                crypto.Write(cipher, 0, cipher.Length);
            }

            return Encoding.UTF8.GetString(ms.ToArray());
        }
    }
}
