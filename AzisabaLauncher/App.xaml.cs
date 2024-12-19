using AzisabaLauncher.Minecraft;
using Microsoft.UI.Xaml;
using System.Net.Http;
using Windows.Storage;

namespace AzisabaLauncher
{
    public partial class App : Application
    {
        public static readonly HttpClient HttpClient = new HttpClient();

        public static readonly StorageFolder AppDirectory = ApplicationData.Current.LocalFolder;

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
            await MinecraftVersion.Fetch();
        }
    }
}
