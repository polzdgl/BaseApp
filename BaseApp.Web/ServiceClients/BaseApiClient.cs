namespace BaseApp.Web.ServiceClients
{
    public abstract class BaseApiClient
    {
        protected readonly HttpClient HttpClient;

        protected BaseApiClient(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }
    }
}