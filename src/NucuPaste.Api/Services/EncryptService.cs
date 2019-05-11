using System;
using System.Security.Cryptography;

namespace NucuPaste.Api.Services
{
    public class EncryptService : IEncrypt
    {
        private const int SaltSize = 40;
        private const int DeriveBytesIterationCount = 100000;

        public string GetSalt(string value)
        {
            var saltBytes = new byte[SaltSize];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(saltBytes);

            return Convert.ToBase64String(saltBytes);
        }

        public static byte[] GetBytes(string value)
        {
            var bytes = new byte[value.Length*sizeof(char)];
            Buffer.BlockCopy(value.ToCharArray(), 0, bytes, 0,bytes.Length);
            return bytes;
        }

        public string GetHash(string value, string salt)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(value, GetBytes(salt), DeriveBytesIterationCount);
            return Convert.ToBase64String(pbkdf2.GetBytes(SaltSize));
        }
    }
}