using System.Security.Cryptography;
using System.Numerics;

namespace Encrow
{
    public class ZkProof
    {
        private readonly int _primeModulus = 23;
        private readonly int _generator = 3;
        private readonly int _primeOrder = 11;
        private readonly int _secretValue = 18;
       





        public (int, int) ProveKnowledge(int age)
        {


            using (var rng = RandomNumberGenerator.Create()) // Use a secure random number generator
            {
                byte[] randomNumberBytes = new byte[4]; // Assuming 4 bytes for integer representation
                rng.GetBytes(randomNumberBytes);
                int randomInteger = BitConverter.ToInt32(randomNumberBytes, 0);
                randomInteger %= _primeOrder; // Ensure the value falls within Z_q

                int publicKey = ModPow(_generator, _secretValue, _primeModulus);
                int commitment = ModPow(_generator, randomInteger, _primeModulus);
                int hashValue = SimpleHash(commitment);

                int response = (randomInteger + (_secretValue * hashValue)) % _primeOrder;

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
            string dataString = Convert.ToString(data, 16); // Convert integer to hexadecimal string
            foreach (char c in dataString)
            {
                hash = (hash * 37 + (int)c) % int.MaxValue;
            }
            return hash;
        }


        private static int ConvertToInt(byte[] hash)
        {
            int hashValue = 0;
            for (int i = 0; i < hash.Length; i++)
            {
                hashValue |= (hash[i] << (8 * i)); // Combine bytes into an integer
            }
            return hashValue;
        }
    }
}
