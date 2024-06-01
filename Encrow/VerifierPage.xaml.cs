using Camera.MAUI;
using System.Numerics;


namespace Encrow;

public partial class VerifierPage : ContentPage
{


    public VerifierPage()
    {
        InitializeComponent();
        barcodeReader.Options = new ZXing.Net.Maui.BarcodeReaderOptions
        {

            Formats = ZXing.Net.Maui.BarcodeFormat.QrCode,
            AutoRotate = true,
            Multiple = true,
            
        };

    }

    private void barcodeReader_BarcodeDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
        
    {
        var first = e.Results.FirstOrDefault();
        if (first == null)
        {
            return;

            Dispatcher.DispatchAsync(async () =>
            {
                await DisplayAlert("Barcode detected", first.Value, "OK");
            });
        }
    }
}
   

   

   



