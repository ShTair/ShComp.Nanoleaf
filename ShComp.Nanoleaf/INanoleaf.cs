namespace ShComp.Nanoleaf;

public interface INanoleaf
{
    IState State { get; }

    IEffectCollection Effects { get; }
}
