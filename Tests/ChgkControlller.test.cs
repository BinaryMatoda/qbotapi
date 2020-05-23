using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using qbotapi.Controllers;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using qbotapi.Resources;
using System.Collections.Generic;

namespace Tests
{
    public class ChgkControllerTest : IClassFixture<WebApplicationFactory<qbotapi.Startup>>
    {
        private readonly WebApplicationFactory<qbotapi.Startup> _factory;
        private readonly ITestOutputHelper _output;

        public ChgkControllerTest(WebApplicationFactory<qbotapi.Startup> factory, ITestOutputHelper output)
        {
            _output = output;
            _factory = factory;
        }

        [Theory]
        [InlineData("/weatherforecast")]
        [InlineData("/api/chgk/type/1/complexity/1/limit/4")]
        public async Task ControllerCanCreate_returnsController(string url)
        {
            // Act
            var client = _factory.CreateClient();

            // Arrange
            var res = await client.GetAsync(url);
            var contentString = await res.Content.ReadAsStringAsync();

            _output.WriteLine(contentString);
            var obj = JsonConvert.DeserializeObject<List<QuestionResource>>(contentString);
            _output.WriteLine(obj.Count.ToString());
            foreach (var item in obj)
            {
                _output.WriteLine(item.question);
            }
            // Assert
            res.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.NotEmpty(contentString);
            Assert.NotEmpty(obj);
        }
    }
}
