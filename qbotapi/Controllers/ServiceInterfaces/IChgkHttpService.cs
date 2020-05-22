using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace qbotapi.Controllers.ServiceInterfaces
{
    public interface IChgkHttpService
    {
        Task<HttpResponseMessage> OnGet(int type, int complexity, int limit);
    }
}
