using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CruzeApi.CustomsBorderProtection.Models
{
    public class CheckGalleryRequest
    {
        [JsonPropertyName("CarrierCode")]
        public string CarrierCode { get; set; }

        [JsonPropertyName("FlightNumber")]
        public string FlightNumber { get; set; }

        [JsonPropertyName("ScheduledEncounterPort")]
        public string ScheduledEncounterPort { get; set; }
    }
}
