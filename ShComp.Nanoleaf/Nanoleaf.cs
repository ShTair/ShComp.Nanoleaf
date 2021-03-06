using ShComp.Nanoleaf.Fluent;
using ShComp.Nanoleaf.Fluent.Effect;
using ShComp.Nanoleaf.Fluent.PanelLayout;
using ShComp.Nanoleaf.Fluent.State;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ShComp.Nanoleaf;

public sealed class Nanoleaf : IDisposable, INanoleaf, IPanelLayout, IState, IEffectCollection
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    private readonly Uri _baseUri;

    public bool IDisposed { get; private set; }

    private Nanoleaf(string host, int port, string authToken)
    {
        _client = new HttpClient();
        _jsonSerializerOptions = new(JsonSerializerDefaults.Web)
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

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

    IPanelLayout INanoleaf.PanelLayout => this;

    IState INanoleaf.State => this;

    IEffectCollection INanoleaf.Effects => this;

    #endregion

    #region IPanelLayout

    async Task<PanelLayout> IPanelLayout.GetLayoutAsync()
    {
        var uri = _baseUri + "/panelLayout/layout";
        var result = await _client.GetFromJsonAsync<PanelLayout>(uri);
        if (result is null) throw new Exception();
        return result;
    }

    #endregion

    #region IState

    async Task IState.PutBrightnessAsync(int value, TimeSpan duration)
    {
        var uri = _baseUri + "/state";
        var response = await _client.PutAsJsonAsync(uri, new { brightness = new { value, duration = (int)duration.TotalSeconds } });
        if (!response.IsSuccessStatusCode) throw new ArgumentException("invalid value");
    }

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
        if (!response.IsSuccessStatusCode) throw new ArgumentException("invalid effect name", nameof(effectName));
    }

    private Task<HttpResponseMessage> WriteCommandAsync(EffectCommand command)
    {
        var uri = _baseUri + "/effects";
        return _client.PutAsJsonAsync(uri, new { write = command }, _jsonSerializerOptions);
    }

    async Task<Animation> IEffectCollection.WriteRequestAsync(string animName)
    {
        var command = EffectCommand.CreateRequest(animName);
        var response = await WriteCommandAsync(command);
        if (!response.IsSuccessStatusCode) throw new ArgumentException("invalid command", nameof(command));
        return (await response.Content.ReadFromJsonAsync<Animation>())!;
    }

    async Task IEffectCollection.WriteDisplayAsync(EffectCommand command)
    {
        var response = await WriteCommandAsync(command);
        if (!response.IsSuccessStatusCode) throw new ArgumentException("invalid command", nameof(command));
    }

    #endregion
}
