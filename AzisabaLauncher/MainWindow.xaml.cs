using AzisabaLauncher.Minecraft;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Imaging;
using System;

namespace AzisabaLauncher
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            this.AppWindow.SetIcon("Assets/Resources/Icon.ico");
            this.AppWindow.Resize(new Windows.Graphics.SizeInt32(1024, 640));

            this.LoadVersions();
        }

        private async void LoadVersions()
        {
            await MinecraftVersion.Fetch();

            foreach (MinecraftVersion version in MinecraftVersion.GetInstances())
            {
                Versions.Items.Add(new VersionItemModel
                {
                    Icon = new BitmapImage(new Uri("ms-appx:///Assets/Minecraft.png")),
                    Name = version.Name!
                });
            }
        }

        private async void OnPlayButtonClicked(object sender, RoutedEventArgs e)
        {
            object? selectedItem = Versions.SelectedItem;

            if (selectedItem == null)
            {
                System.Diagnostics.Debug.WriteLine("rtn1");
                return;
            }

            VersionItemModel? model = selectedItem as VersionItemModel;

            if (model == null)
            {
                System.Diagnostics.Debug.WriteLine("rtn2");
                return;
            }

            MinecraftVersion version = MinecraftVersion.GetInstance(model.Name)!;
            await version.DownloadJarFile();
        }

        public class VersionItemModel
        {
            public required string Name { get; set; }
            public required BitmapImage Icon { get; set; }
        }
    }
}
