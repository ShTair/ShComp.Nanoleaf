namespace ShComp.Nanoleaf;

public class PanelLayout
{
    public int numPanels { get; set; }

    public int sideLength { get; set; }
    
    public Positiondata[] positionData { get; set; }
}

public class Positiondata
{
    public int panelId { get; set; }
    
    public int x { get; set; }
    
    public int y { get; set; }
    
    public int o { get; set; }
    
    public int shapeType { get; set; }
}
