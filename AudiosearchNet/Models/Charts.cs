using Newtonsoft.Json;
using System.Collections.Generic;

namespace AudiosearchNet.Models
{
	/// <summary>
	/// Return result from Endpoint: /chart_daily?limit=10&country=us
	/// </summary>
	public class Charts
	{
		[JsonProperty("country")]
		public string Country { get; set; }
		[JsonProperty("limit")]
		public string Limit { get; set; }
		[JsonProperty("start_date")]
		public string Start_date { get; set; }
		[JsonProperty("shows")]
		public Dictionary<int, string> Shows { get; set; }
	}
}
