using System.Net.Http;

namespace CruzeMob
{
    public interface IHttpClientHandlerService
    {
        HttpClientHandler GetInsecureHandler();
    }
}