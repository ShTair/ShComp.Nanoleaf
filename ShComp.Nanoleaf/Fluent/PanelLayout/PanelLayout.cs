#pragma warning disable CS8618
namespace ShComp.Nanoleaf;

public class PanelLayout
{
    public int NumPanels { get; set; }

    public int SideLength { get; set; }

    public PositionData[] PositionData { get; set; }
}

public class PositionData
{
    public int PanelId { get; set; }

    public int X { get; set; }

    public int Y { get; set; }

    public int O { get; set; }

    public int ShapeType { get; set; }
}
