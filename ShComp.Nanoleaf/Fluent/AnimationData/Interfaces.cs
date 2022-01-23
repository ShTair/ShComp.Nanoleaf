namespace ShComp.Nanoleaf.Fluent.AnimationData;

public interface IBlank : IWithPanelColors { }

public interface IWithPanelColors
{
    IWithPanelColor WithPanelColors(int panelId);
}

public interface IWithPanelColor : IWithPanelColors
{
    IWithPanelColor WithPanelColor(int r, int g, int b, int w, TimeSpan t);

    string ConvertToString();
}
