using Camera.MAUI;
using System.Numerics;


namespace Encrow;

public partial class VerifierPage : ContentPage
{
    private static BigInteger p = 11; // Prime modulus
    private static BigInteger g = 5;  // Generator
    private static BigInteger q = 5;  // Prime order
    private static BigInteger w = 33;      // Private key (user age)
    private static BigInteger a;
    private static BigInteger z;

    public VerifierPage()
	{
		InitializeComponent();
        cameraView.BarCodeOptions = new(){
        TryHarder = true,

            
        };

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
            barcodeResult.Text = $"{args.Result[0].BarcodeFormat}: {args.Result[0].Text}";
        });
    }

   
}


