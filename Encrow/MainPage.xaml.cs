namespace Encrow
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

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
             Navigation.PushAsync(new MitId());
        }
    }

}
