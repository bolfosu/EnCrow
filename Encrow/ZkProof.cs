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

        public (int, int) ProveKnowledge(int age)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                // Generate random integer within Z_q
                byte[] randomNumberBytes = new byte[4];
                rng.GetBytes(randomNumberBytes);
                int randomInteger = BitConverter.ToInt32(randomNumberBytes, 0) % Order;

                // Calculate public key and commitment
                int publicKey = ModPow(Generator, age, CommitmentModulus);
                int commitment = ModPow(Generator, randomInteger, CommitmentModulus);

                // Hash the commitment and convert to integer (consider alternatives)
                int hashValue = SimpleHash(commitment);

                // Calculate response
                int response = (randomInteger + (age * 49)) % Order;

                return (commitment, response);
            }
        }

        private static int ModPow(int baseValue, int exponent, int modulus)
        {
            int result = 1;
            while (exponent > 0)
            {
                if ((exponent & 1) == 1)
                {
                    result = (result * baseValue) % modulus;
                }
                exponent >>= 1;
                baseValue = (baseValue * baseValue) % modulus;
            }
            return result;
        }

        public static int SimpleHash(int data)
        {
            int hash = 0;
            string dataString = data.ToString(); // Convert integer to string

            // Iterate through each character in the string representation
            foreach (char c in dataString)
            {
                // Combine the current hash with the character's ASCII code
                // and a prime number (37) to improve distribution
                hash = (hash * 37 + (int)c) % int.MaxValue;
            }
            return hash;
        }

    }
}
