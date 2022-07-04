using System;
using System.Text.Json.Serialization;

namespace CruzeApi.CustomsBorderProtection.Models
{
    public class IdentifyAirExitResponse
    {
        [JsonPropertyName("ExecutionTime")]
        public decimal ExecutionTime { get; set; }

        [JsonPropertyName("ScheduledEncounterPort")]
        public string ScheduledEncounterPort { get; set; }

        [JsonPropertyName("Result")]
        public string Result { get; set; }

        [JsonPropertyName("UID")]
        public string UID { get; set; }

        [JsonPropertyName("errorMessage")]
        public string ErrorMessage { get; set; }
    }
}
