using Newtonsoft.Json;
using System.Collections.Generic;

namespace AudiosearchNet.Models
{
	public class Episode
	{
		[JsonProperty("item_id")]
		public int Id { get; set; }

		[JsonProperty("file_name")]
		public string Name { get; set; }

		[JsonProperty("item_name")]
		public string ItemName { get; set; }

		[JsonProperty("file_status")]
		public string Status { get; set; }
	}

	/// <summary>
	/// EpisodeById
	/// from Endpoint /episode/{id}
	/// https://www.audiosear.ch/swagger#!/episodes/get_episodes_id
	/// </summary>
	public class EpisodeById
	{
		[JsonProperty("id")]
		public int Id { get; set; }
		[JsonProperty("title")]
		public string Title { get; set; }
		[JsonProperty("description")]
		public string Description { get; set; }
		[JsonProperty("date_created")]
		public string Date_created { get; set; }
		[JsonProperty("identifier")]
		public string Identifier { get; set; }
		[JsonProperty("digital_location")]
		public string Digital_location { get; set; }
		[JsonProperty("physical_location")]
		public string Physical_location { get; set; }
		[JsonProperty("duration")]
		public int Duration { get; set; }
		[JsonProperty("tags")]
		public List<object> Tags { get; set; }
		[JsonProperty("updated_at")]
		public string Updated_at { get; set; }
		[JsonProperty("itunes_episode")]
		public string Itunes_episode { get; set; }
		[JsonProperty("buzz_score")]
		public double? Buzz_score { get; set; }
		[JsonProperty("date_added")]
		public string Date_added { get; set; }
		[JsonProperty("show_id")]
		public int Show_id { get; set; }
		[JsonProperty("show_title")]
		public string Show_title { get; set; }
		[JsonProperty("audio_files")]
		public List<AudioFile> Audio_files { get; set; }
		[JsonProperty("image_files")]
		public List<ImageFile> Image_files { get; set; }
		[JsonProperty("rss_url")]
		public string Rss_url { get; set; }
		[JsonProperty("extra")]
		public Extra Extra { get; set; }
		[JsonProperty("urls")]
		public Urls Urls { get; set; }
		[JsonProperty("categories")]
		public List<Category> Categories { get; set; }
		[JsonProperty("highlights")]
		public Highlights Highlights { get; set; }
		[JsonProperty("entities")]
		public List<object> Entities { get; set; }
	}

	/// <summary>
	/// from Endpoint /episode/{id}
	/// https://www.audiosear.ch/swagger#!/episodes/get_episodes_id
	/// </summary>
	public class AudioFile
	{
		[JsonProperty("id")]
		public int Id { get; set; }
		[JsonProperty("duration")]
		public int? Duration { get; set; }
		[JsonProperty("url")]
		public string Url { get; set; }
	}

	/// <summary>
	/// from Endpoint /episode/{id}
	/// https://www.audiosear.ch/swagger#!/episodes/get_episodes_id
	/// </summary>
	public class Extra
	{
		[JsonProperty("itunes_episode")]
		public string Itunes_episode { get; set; }
		[JsonProperty("skip_transcript")]
		public string Skip_transcript { get; set; }
	}

	/// <summary>
	/// from Endpoint /episode/{id}
	/// https://www.audiosear.ch/swagger#!/episodes/get_episodes_id
	/// </summary>
	public class Highlights
	{
		// TODO: This is what was generated from episode 12909
	}
}
