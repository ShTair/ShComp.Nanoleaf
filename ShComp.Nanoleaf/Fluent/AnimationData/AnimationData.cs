#pragma warning disable CS8618

using System.Text;

namespace ShComp.Nanoleaf.Fluent.AnimationData;

public class AnimationData : List<PanelColors>, IBlank, IWithPanelColors, IWithPanelColor
{
    public string ConvertToString()
    {
        if (Count == 0) return "";

        var sb = new StringBuilder();
        sb.Append(Count);

        foreach (var pc in this)
        {
            sb.Append($" {pc.PanelId} {pc.Colors.Count}");

            foreach (var pcc in pc.Colors)
            {
                sb.Append($" {pcc.R} {pcc.G} {pcc.B} {pcc.W} {(int)(pcc.T.TotalSeconds * 10)}");
            }
        }

        return sb.ToString();
    }

    public static IBlank Create()
    {
        return new AnimationData();
    }

    #region IWithPanelColors

    IWithPanelColor IWithPanelColors.WithPanelColors(int panelId)
    {
        Add(new PanelColors { PanelId = panelId, Colors = new List<PanelColor>() });
        return this;
    }

    #endregion

    #region IWithPanelColor

    IWithPanelColor IWithPanelColor.WithPanelColor(int r, int g, int b, int w, TimeSpan t)
    {
        this[Count - 1].Colors.Add(new PanelColor(r, g, b, w, t));
        return this;
    }

    IWithPanelColor IWithPanelColor.WithPanelColor(int r, int g, int b, int w, TimeSpan t, TimeSpan span)
    {
        this[Count - 1].Colors.Add(new PanelColor(r, g, b, w, t));
        if (span != TimeSpan.Zero)
        {
            this[Count - 1].Colors.Add(new PanelColor(r, g, b, w, span));
        }
        return this;
    }

    #endregion
}

public class PanelColors
{
    public int PanelId { get; set; }

    public List<PanelColor> Colors { get; set; }
}

public class PanelColor
{
    public int R { get; set; }

    public int G { get; set; }

    public int B { get; set; }

    public int W { get; set; }

    public TimeSpan T { get; set; }

    public PanelColor(int r, int g, int b, int w, TimeSpan t)
    {
        R = r;
        G = g;
        B = b;
        W = w;
        T = t;
    }
}
