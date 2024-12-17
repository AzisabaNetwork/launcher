using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AzisabaLauncher.Minecraft
{
    public enum ComplianceLevel
    {
        Level0,
        Level1
    }

    public class ComplianceLevelConverter : JsonConverter<ComplianceLevel>
    {
        public override ComplianceLevel Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetInt32() switch
            {
                0 => ComplianceLevel.Level0,
                1 => ComplianceLevel.Level1,
                _ => throw new JsonException($"Unsupported compliance level ${reader.GetString()}")
            };
        }

        public override void Write(Utf8JsonWriter writer, ComplianceLevel value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value switch
            {
                ComplianceLevel.Level0 => 1,
                ComplianceLevel.Level1 => 1,
                _ => throw new JsonException($"Unsupported compliance level {value}")
            });
        }
    }
}
