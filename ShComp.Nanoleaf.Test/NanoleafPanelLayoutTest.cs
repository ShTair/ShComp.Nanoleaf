using Microsoft.Extensions.Configuration;
using ShComp.Nanoleaf.Fluent;
using System.Threading.Tasks;
using Xunit;

namespace ShComp.Nanoleaf.Test
{
    public class NanoleafPanelLayoutTest
    {
        private readonly IConfiguration _configuration;
        private readonly INanoleaf _nanoleaf;

        public NanoleafPanelLayoutTest()
        {
            _configuration = new ConfigurationBuilder().AddUserSecrets<NanoleafPanelLayoutTest>().Build();
            _nanoleaf = Nanoleaf.Create(_configuration["Host"], _configuration["Token"]);
        }

        [Fact]
        public async Task PanelLayoutGetLayoutTest()
        {
            var panelLayout = await _nanoleaf.PanelLayout.GetLayoutAsync();
            Assert.NotNull(panelLayout);
        }
    }
}