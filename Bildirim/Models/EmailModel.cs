using Newtonsoft.Json;

namespace Bildirim.Models
{
	public class EmailModel
	{


		[JsonProperty("receiver")]
		public bool Receiver { get; set; }

		[JsonProperty("subject")]
		public string Subject { get; set; }

		[JsonProperty("body")]
		public string Body { get; set; }

	}
}
