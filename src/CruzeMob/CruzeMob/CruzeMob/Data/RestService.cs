using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Xamarin.Forms;

using CruzeMob.Models;

namespace CruzeMob.Data
{
    public class RestService : IRestService
    {
        HttpClient client;

        public RestService()
        {
#if DEBUG
            client = new HttpClient(DependencyService.Get<IHttpClientHandlerService>().GetInsecureHandler());
#else
            client = new HttpClient();
#endif
        }

        public async Task<LoginResponse> LoginAsync()
        {
            LoginResponse loginResponse = new LoginResponse();

            Uri uri = new Uri(string.Format(Constants.RestUrl, @"/api/login"));
            try
            {

                var response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                var contentStream = await response.Content.ReadAsStreamAsync();

                loginResponse = await JsonSerializer.DeserializeAsync<LoginResponse>(
                    contentStream,
                    new JsonSerializerOptions
                    {
                        IgnoreNullValues = true,
                        PropertyNameCaseInsensitive = true
                    }
                );
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return loginResponse;
        }

        public async Task<IdentifyResponse> IdentifyAsync(IdentifyRequest request)
        {
            IdentifyResponse identifyResponse = new IdentifyResponse();

            Uri uri = new Uri(string.Format(Constants.RestUrl, string.Format(@"/api/identify/{0}/{1}/{2}/{3}", request.CarrierCode, request.FlightNumber, request.ScheduledEncounterPort, request.ScheduledEncounterDate)));
            try
            {
                // Add appropriate headers 
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue("AlaskaAir-Cruze-Api")));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                StringContent content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

                var response = await client.PostAsync(uri, content);
                response.EnsureSuccessStatusCode();
                var contentStream = await response.Content.ReadAsStreamAsync();

                identifyResponse = await JsonSerializer.DeserializeAsync<IdentifyResponse>(
                    contentStream,
                    new JsonSerializerOptions
                    {
                        IgnoreNullValues = true,
                        PropertyNameCaseInsensitive = true
                    }
                );
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return identifyResponse;
        }
    }
}