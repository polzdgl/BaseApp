using BaseApp.ServiceProvider.Interfaces;
using System.Net.Http;

namespace BaseApp.Web.ServiceClients
{
    public partial class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
    }
}
