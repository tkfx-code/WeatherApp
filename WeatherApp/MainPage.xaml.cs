using WeatherApp.ViewModels;

namespace WeatherApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        //Override Size background to solve responsivity issues
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (width > 0 && height > 0)
            {
                double windowAspect = width / height;
                double imageAspect = 1920.0 / 1080.0;

                if (windowAspect > imageAspect)
                {
                    BackgroundImage.Aspect = Aspect.AspectFill;
                }
                else
                {
                    BackgroundImage.Aspect = Aspect.Fill;
                }
            }
        }
    }
}
