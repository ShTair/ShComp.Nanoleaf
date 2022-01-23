using ShComp.Nanoleaf.Fluent.Effect;
using ShComp.Nanoleaf.Fluent.PanelLayout;
using ShComp.Nanoleaf.Fluent.State;

namespace ShComp.Nanoleaf.Fluent;

public interface INanoleaf
{
    IPanelLayout PanelLayout { get; }

    IState State { get; }

    IEffectCollection Effects { get; }
}
