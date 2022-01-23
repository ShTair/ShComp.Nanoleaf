namespace ShComp.Nanoleaf;

public interface IPanelLayout
{
    Task<PanelLayout> GetLayoutAsync();
}
