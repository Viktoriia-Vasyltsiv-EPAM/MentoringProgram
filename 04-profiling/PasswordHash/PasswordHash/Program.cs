﻿using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace PasswordHash
{
    internal class Program
    {
        private const int iterate = 10000;
        static void Main(string[] args)
        {
            const string password = "torrefatto_e_macinato";
            var hash = GeneratePasswordHashUsingSalt(password, GetBytes(password));
            Console.WriteLine(hash);
        }

        static string GeneratePasswordHashUsingSalt(string passwordText, byte[] salt)
        {
            byte[] hashBytes = new byte[36];
            var hash = KeyDerivation.Pbkdf2(passwordText, salt, KeyDerivationPrf.HMACSHA1, iterate, 20);
            Buffer.BlockCopy(salt, 0, hashBytes, 0, 16);
            Buffer.BlockCopy(hash, 0, hashBytes, 16, 20);

            return Convert.ToBase64String(hashBytes);
        }

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}
