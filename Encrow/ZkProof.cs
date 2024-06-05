using System.Security.Cryptography;

namespace Encrow
{
    public class ZkProof
    {
        private readonly int _primeModulus;
        private readonly int _generator;
        private readonly int _primeOrder;
        private readonly int _secretValue;

        public ZkProof(int primeModulus, int generator, int primeOrder, int secretValue)
        {
            _primeModulus = primeModulus;
            _generator = generator;
            _primeOrder = primeOrder;
            _secretValue = secretValue;
        }

        public (int, int) ProveKnowledge(int _secretValue)
        {

            using (var rng = RandomNumberGenerator.Create()) // Use a secure random number generator
            {
                byte[] randomNumberBytes = new byte[4]; // Assuming 4 bytes for integer representation
                rng.GetBytes(randomNumberBytes);
                int randomInteger = BitConverter.ToInt32(randomNumberBytes, 0);
                randomInteger %= _primeOrder; // Ensure the value falls within Z_q

                int publicKey = ModPow(_generator, _secretValue, _primeModulus);
                int commitment = ModPow(_generator, randomInteger, _primeModulus);
                int hashValue = HashFunction(commitment);
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

        public static int HashFunction(int value)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = BitConverter.GetBytes(value);
                byte[] hash = sha256.ComputeHash(bytes);
                return ConvertToInt(hash);
            }
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
