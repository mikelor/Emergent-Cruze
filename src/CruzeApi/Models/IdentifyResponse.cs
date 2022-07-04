using System;
using System.Text.Json.Serialization;

namespace CruzeApi.Models
{
    public class IdentifyResponse
    {
        [JsonPropertyName("ExecutionTime")]
        public decimal ExecutionTime { get; set; }

        [JsonPropertyName("ScheduledEncounterPort")]
        public string ScheduledEncounterPort { get; set; }

        [JsonPropertyName("Result")]
        public string Result { get; set; }

        [JsonPropertyName("UID")]
        public string UID { get; set; }
    }
}
