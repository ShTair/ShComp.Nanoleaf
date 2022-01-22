using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ShComp.Nanoleaf.Test
{
    public class NanoleafEffectsTest
    {
        private readonly IConfiguration _configuration;
        private readonly INanoleaf _nanoleaf;

        public NanoleafEffectsTest()
        {
            _configuration = new ConfigurationBuilder().AddUserSecrets<NanoleafEffectsTest>().Build();
            _nanoleaf = Nanoleaf.Create(_configuration["Host"], _configuration["Token"]);
        }

        [Fact]
        public async Task EffectsListTest()
        {
            var effectNames = await _nanoleaf.Effects.ListAsync();
            Assert.NotEmpty(effectNames);
        }

        [Fact]
        public async Task EffectsGetSelectTest()
        {
            var effectName = await _nanoleaf.Effects.GetSelectAsync();
            Assert.False(string.IsNullOrWhiteSpace(effectName));
        }

        [Fact]
        public async Task EffectsPutSelectTest()
        {
            var effectName = _configuration["EffectName"];
            await _nanoleaf.Effects.PutSelectAsync(effectName);
        }

        [Fact]
        public async Task EffectsPutSelectErrorTest()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _nanoleaf.Effects.PutSelectAsync("ErrorName"));
        }

        [Fact]
        public async Task EffectsWriteRequestTest()
        {
            var effectName = _configuration["EffectName"];
            var command = await _nanoleaf.Effects.WriteRequestAsync(effectName);
            Assert.Equal(effectName, command.AnimName);
        }

        [Fact]
        public async Task EffectsWriteDisplayCommandTest()
        {
            var command = EffectCommand.CreateDisplay()
                .WithPalette(0, 37, 100)
                .WithPalette(60, 37, 100)
                .WithPalette(120, 37, 100)
                .WithPalette(180, 37, 100)
                .WithPalette(240, 37, 100)
                .WithPalette(300, 37, 100)
                .WithPalette(0, 0, 0, 16)
                .WithRandomPlugin(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(20))
                .HasOverlay(AnimationData.Create()
                    .WithPanelColors(841)
                        .WithPanelColor(255, 0, 0, 0, TimeSpan.FromSeconds(1))
                        .WithPanelColor(127, 0, 0, 0, TimeSpan.FromSeconds(1))
                    .WithPanelColors(62283)
                        .WithPanelColor(0, 255, 0, 0, TimeSpan.FromSeconds(0.5))
                        .WithPanelColor(0, 127, 0, 0, TimeSpan.FromSeconds(0.5))
                        .WithPanelColor(0, 255, 0, 0, TimeSpan.FromSeconds(0.5))
                        .WithPanelColor(0, 127, 0, 0, TimeSpan.FromSeconds(0.5))
                    .ConvertToString());

            await _nanoleaf.Effects.WriteDisplayAsync(command);
        }
    }
}