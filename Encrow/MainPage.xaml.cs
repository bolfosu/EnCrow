namespace Encrow
{
    public partial class MainPage : ContentPage
    {
      

        public MainPage()
        {
            InitializeComponent();
        }

       

        private void ProveButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new QRCodePage());
        }

        private void VerifyButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new VerifierPage());
        }
    }

}
