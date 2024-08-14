namespace AcaiMobile
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void OnAddItemButtonClicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new NewItemContentPage());
        }
    }
}