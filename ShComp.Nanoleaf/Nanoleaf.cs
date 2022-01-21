using System.Net.Http.Json;

namespace ShComp.Nanoleaf;

public sealed class Nanoleaf : IDisposable, INanoleaf, IEffectCollection
{
    private readonly HttpClient _client;

    private readonly Uri _baseUri;

    public bool IDisposed { get; private set; }

    private Nanoleaf(string host, int port, string authToken)
    {
        _client = new HttpClient();
        _baseUri = new Uri($"http://{host}:{port}/api/v1/{authToken}");
    }

    public static INanoleaf Create(string host, string authToken, int port = 16021)
    {
        return new Nanoleaf(host, port, authToken);
    }

    public void Dispose()
    {
        if (IDisposed) return;
        IDisposed = true;

        _client.Dispose();
    }

    #region INanoleaf

    IEffectCollection INanoleaf.Effects => this;

    #endregion

    #region IEffectCollection

    async Task<IReadOnlyList<string>> IEffectCollection.ListAsync()
    {
        var uri = _baseUri + "/effects/effectsList";
        var result = await _client.GetFromJsonAsync<string[]>(uri);
        return result ?? Array.Empty<string>();
    }

    async Task<string> IEffectCollection.GetSelectAsync()
    {
        var uri = _baseUri + "/effects/select";
        var result = await _client.GetStringAsync(uri);
        return result;
    }

    async Task IEffectCollection.PutSelectAsync(string effectName)
    {
        var uri = _baseUri + "/effects";
        var response = await _client.PutAsJsonAsync(uri, new { select = effectName });
        if (!response.IsSuccessStatusCode) throw new ArgumentException();
    }

    #endregion
}
