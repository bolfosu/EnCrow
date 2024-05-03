using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Security.Cryptography;

namespace Encrow
{
    internal class ZkProof
    {
        // Public parameters
        private static BigInteger p = 11; // Prime modulus
        private static BigInteger g = 5;  // Generator
        private static BigInteger q = 5;  // Prime order
        private static BigInteger w;      // Private key

        // Hash function
        private static BigInteger H(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                return new BigInteger(hashBytes);
            }
        }
        // Generate public key
        private static BigInteger GeneratePublicKey(string age)
        {
            BigInteger wHash = H(age) % 17; // Hash the age and take modulo 17
            w = wHash;
            return BigInteger.ModPow(g, wHash, p); // Compute g^w mod p
        }


    }
}
