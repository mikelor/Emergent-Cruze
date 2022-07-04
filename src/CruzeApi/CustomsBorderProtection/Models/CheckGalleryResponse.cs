using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CruzeApi.CustomsBorderProtection.Models
{
    public class CheckGalleryResponse
    {
        [JsonPropertyName("GalleryAvailable")]
        public string GalleryAvailable { get; set; }

        [JsonPropertyName("PortCode")]
        public string PortCode { get; set; }

        [JsonPropertyName("FlightNumber")]
        public string FlightNumber { get; set; }

        [JsonPropertyName("CarrierCode")]
        public string CarrierCode { get; set; }
    }
}
