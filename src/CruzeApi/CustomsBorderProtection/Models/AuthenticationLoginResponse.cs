using System;
using System.Text.Json.Serialization;

namespace CruzeApi.CustomsBorderProtection.Models
{
    /// <summary>
    /// A partial representation of an issue object from the GitHub API
    /// </summary>
    public class AuthenticationLoginResponse
    {
        [JsonPropertyName("AccessToken")]
        public string AccessToken { get; set; }

        [JsonPropertyName("ExpiresIn")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("TokenType")]
        public string TokenType { get; set; }

        [JsonPropertyName("RefreshToken")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("IdToken")]
        public string IdToken { get; set; }

        [JsonPropertyName("errorMessage")]
        public string ErrorMessage {get; set; }

    }
}