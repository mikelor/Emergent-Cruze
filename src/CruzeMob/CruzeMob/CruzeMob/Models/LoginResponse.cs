using System;
using System.Text.Json.Serialization;

namespace CruzeMob.Models
{
    /// <summary>
    /// A partial representation of an issue object from the GitHub API
    /// </summary>
    public class LoginResponse
    {
        [JsonPropertyName("RefreshToken")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("errorMessage")]
        public string ErrorMessage {get; set; }

    }
}