using System;
using System.Collections.Generic;
using ZXing.Mobile;

namespace YourNamespace
{
    public partial class YourPage : ContentPage
    {
        MobileBarcodeScanner scanner;

        public YourPage()
        {
            InitializeComponent();
            scanner = new MobileBarcodeScanner();
        }

        async void ScanButton_Clicked(object sender, EventArgs e)
        {
            // Initialize scanner options
            var options = new MobileBarcodeScanningOptions
            {
                // Customize scanning options if needed
                PossibleFormats = new List<ZXing.BarcodeFormat> { ZXing.BarcodeFormat.QR_CODE }
            };

            // Start scanning
            var result = await scanner.Scan(options);

            // Check if a barcode was found
            if (result != null)
            {
                // Handle the scanned barcode result
                HandleResult(result.Text);
            }
        }

        void HandleResult(string resultText)
        {
            // Process the result as needed
            DisplayAlert("Scanned Barcode", resultText, "OK");
        }
    }
}
