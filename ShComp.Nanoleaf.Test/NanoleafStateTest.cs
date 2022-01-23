using Microsoft.Extensions.Configuration;
using ShComp.Nanoleaf.Fluent;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ShComp.Nanoleaf.Test
{
    public class NanoleafStateTest
    {
        private readonly IConfiguration _configuration;
        private readonly INanoleaf _nanoleaf;

        public NanoleafStateTest()
        {
            _configuration = new ConfigurationBuilder().AddUserSecrets<NanoleafStateTest>().Build();
            _nanoleaf = Nanoleaf.Create(_configuration["Host"], _configuration["Token"]);
        }

        [Fact]
        public async Task StatePutBrightnessTest()
        {
            await _nanoleaf.State.PutBrightnessAsync(50, TimeSpan.FromSeconds(1));
        }
    }
}