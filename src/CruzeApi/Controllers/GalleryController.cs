using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CruzeApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using CruzeApi.CustomsBorderProtection.Models;
using System.Net.Http;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Text;

namespace CruzeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GalleryController : ControllerBase
    {
        private readonly ILogger<GalleryController> _logger;

        private readonly string _satEnvironmentAddress = @"https://sat.tvs-cbp.com/";
        private readonly string _alaskaairEnvironmentAddress = @"https://alaskaair.tvs-cbp.com:9001/";

        public GalleryController(ILogger<GalleryController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{airlineCode}/{flightNumber}/{departureAirport}")]
        public async Task<GalleryResponse> GetGallery(string airlineCode, string flightNumber, string departureAirport)
        {
            AuthenticationLoginResponse loginResponse = await AuthenticateLogin("tvsuser", @"ar+cWHAfmB*B%^s-R#K%yc2c");
            CheckGalleryRequest checkGalleryRequest = new CheckGalleryRequest()
            {
                CarrierCode = airlineCode,
                FlightNumber = flightNumber,
                ScheduledEncounterPort = departureAirport
            };
            CheckGalleryResponse checkGalleryResponse = await CheckGallery(loginResponse, checkGalleryRequest);

            GalleryResponse response = new GalleryResponse()
            {
                IsAvailable = bool.Parse(checkGalleryResponse.GalleryAvailable)
            };

            return response;
        }

        /// <summary>
        /// Calls the CBP Author
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private async Task<AuthenticationLoginResponse> AuthenticateLogin(string username, string password)
        {
            AuthenticationLoginRequest request = new AuthenticationLoginRequest
            {
                Username = username,
                Password = password
            };

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(_alaskaairEnvironmentAddress)
            };

            // Add appropriate headers 
            httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue("AlaskaAir-Cruze-Api")));
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            StringContent content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("/api/auth/login", content);
            response.EnsureSuccessStatusCode();
            var contentStream = await response.Content.ReadAsStreamAsync();

            AuthenticationLoginResponse loginResponse =
                await JsonSerializer.DeserializeAsync<AuthenticationLoginResponse>(
                    contentStream,
                    new JsonSerializerOptions
                    {
                        IgnoreNullValues = true,
                        PropertyNameCaseInsensitive = true
                    }
                );
            return loginResponse;
        }

        /// <summary>
        /// Calls the CBP Author
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private async Task<CheckGalleryResponse> CheckGallery(AuthenticationLoginResponse authenticateResponse, CheckGalleryRequest galleryRequest)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(_alaskaairEnvironmentAddress),
            };

            // Add appropriate headers 
            httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue("AlaskaAir-Cruze-Api")));
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("Authorization", authenticateResponse.IdToken);

            StringContent content = new StringContent(JsonSerializer.Serialize(galleryRequest), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("/api/bio/checkGallery", content);
            response.EnsureSuccessStatusCode();
            //var contentStream = await response.Content.ReadAsStreamAsync();
            var contentStream = await response.Content.ReadAsStringAsync();
            CheckGalleryResponse checkGalleryResponse = new CheckGalleryResponse();
            /*
            CheckGalleryResponse checkGalleryResponse =
                await JsonSerializer.DeserializeAsync<CheckGalleryResponse>(
                    contentStream,
                    new JsonSerializerOptions
                    {
                        IgnoreNullValues = true,
                        PropertyNameCaseInsensitive = true
                    }
                );
            */
            return checkGalleryResponse;
        }
    }
}
