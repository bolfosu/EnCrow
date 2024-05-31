﻿using System;
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
        public QRCodePage()
        {
            InitializeComponent();
            this.Appearing += QRCodePage_Selfie;
        }

        private void OnGenerateClicked(object sender, EventArgs e)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode("23", QRCodeGenerator.ECCLevel.L);
            PngByteQRCode qRCode = new PngByteQRCode(qrCodeData);
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

