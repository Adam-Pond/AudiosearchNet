using Newtonsoft.Json;
using System.Collections.Generic;

namespace AudiosearchNet.Models
{
 	public class ShowById
	{
		[JsonProperty("id")]
		public int Id { get; set; }
		[JsonProperty("title")]
		public string Title { get; set; }
		[JsonProperty("description")]
		public string Description { get; set; }
		[JsonProperty("network")]
		public object Network { get; set; }
		[JsonProperty("itunes_id")]
		public int Itunes_id { get; set; }
		[JsonProperty("categories")]
		public List<string> Categories { get; set; }
		[JsonProperty("buzz_score")]
		public string Buzz_score { get; set; }
		[JsonProperty("image_files")]
		public List<ImageFile> Image_files { get; set; }
		[JsonProperty("number_of_episodes")]
		public int Number_of_episodes { get; set; }
		[JsonProperty("episode_ids")]
		public List<int> Episode_ids { get; set; }
		[JsonProperty("urls")]
		public Urls Urls { get; set; }
		[JsonProperty("rss_url")]
		public string Rss_url { get; set; }
		[JsonProperty("sc_feed")]
		public object Sc_feed { get; set; }
		[JsonProperty("web_profiles")]
		public object Web_profiles { get; set; }
	}
}
