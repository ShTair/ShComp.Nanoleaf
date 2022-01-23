using System.Text.Json.Serialization;

namespace ShComp.Nanoleaf.Fluent.Effect;

#pragma warning disable CS8618

public class EffectCommand : IWithPalette, IWithPalettes, IWithAnimType, IHasOverlay
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


    public static EffectCommand CreateRequest(string animName) => new EffectCommand
    {
        Command = "request",
        AnimName = animName,
    };

    public static IWithPalette CreateDisplay() => new EffectCommand
    {
        Version = "2.0",
        Command = "display",
        ColorType = "HSB",
        LogicalPanelsEnabled = true,
    };

    #region IWithPalette

    IWithAnimType IWithPalette.WithPalette(int hue, int saturation, int brightness, int count, double probability)
    {
        if (count < 1) throw new ArgumentOutOfRangeException(nameof(count));
        Palettes = Enumerable.Range(0, count).Select(t => new Palette(hue, saturation, brightness, probability)).ToList();
        return this;
    }

    #endregion

    #region IWithPalettes

    IWithAnimType IWithPalettes.WithPalette(int hue, int saturation, int brightness, int count, double probability)
    {
        if (count < 1) throw new ArgumentOutOfRangeException(nameof(count));
        Palettes!.AddRange(Enumerable.Range(0, count).Select(t => new Palette(hue, saturation, brightness, probability)));
        return this;
    }

    #endregion

    #region IWithAnimType

    IHasOverlay IWithAnimType.WithWheelPlugin(string linDirection, bool loop, int nColorsPerFrame, TimeSpan transTime)
    {
        AnimType = "plugin";
        PluginType = "color";
        PluginUuid = "6970681a-20b5-4c5e-8813-bdaebc4ee4fa";
        PluginOptions = new List<PluginOption>
        {
            new("linDirection", linDirection),
            new("loop", loop),
            new("nColorsPerFrame", nColorsPerFrame),
            new("transTime", transTime.TotalSeconds),
        };

        return this;
    }

    IHasOverlay IWithAnimType.WithRandomPlugin(TimeSpan delayTime, TimeSpan transTime)
    {
        AnimType = "plugin";
        PluginType = "color";
        PluginUuid = "ba632d3e-9c2b-4413-a965-510c839b3f71";
        PluginOptions = new List<PluginOption>
        {
            new("delayTime", delayTime.TotalSeconds),
            new("transTime", transTime.TotalSeconds),
        };

        return this;
    }

    #endregion

    #region IHasOverlay

    EffectCommand IHasOverlay.HasOverlay(string animData)
    {
        AnimData = animData;
        HasOverlay = true;
        return this;
    }

    EffectCommand IHasOverlay.HasNotOverlay()
    {
        HasOverlay = false;
        return this;
    }

    #endregion
}

public interface IWithPalette
{
    IWithAnimType WithPalette(int hue, int saturation, int brightness, int count = 1, double probability = 0);
}

public interface IWithPalettes
{
    IWithAnimType WithPalette(int hue, int saturation, int brightness, int count = 1, double probability = 0);
}

public interface IWithAnimType : IWithPalettes
{
    IHasOverlay WithWheelPlugin(string linDirection, bool loop, int nColorsPerFrame, TimeSpan transTime);

    IHasOverlay WithRandomPlugin(TimeSpan delayTime, TimeSpan transTime);
}

public interface IHasOverlay
{
    EffectCommand HasOverlay(string animData);

    EffectCommand HasNotOverlay();
}

public class Animation
{
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

    public double Probability { get; set; }

    public Palette(int hue, int saturation, int brightness, double probability)
    {
        Hue = hue;
        Saturation = saturation;
        Brightness = brightness;
        Probability = probability;
    }

    public override string ToString() => $"H = {Hue}, S = {Saturation}, B = {Brightness}, P = {Probability}";
}

public class PluginOption
{
    public string Name { get; set; }

    public object Value { get; set; }

    public PluginOption(string name, object value)
    {
        Name = name;
        Value = value;
    }

    public override string ToString() => $"Name = {Name}, Value = {Value}";
}

#pragma warning restore CS8618
