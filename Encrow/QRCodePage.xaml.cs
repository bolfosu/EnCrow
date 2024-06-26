﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.ApplicationModel;
using QRCoder;
using System.Security.Cryptography.X509Certificates;

namespace Encrow
{
    public partial class QRCodePage : ContentPage
    {
        private ZkProof zkProof;

        public QRCodePage()
        {
            InitializeComponent();
            this.Appearing += QRCodePage_Selfie;
            zkProof = new ZkProof(); 
        }

        private void OnGenerateClicked(object sender, EventArgs e)
        {
            (int commitment, int response, int publicKey) = zkProof.ProveKnowledge(18);

            string qrCodeData = $"{commitment},{response},{publicKey}";

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCode = qrGenerator.CreateQrCode(qrCodeData, QRCodeGenerator.ECCLevel.L);
            PngByteQRCode qRCode = new PngByteQRCode(qrCode);
            byte[] qrCodeBytes = qRCode.GetGraphic(20);
            QrCodeImage.Source = ImageSource.FromStream(() => new MemoryStream(qrCodeBytes));
        }


        private void QRCodePage_Selfie(object sender, System.EventArgs e)
        {
            // Load the image when the page is appearing
            UserSelfie.Source = ImageSource.FromFile("");
        }
    }
}
