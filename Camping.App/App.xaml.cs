namespace Camping.App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell())
            {
                Width = DeviceDisplay.Current.MainDisplayInfo.Width,
                Height = DeviceDisplay.Current.MainDisplayInfo.Width,
                X = 100,
                Y = 100

            };
        }
    }
}