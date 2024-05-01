using System.Net.NetworkInformation;
using ZXing.Mobile;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.PlatformConfiguration.Android.App;
using Android.Content;
using Android.OS;
using QRCodeScanner.Droid.Services;
using Xamarin.Essentials;
using ZXing.Mobile;

namespace Encrow;

public partial class ProverPage : ContentPage
{
	public ProverPage()
	{
		InitializeComponent();
	}
    public async Task<string> ScanAsync()
    {
        MobileBarcodeScanner.Initialize(Application);
        var optionsCustom = new MobileBarcodeScanningOptions
        {
            PossibleFormats = new List<ZXing.BarcodeFormat>
            {
                    ZXing.BarcodeFormat.QR_CODE
                }
        };

        var result = await MobileBarcodeScanner
            .Scan(optionsCustom);

        return result.Text;
    }
}