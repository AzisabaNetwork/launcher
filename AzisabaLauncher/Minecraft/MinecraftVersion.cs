using AzisabaLauncher.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Windows.Storage;

namespace AzisabaLauncher.Minecraft
{
    public class MinecraftVersion
    {
        private static readonly HashSet<MinecraftVersion> Instances = new HashSet<MinecraftVersion>();

        public static MinecraftVersion? GetInstance(string name)
        {
            return Instances.FirstOrDefault(instance => instance.Name == name);
        }

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

        public JsonElement? Json { get; private set; }

        public MinecraftVersion()
        {
            Instances.Add(this);
        }

        public async Task<StorageFolder> GetStorageFolder()
        {
            StorageFolder folder;

            try
            {
                folder = await App.AppDirectory.GetFolderAsync(this.Name);
            }
            catch (FileNotFoundException)
            {
                folder = await App.AppDirectory.CreateFolderAsync(this.Name);
            }

            return folder;
        }

        public async Task<JsonElement> GetJsonElement()
        {
            StorageFolder folder = await this.GetStorageFolder();

            if (! await folder.Contains($"{this.Name}.json"))
            {
                await this.DownloadJsonFile();
            }

            StorageFile file = await folder.GetFileAsync($"{this.Name}.json");

            string json;

            using (var stream = await file.OpenStreamForReadAsync())
            using (var reader = new StreamReader(stream))
            {
                json = await reader.ReadToEndAsync();
            }

            using (JsonDocument document = JsonDocument.Parse(json))
            {
                return document.RootElement.Clone();
            }
        }

        public async Task<StorageFile> DownloadJarFile()
        {
            JsonElement json = await this.GetJsonElement();
            string url = json.GetProperty("downloads").GetProperty("client").GetProperty("url").GetString()!;

            byte[] bytes = await App.HttpClient.GetByteArrayAsync(url);

            StorageFile file = await (await this.GetStorageFolder()).CreateFileAsync($"{this.Name}.jar", CreationCollisionOption.ReplaceExisting);

            await FileIO.WriteBytesAsync(file, bytes);

            return file;
        }

        public async Task<StorageFile> DownloadJsonFile()
        {
            string json = await App.HttpClient.GetStringAsync(this.DetailsUrl);

            StorageFolder folder = await this.GetStorageFolder();
            StorageFile file = await folder.CreateFileAsync($"{this.Name}.json", CreationCollisionOption.ReplaceExisting);

            await FileIO.WriteTextAsync(file, json);

            return file;
        }
    }
}
