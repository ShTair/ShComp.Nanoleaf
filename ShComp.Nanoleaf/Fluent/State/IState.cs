namespace ShComp.Nanoleaf;

public interface IState
{
    Task PutBrightnessAsync(int value, TimeSpan duration);
}
