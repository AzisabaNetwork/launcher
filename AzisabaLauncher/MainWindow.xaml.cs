using AzisabaLauncher.Minecraft;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
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

            foreach (var version in MinecraftVersion.GetInstances())
            {
                ComboBoxItem item = new ComboBoxItem();

                StackPanel stackPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    VerticalAlignment = VerticalAlignment.Center
                };

                Image image = new Image
                {
                    Height = 32,
                    Source = new BitmapImage(new Uri("ms-appx:///Assets/Minecraft.png")),
                    VerticalAlignment = VerticalAlignment.Center,
                    Width = 32
                };

                TextBlock textBlock = new TextBlock()
                {
                    Margin = new Thickness(10, 0, 0, 0),
                    Text = version.Name,
                    VerticalAlignment = VerticalAlignment.Center
                };

                stackPanel.Children.Add(image);
                stackPanel.Children.Add(textBlock);
                item.Content = stackPanel;
                Versions.Items.Add(item);
            }
        }
    }
}
