using System.Numerics;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Encrow
{
    public partial class VerifierPage : ContentPage
    {
        private readonly int _primeModulus = 23;
        public const int Order = 11;
        private readonly int _generator = 3; // Ensure this matches the generator used in ZkProof
        private readonly int publicKey = 10; // Make sure this is the actual public key

        public VerifierPage()
        {
            InitializeComponent();
            cameraView.BarCodeOptions = new()
            {
                TryHarder = true,
            };
            cameraView.BarCodeDetectionEnabled = true;
        }

        private void cameraView_CamerasLoaded(object sender, EventArgs e)
        {
            if (cameraView.Cameras.Count > 0)
            {
                cameraView.Camera = cameraView.Cameras.First();
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await cameraView.StopCameraAsync();
                    await cameraView.StartCameraAsync();
                });
            }
        }

        private void cameraView_BarcodeDetected(object sender, Camera.MAUI.ZXingHelper.BarcodeEventArgs args)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                string qrCodeData = args.Result[0].Text;

                // Regex pattern to match the QR code data format
                string pattern = @"^\s*(\d+|-?\d+)\s*,\s*(\d+|-?\d+)\s*(\n*)*$";

                Match match = Regex.Match(qrCodeData, pattern);
                if (!match.Success)
                {
                    barcodeResult.Text = "Invalid QR code format";
                    barcodeResult.BackgroundColor = Colors.Red;
                    return;
                }

                string commitmentString = match.Groups[1].Value;
                string responseString = match.Groups[2].Value;

                int commitment = int.Parse(commitmentString);

                int response;
                if (responseString.StartsWith("-"))
                {
                    response = -int.Parse(responseString.Substring(1));
                }
                else
                {
                    response = int.Parse(responseString);
                }

                // Perform verification 
                bool verified = Verify(commitment, response);

                barcodeResult.Text = verified ? "Accepted" : "Rejected";
                barcodeResult.BackgroundColor = verified ? Colors.Green : Colors.Red;
                commitmentLabel.Text = "Commitment: " + commitment;
                responseLabel.Text = "Response: " + response;
            });
        }

        private bool Verify(int commitment, int response)
        {
            // Calculate hash value 
            int hashValue = HashToInt(commitment, publicKey);

            // Display hash value in a new label
            hashValueLabel.Text = "Hash Value: " + hashValue;

            // Calculate left side of the formula (generator raised to power of response)
            BigInteger leftSide = BigInteger.ModPow(_generator, response, _primeModulus);

            // Calculate right side of the formula (public key raised to hash value, multiplied by commitment)
            BigInteger rightSide = (BigInteger.ModPow(publicKey, hashValue, _primeModulus) * commitment) % _primeModulus;

            // Verify congruence (remainders after division by prime modulus)
            return leftSide == rightSide;
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
