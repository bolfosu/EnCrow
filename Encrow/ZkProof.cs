using System.Security.Cryptography;
using System.Numerics;

namespace Encrow
{
    public class ZkProof
    {
        // Public constants for easier modification
        public const int CommitmentModulus = 23;
        public const int Generator = 3;
        public const int Order = 11;

        public (int, int, int) ProveKnowledge(int age)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                // Generate random integer within Z_q
                byte[] randomNumberBytes = new byte[4];
                rng.GetBytes(randomNumberBytes);
                int randomInteger = Math.Abs(BitConverter.ToInt32(randomNumberBytes, 0)) % Order;

                // Calculate public key and commitment
                int publicKey = ModPow(Generator, age, CommitmentModulus);
                int commitment = ModPow(Generator, randomInteger, CommitmentModulus);

                // Hash the commitment and public key together and convert to integer
                int hashValue = HashToInt(commitment, publicKey);

                // Calculate response
                int response = (randomInteger + (age * hashValue)) % Order;
                if (response < 0) response += Order;

                return (commitment, response, publicKey);
            }
        }

        private static int ModPow(int baseValue, int exponent, int modulus)
        {
            int result = 1;
            baseValue = baseValue % modulus;
            while (exponent > 0)
            {
                if ((exponent & 1) == 1)
                    result = (result * baseValue) % modulus;
                exponent >>= 1;
                baseValue = (baseValue * baseValue) % modulus;
            }
            return result;
        }

        private static int HashToInt(int commitment, int publicKey)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] commitmentBytes = BitConverter.GetBytes(commitment);
                byte[] publicKeyBytes = BitConverter.GetBytes(publicKey);
                byte[] combined = new byte[commitmentBytes.Length + publicKeyBytes.Length];
                Buffer.BlockCopy(commitmentBytes, 0, combined, 0, commitmentBytes.Length);
                Buffer.BlockCopy(publicKeyBytes, 0, combined, commitmentBytes.Length, publicKeyBytes.Length);

                byte[] hash = sha256.ComputeHash(combined);
                return Math.Abs(BitConverter.ToInt32(hash, 0)) % Order;
            }
        }
    }
}
