using Newtonsoft.Json;

namespace WebApplication1.Models
{
	public class MailModel
	{
        [JsonProperty("body")]

        public string? Body { get; set; }


        [JsonProperty("subject")]

        public string? Subject { get; set; }

	}
}
