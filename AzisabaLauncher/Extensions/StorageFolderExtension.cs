using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace AzisabaLauncher.Extensions
{
    public static class StorageFolderExtension
    {
        public static async Task<bool> Contains(this StorageFolder folder, string name)
        {
            var files = await folder.GetFilesAsync();
            var folders = await folder.GetFoldersAsync();

            return files.Any(file => file.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) ||
                folders.Any(folder => folder.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
