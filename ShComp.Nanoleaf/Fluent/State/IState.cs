namespace ShComp.Nanoleaf.Fluent.State;

public interface IState
{
    Task PutBrightnessAsync(int value, TimeSpan duration);
}
