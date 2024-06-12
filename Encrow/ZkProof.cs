using System.Security.Cryptography;
using System.Numerics;

namespace Encrow
{
    public class ZkProof
    {
        private readonly int _primeModulus = 23;
        private readonly int _generator = 3;
        private readonly int _primeOrder = 11;
        
       



        public (int, int) ProveKnowledge(int age)
        {


            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] randomNumberBytes = new byte[4]; 
                rng.GetBytes(randomNumberBytes);
                int randomInteger = BitConverter.ToInt32(randomNumberBytes, 0);
                randomInteger %= _primeOrder; // Ensure the value falls within Z_q

                int publicKey = ModPow(_generator, age, _primeModulus);
                int commitment = ModPow(_generator, randomInteger, _primeModulus);
                int hashValue = SimpleHash(commitment);

                int response = (randomInteger + (age * hashValue)) % _primeOrder;

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
            string dataString = Convert.ToString(data, 16); 
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
                hashValue |= (hash[i] << (8 * i)); 
            }
            return hashValue;
        }
    }
}
