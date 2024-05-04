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
        
        private static BigInteger p = 11; // Prime modulus
        private static BigInteger g = 5;  // Generator
        private static BigInteger q = 5;  // Prime order
        private static BigInteger w;      // Private key (user age)

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
        // Commitment phase
        public static (BigInteger, BigInteger) Commit(string age)
        {
            BigInteger r;
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] bytes = new byte[16];
                rng.GetBytes(bytes);
                r = new BigInteger(bytes);
            }

            BigInteger a = BigInteger.ModPow(g, r, p); // Compute g^r mod p
            BigInteger c = H(a.ToString()); // Compute hash of a

            BigInteger z = (r + (w * c)) % q; // Compute z = r + wc mod q

            return (a, z);
        }



    }
}
