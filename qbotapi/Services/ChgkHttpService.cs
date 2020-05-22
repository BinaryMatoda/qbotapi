using qbotapi.Controllers.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace qbotapi.Services
{
    public class ChgkHttpService : IChgkHttpService
    {
        private readonly IHttpClientFactory _clientFactory;
        public ChgkHttpService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<HttpResponseMessage> OnGet(int type, int complexity, int limit)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"random/answers/types{type}/complexity{complexity}/limit{limit}"
                );

            var client = _clientFactory.CreateClient("chgk");

            var response = await client.SendAsync(request);
            return response;
        }
    }
}
