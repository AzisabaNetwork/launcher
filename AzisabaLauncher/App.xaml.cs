using AzisabaLauncher.Minecraft;
using Microsoft.UI.Xaml;

namespace AzisabaLauncher
{
    public partial class App : Application
    {
        private Window? m_window;

        public App()
        {
            this.InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            this.m_window = new MainWindow();
            this.m_window.Activate();

            this.OnLaunchedAsync(args);
        }

        protected async void OnLaunchedAsync(LaunchActivatedEventArgs args)
        {
            await Version.Fetch();
        }
    }
}
