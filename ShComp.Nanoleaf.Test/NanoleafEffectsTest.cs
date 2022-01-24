using Microsoft.Extensions.Configuration;
using ShComp.Nanoleaf.Fluent;
using ShComp.Nanoleaf.Fluent.AnimationData;
using ShComp.Nanoleaf.Fluent.Effect;
using System;
using System.Linq;
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
            var panelLayout = await _nanoleaf.PanelLayout.GetLayoutAsync();
            var panelIdsWithoutControl = panelLayout.PositionData
                .Where(t => t.ShapeType != 12)
                .Select(t => t.PanelId).ToArray();

            IWithPanelColors withPanelColors = AnimationData.Create();
            for (int i = 0; i < panelIdsWithoutControl.Length; i++)
            {
                var panelId = panelIdsWithoutControl[i];
                withPanelColors = withPanelColors.WithPanelColors(panelId)
                    .WithPanelColor(0, 0, 0, 0, TimeSpan.Zero, TimeSpan.FromSeconds(0.1 * (2 + i)))
                    .WithPanelColor(255, 0, 0, 0, TimeSpan.Zero, TimeSpan.FromSeconds(1))
                    .WithPanelColor(0, 0, 0, 0, TimeSpan.Zero, TimeSpan.FromSeconds(0.1 * (8 - i)));
            }

            var command = EffectCommand.CreateDisplay()
                .WithPalette(0, 37, 100)
                .WithPalette(60, 37, 100)
                .WithPalette(120, 37, 100)
                .WithPalette(180, 37, 100)
                .WithPalette(240, 37, 100)
                .WithPalette(300, 37, 100)
                .WithPalette(0, 0, 0, 16)
                .WithRandomPlugin(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(20))
                .HasOverlay(withPanelColors.ConvertToString());

            await _nanoleaf.Effects.WriteDisplayAsync(command);
        }
    }
}