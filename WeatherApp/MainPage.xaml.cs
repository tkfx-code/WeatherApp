namespace WeatherApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage() //Should i take in viewmodel as parameter in my constructor?
        {
            InitializeComponent();
            //BindingContext = viewModel;
            //viewModel.Param = "";
            //viewModel.Object.Update();
        }
    }
}
