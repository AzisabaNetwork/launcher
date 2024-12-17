using Microsoft.UI.Xaml;

namespace AzisabaLauncher
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            this.AppWindow.SetIcon(@"Assets/Resources/Icon.ico");
            this.AppWindow.Resize(new Windows.Graphics.SizeInt32(1024, 640));
        }
    }
}
