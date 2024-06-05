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
        private readonly int publicKey = 5;

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

                // Perform verification
                bool verified = Verify(commitment, response);

                barcodeResult.Text = verified ? "Good" : "Not Good";
            });
        }


        private bool Verify(int commitment, int response)
        {
            // Calculate expected value (without hash function)
            BigInteger expectedValue = BigInteger.Pow(_generator, response) * BigInteger.Pow(publicKey, commitment) % _primeModulus;

            // Check if the equation holds
            return commitment == (int)expectedValue;
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
