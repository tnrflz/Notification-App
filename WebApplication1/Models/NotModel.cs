using Newtonsoft.Json;

namespace WebApplication1.Models
{
    public class NotModel
    {


        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }

        [JsonProperty("isAndroidDevice")]
        public bool isAndroidDevice { get; set; }


        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }


    }
}
