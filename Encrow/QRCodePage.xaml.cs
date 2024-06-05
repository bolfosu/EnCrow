using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.ApplicationModel;
using QRCoder;

namespace Encrow
{
    public partial class QRCodePage : ContentPage
    {
        private ZkProof zkProof;

        public QRCodePage()
        {
            InitializeComponent();
            this.Appearing += QRCodePage_Selfie;
            zkProof = new ZkProof(23, 3, 11, 18); // Replace with your actual parameters
        }

        private void OnGenerateClicked(object sender, EventArgs e)
        {
            (int commitment, int response) = zkProof.ProveKnowledge(18); // Assuming secret value is 18

            // Combine commitment and response into a single string (modify as needed)
            string qrCodeData = $"{commitment},{response}";

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCode = qrGenerator.CreateQrCode(qrCodeData, QRCodeGenerator.ECCLevel.L);
            PngByteQRCode qRCode = new PngByteQRCode(qrCode);
            byte[] qrCodeBytes = qRCode.GetGraphic(20);
            QrCodeImage.Source = ImageSource.FromStream(() => new MemoryStream(qrCodeBytes));
        }

        private void QRCodePage_Selfie(object sender, System.EventArgs e)
        {
            // Load the image when the page is appearing
            UserSelfie.Source = ImageSource.FromFile("Resources/Images/mitidkvadrat.png");
        }
    }
}
