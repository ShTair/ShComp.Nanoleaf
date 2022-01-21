using System.Text.Json.Serialization;

namespace ShComp.Nanoleaf;

public class RequestCommand : EffectCommand
{
    public RequestCommand(string animName)
    {
        Command = "request";
        AnimName = animName;
    }
}

#pragma warning disable CS8618

public class EffectCommand
{
    public string? Command { get; set; }

    public string? Version { get; set; }

    public string? AnimName { get; set; }

    public string? AnimType { get; set; }

    public string? ColorType { get; set; }

    [JsonPropertyName("palette")]
    public List<Palette>? Palettes { get; set; }

    public string? PluginType { get; set; }

    public string? PluginUuid { get; set; }

    public List<PluginOption>? PluginOptions { get; set; }

    public string? AnimData { get; set; }

    public bool? HasOverlay { get; set; }

    public bool? LogicalPanelsEnabled { get; set; }
}

public class Palette
{
    public int Hue { get; set; }

    public int Saturation { get; set; }

    public int Brightness { get; set; }

    public float Probability { get; set; }

    public override string ToString() => $"H = {Hue}, S = {Saturation}, B = {Brightness}, P = {Probability}";
}

public class PluginOption
{
    public string Name { get; set; }

    public object Value { get; set; }

    public override string ToString() => $"Name = {Name}, Value = {Value}";
}

#pragma warning restore CS8618
