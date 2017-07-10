using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AudiosearchNet.Models
{
    public class Show
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
		[Display(Name = "Title")]
		public string Title { get; set; }

        [JsonProperty("network")]
        public Network Network { get; set; }

        [JsonProperty("categories")]
		[Display(Name = "Categories")]
		public List<Category> Categories { get; set; }

        [JsonProperty("description")]
		[Display(Name = "Description")]
		public string Description { get; set; }

        [JsonProperty("ui_url")]
		public string AudiosearchUrl { get; set; }

        [JsonProperty("rss_url")]
        public string RssUrl { get; set; }

        [JsonProperty("buzz_score")]
		[Display(Name = "Buzz Score")]
		public string Score { get; set; }

        [JsonProperty("image_files")]
		[Display(Name = "Image")]
		public List<Image> Images { get; set; }

        [JsonProperty("sc_feed")]
        public string Feed { get; set; }

        [JsonProperty("web_profiles")]
        public List<object> Profiles { get; set; }

        [JsonProperty("episode_ids")]
		[Display(Name = "Episodes")]
		public List<int> EpisodeIds { get; set; }

        [JsonProperty("urls")]
        public object Urls { get; set; }

        [JsonProperty("recent_episodes")]
        public List<Episode> RecentEpisodes { get; set; }
    }
}
