using System;
using System.Text;
using static System.Random;


namespace Encrow
{


    public class ZkProof
    {
        private readonly int _primeModulus = 11;
        private readonly int _generator = 5;
        private readonly int _privateKey = 23;
        private readonly int _randomValue;

        public int GeneratePublicKey()
        {
            return ModPow(_generator, _privateKey, _primeModulus);
        }

        public (int, int) GenerateCommitment(int randomValue)
        {
            Random random = new Random();
            int _randomValue = random.Next(1, int.MaxValue); 

            int commitment = ModPow(_generator, _randomValue, _primeModulus);
            int hash = HashFunction(commitment.ToString()); // Convert commitment to string for hashing
            int challenge = (randomValue + _privateKey * hash) % 5; // Use prime order q for modulo

            return (commitment, challenge);
        }

        private static int ModPow(int baseValue, int exponent, int modulus)
        {
            long result = 1;
            while (exponent > 0)
            {
                if ((exponent & 1) == 1)
                {
                    result = (result * baseValue) % modulus;
                }
                exponent >>= 1;
                baseValue = (baseValue * baseValue) % modulus;
            }
            return (int)result;
        }

        // Simple hash function (replace with a secure hash function like SHA-256 in a real application)
        private int HashFunction(string data)
        {
            int hash = 0;
            foreach (char c in data)
            {
                hash = (hash * 31 + (int)c) % _primeModulus; // Use prime modulus for modulo operation
            }
            return hash;
        }
    }
}