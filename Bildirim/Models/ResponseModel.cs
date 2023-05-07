using Newtonsoft.Json;


namespace Bildirim.Models
{
    public class ResponseModel
    {

        [JsonProperty ("isSucces")]
        public bool isSucces  { get; set; }

        [JsonProperty ("message")]
        public string Message { get; set; }

    }
}
