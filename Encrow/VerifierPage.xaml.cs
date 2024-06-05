using System.Numerics;
using System.Text;
using Camera.MAUI;
using QRCoder;
using System.Text.RegularExpressions;

namespace Encrow
{
    public partial class VerifierPage : ContentPage
    {
        private readonly int _primeModulus = 23;
        private readonly int _generator = 3;
        private readonly int publicKey = 2;

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
                qrCodeDataLabel.Text = "QR Code Data: " + qrCodeData;

                // Regular expression for colon-separated values with optional whitespace and negative response
                string pattern = @"^\s*(\d+|-?\d+)\s*,\s*(\d+)\s*(\n*)*$";

                Match match = Regex.Match(qrCodeData, pattern);
                if (!match.Success)
                {
                    barcodeResult.Text = "Invalid QR code format";
                    return;
                }

                string commitmentString = match.Groups[1].Value;
                string responseString = match.Groups[2].Value;

                // Parse commitment (positive or negative)
                int commitment = int.Parse(commitmentString);

                // Parse response (considering negative sign)
                int response;
                if (responseString.StartsWith("-"))
                {
                    response = -int.Parse(responseString.Substring(1)); // Remove leading hyphen and parse as negative
                }
                else
                {
                    response = int.Parse(responseString); // Positive value
                }

                // Perform verification with SimpleHash challenge computation (for demonstration purposes)
                bool verified = Verify(commitment, response);

                barcodeResult.Text = verified ? "Good" : "Not Good";
            });
        }

        private bool Verify(int commitment, int response)
        {
            // Calculate hash value using SimpleHash (not cryptographically secure)
            int hashValue = SimpleHash(commitment);

            // Calculate left side of the formula (generator raised to power of response)
            BigInteger leftSide = BigInteger.Pow(_generator, response) % _primeModulus;

            // Calculate right side of the formula (public key raised to hash value, multiplied by commitment)
            BigInteger rightSide = (BigInteger.Pow(publicKey, hashValue) * commitment) % _primeModulus;

            // Verify congruence (remainders after division by prime modulus)
            return leftSide == rightSide;
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
    }
}
