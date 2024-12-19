using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Windows.Storage;

namespace AzisabaLauncher.Minecraft
{
    public class MinecraftVersion
    {
        private static readonly HashSet<MinecraftVersion> Instances = new HashSet<MinecraftVersion>();

        public static List<MinecraftVersion> GetInstances()
        {
            return new List<MinecraftVersion>(Instances);
        }

        public static async Task<List<MinecraftVersion>> Fetch()
        {
            Instances.Clear();

            var json = await App.HttpClient.GetStringAsync("https://piston-meta.mojang.com/mc/game/version_manifest_v2.json");

            var document = JsonDocument.Parse(json);
            var versions = document.RootElement.GetProperty("versions");

            var options = new JsonSerializerOptions
            {
                Converters = { new ComplianceLevelConverter(), new VersionTypeConverter() }
            };

            foreach (var version in versions.EnumerateArray())
            {
                var ver = JsonSerializer.Deserialize<MinecraftVersion>(version.GetRawText(), options)!;
                await ver.InitializeAsync();
                Instances.Add(ver);
            }

            return new List<MinecraftVersion>(Instances);
        }

        [JsonPropertyName("id")]
        public string? Name { get; init; }

        [JsonPropertyName("type")]
        public VersionType Type { get; init; }

        [JsonPropertyName("complianceLevel")]
        public ComplianceLevel ComplianceLevel { get; init; }

        [JsonPropertyName("time")]
        public DateTime Time { get; init; }

        [JsonPropertyName("relaseTime")]
        public DateTime ReleaseTime { get; init; }

        [JsonPropertyName("url")]
        public string? DetailsUrl { get; init; }

        public StorageFolder? Directory { get; private set; }

        public JsonElement? Json { get; private set; }

        public MinecraftVersion()
        {
            Instances.Add(this);
        }

        private async Task InitializeAsync()
        {
            this.Directory = await this.TryGetDirectory();
        }

        public async Task<StorageFolder> CreateDirectory()
        {
            if (this.Directory != null)
            {
                return this.Directory;
            }

            return await App.AppDirectory.CreateFolderAsync(this.Name, CreationCollisionOption.FailIfExists);
        }

        private async Task<StorageFolder?> TryGetDirectory()
        {
            try
            {
                return await App.AppDirectory.GetFolderAsync(this.Name);
            }
            catch
            {
                return null;
            }
        }
    }
}
