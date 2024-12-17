using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AzisabaLauncher.Minecraft
{
    public class Version
    {
        private static readonly HashSet<Version> instances = new HashSet<Version>();

        public static async Task<List<Version>> Fetch()
        {
            using HttpClient client = new HttpClient();

            var json = await client.GetStringAsync("https://piston-meta.mojang.com/mc/game/version_manifest_v2.json");

            var document = JsonDocument.Parse(json);
            var versions = document.RootElement.GetProperty("versions");

            var options = new JsonSerializerOptions
            {
                Converters = { new ComplianceLevelConverter(), new VersionTypeConverter() }
            };

            foreach (var version in versions.EnumerateArray())
            {
                instances.Add(JsonSerializer.Deserialize<Version>(version.GetRawText(), options)!);
            }

            return new List<Version>(instances);
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

        public Version()
        {
            instances.Add(this);
        }
    }
}
