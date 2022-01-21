using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ShComp.Nanoleaf.Test
{
    public class NanoleafTest
    {
        private readonly IConfiguration _configuration;
        private readonly INanoleaf _nanoleaf;

        public NanoleafTest()
        {
            _configuration = new ConfigurationBuilder().AddUserSecrets<NanoleafTest>().Build();
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
        public async Task EffectsWriteCommandTest()
        {
            var effectName = _configuration["EffectName"];
            var command = await _nanoleaf.Effects.WriteCommandAsync(new RequestCommand(effectName));
            Assert.Equal(effectName, command.AnimName);
        }
    }
}