namespace ShComp.Nanoleaf;

public interface INanoleaf
{
    IPanelLayout PanelLayout { get; }

    IState State { get; }

    IEffectCollection Effects { get; }
}
