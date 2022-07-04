using System;
using System.Text.Json.Serialization;

namespace CruzeApi.CustomsBorderProtection.Models
{
    /// <summary>
    /// A partial representation of an issue object from the GitHub API
    /// </summary>
    public class AuthenticationLoginRequest
    {
        [JsonPropertyName("Username")]
        public string Username { get; set; }

        [JsonPropertyName("Password")]
        public string Password { get; set; }
    }
}