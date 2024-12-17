using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AzisabaLauncher.Minecraft
{
    public enum VersionType
    {
        Release,
        Snapshot,
        OldBeta,
        OldAlpha,
        Unknown
    }

    public class VersionTypeConverter : JsonConverter<VersionType>
    {
        public override VersionType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetString() switch
            {
                "release" => VersionType.Release,
                "snapshot" => VersionType.Snapshot,
                "old_beta" => VersionType.OldBeta,
                "old_alpha" => VersionType.OldAlpha,
                _ => throw new JsonException($"Unsupported version type ${reader.GetString()}")
            };
        }

        public override void Write(Utf8JsonWriter writer, VersionType value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value switch
            {
                VersionType.Release => "release",
                VersionType.Snapshot => "snapshot",
                VersionType.OldBeta => "old_beta",
                VersionType.OldAlpha => "old_alpha",
                _ => throw new JsonException($"Unsupported version type {value}")
            });
        }
    }
}
