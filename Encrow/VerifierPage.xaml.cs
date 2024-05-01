using ZXing.Mobile;

namespace Encrow;

public partial class VerifierPage : ContentPage
{
    MobileBarcodeScanner scanner;

    public VerifierPage()
	{
		InitializeComponent();

		scanner =  new MobileBarcodeScanner();
        {
        
        };
    }
    async void ScanButton_Clicked(object sender, EventArgs e)
    {
        var result = await scanner.Scan();

        if (result != null)
        {
            Device.BeginInvokeOnMainThread(() =>
            { DisplayAlert("Scanned QR Code", result.Text, "OK");
            });
        }
    }


}