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
    public class IdentifyController : ControllerBase
    {
        private readonly ILogger<IdentifyController> _logger;

        private readonly string _satEnvironmentAddress = @"https://sat.tvs-cbp.com/";
        private readonly string _alaskaairEnvironmentAddress = @"https://alaskaair.tvs-cbp.com:9001/";

        public IdentifyController(ILogger<IdentifyController> logger)
        {
            _logger = logger;
        }

        [HttpPost("{airlineCode}/{flightNumber}/{departureAirport}/{scheduledDepartureDate}")]
        public async Task<ActionResult<IdentifyResponse>> PostIdentify(string airlineCode, string flightNumber, string departureAirport, string scheduledDepartureDate, [FromBody] IdentifyRequest request)
        {

            AuthenticationLoginResponse loginResponse = await AuthenticateLogin("tvsuser", @"ar+cWHAfmB*B%^s-R#K%yc2c");
            IdentifyAirExitRequest identifyAirExitRequest = new IdentifyAirExitRequest()
            {
                CarrierCode = airlineCode,
                FlightNumber = flightNumber,
                ScheduledEncounterPort = departureAirport,
                ScheduledEncounterDate = scheduledDepartureDate,
                Photo = request.Photo,
                DepartureGate = request.DepartureGate,
                DepartureTerminal = request.DepartureTerminal,
                PhotoDate = request.PhotoDate,
                DeviceId = request.DeviceId,
                Token = request.Token
            };
            IdentifyAirExitResponse identifyAirExitResponse = await IdentifyAirExitAsync(loginResponse, identifyAirExitRequest);

            // Check to see if we received an error
            if (!String.IsNullOrEmpty(identifyAirExitResponse.ErrorMessage))
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = identifyAirExitResponse.ErrorMessage });
            }
            else
            {
                // Right now we're defining the same
                IdentifyResponse response = new IdentifyResponse()
                {
                    ExecutionTime = identifyAirExitResponse.ExecutionTime,
                    ScheduledEncounterPort = identifyAirExitResponse.ScheduledEncounterPort,
                    Result = identifyAirExitResponse.Result,
                    UID = identifyAirExitResponse.UID
                };            
                return new OkObjectResult(response);
            }

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
        private async Task<IdentifyAirExitResponse> IdentifyAirExitAsync(AuthenticationLoginResponse loginResponse, IdentifyAirExitRequest identifyAirExitRequest)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(_alaskaairEnvironmentAddress),
            };

            // Add appropriate headers 
            httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue("AlaskaAir-Cruze-Api")));
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("Authorization", loginResponse.IdToken);

            StringContent content = new StringContent(JsonSerializer.Serialize(identifyAirExitRequest), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("/api/bio/identify/air_exit", content);
            response.EnsureSuccessStatusCode();
            var contentStream = await response.Content.ReadAsStreamAsync();
            // var contentStream = await response.Content.ReadAsStringAsync();
            
            IdentifyAirExitResponse identifyAirExitResponse =
                 await JsonSerializer.DeserializeAsync<IdentifyAirExitResponse>(
                    contentStream,
                    new JsonSerializerOptions
                    {
                        IgnoreNullValues = true,
                        PropertyNameCaseInsensitive = true
                    }
                );

            return identifyAirExitResponse;
        }

    }
}
