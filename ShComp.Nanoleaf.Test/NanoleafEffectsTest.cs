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
                .WithPalette(60, 37, 100, 1)
                .WithPalette(0, 0, 0, 10)
                .Attach()
                .WithRandomPlugin(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(20))
                .HasNotOverlay();

            await _nanoleaf.Effects.WriteDisplayAsync(command);
        }
    }
}