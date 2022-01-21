namespace ShComp.Nanoleaf;

public interface IEffectCollection
{
    Task<IReadOnlyList<string>> ListAsync();

    Task<string> GetSelectAsync();

    Task PutSelectAsync(string effectName);
}
