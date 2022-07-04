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
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;

        private readonly string _satEnvironmentAddress = @"https://sat.tvs-cbp.com/";
        private readonly string _alaskaairEnvironmentAddress = @"https://alaskaair.tvs-cbp.com:9001/";

        [HttpGet]
        public async Task<LoginResponse> Login()
        {
            AuthenticationLoginResponse authenticationLoginResponse = await AuthenticateLogin("tvsuser", @"ar+cWHAfmB*B%^s-R#K%yc2c");
            return new LoginResponse()
            {
                RefreshToken = authenticationLoginResponse.RefreshToken,
            };
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


    }
}
