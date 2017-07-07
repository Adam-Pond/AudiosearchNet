using AudiosearchNet.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace AudiosearchNet
{
	/// <summary>
	/// A client to handles API calls.
	/// </summary>
	public class AudiosearchNetClient
	{
		/// <summary>
		/// Audiosear.ch OAuth Access Token.
		/// </summary>
		public AccessToken AccessToken { get; private set; }

		/// <summary>
		/// Audiosear.ch Application Id.
		/// </summary>
		public string ApplicationId { get; private set; }   // TODO: Shouldn't this be a SecureString?

		/// <summary>
		/// Audiosear.ch Application Secret.
		/// </summary>
		public string ApplicationSecret { get; private set; }   // TODO: Shouldn't this be a SecureString.... and private get?

		/// <summary>
		/// Creates an instance of a client to handles API calls.
		/// </summary>
		/// <param name="applicationId">Audiosear.ch Application Id.</param>
		/// <param name="applicationSecret">Audiosear.ch Application Secret.</param>
		public AudiosearchNetClient(string applicationId, string applicationSecret)
		{
			ApplicationId = applicationId;
			ApplicationSecret = applicationSecret;
			AccessToken = this.Authorize(Config.AUTHORIZATION_ENDPOINT);
		}

		#region Shows

		/// <summary>
		/// Returns search results for Shows.
		/// </summary>
		/// <param name="keyWords">Keyworkds to search shows.</param>
		/// <returns>Results for Shows.</returns>
		public AudiosearchNetApiResult<Show> GetShowsByKeyWords(string keyWords)
		{
			string endpoint = string.Concat(Endpoint.SEARCH_SHOW_BY_QUERY, keyWords);
			var response = GetApiResponse(endpoint);

			return JsonConvert.DeserializeObject<AudiosearchNetApiResult<Show>>(response);
		}

		/// <summary>
		/// Returns search results for Shows.
		/// </summary>
		/// <param name="query">Query to search shows.</param>
		/// <returns>Results for Shows.</returns>
		public AudiosearchNetApiResult<Show> GetShowsByQuery(Query query)
		{
			string endpoint = string.Concat(Endpoint.SEARCH_SHOW_BY_QUERY, query.ToString());
			var response = GetApiResponse(endpoint);
			return JsonConvert.DeserializeObject<AudiosearchNetApiResult<Show>>(response);
		}

		public dynamic GetDynShowById(int id)
		{
			string endpoint = string.Concat(Endpoint.SHOW_BY_ID, id);
			var response = GetApiResponse(endpoint);

			dynamic result = JsonConvert.DeserializeObject<dynamic>(response);

			return result;
		}

		public ShowById GetShowById(int id)
		{
			string endpoint = string.Concat(Endpoint.SHOW_BY_ID, id);
			var response = GetApiResponse(endpoint);

			var result = JsonConvert.DeserializeObject<ShowById>(response);

			return result;
		}

		public dynamic GetDynTrending()
		{
			string endpoint = string.Concat(Endpoint.TRENDING_SHOW);
			var response = GetApiResponse(endpoint);

			var result = JsonConvert.DeserializeObject<dynamic>(response);

			return result;
		}

		public Charts GetTrending()
		{
			string endpoint = string.Concat(Endpoint.TRENDING_SHOW);
			var response = GetApiResponse(endpoint);

			var result = JsonConvert.DeserializeObject<dynamic>(response);

			var charts = new Charts();
			charts.Country = result.country;
			charts.Limit = result.limit;
			charts.Start_date = result.start_date;
			charts.Shows = new Dictionary<int, string>();

			foreach (var obj in result.shows)
			{
				int id;
				string title;

				GetTitleAndIdFromMalformedJson(obj.ToString(), out id, out title);
				charts.Shows.Add(id, title);
			}

			return charts;
		}

		#endregion

		#region Categories

		public dynamic GetDynCategories()
		{
			string response = GetApiResponse(Endpoint.CATEGORIES);
			dynamic results = JsonConvert.DeserializeObject<dynamic>(response);

			return results;
		}

		public List<Category> GetCategoriesList()
		{
			string response = GetApiResponse(Endpoint.CATEGORIES);
			var results = JsonConvert.DeserializeObject<List<Category>>(response);

			return results;
		}

		public Dictionary<int, AudiosearchNet.Models.Category> GetCategories()
		{
			var categories = GetCategoriesList();
			var dictionary = new Dictionary<int, Category>();
			foreach (var category in categories)
			{
				dictionary.Add(category.Id, category);
			}

			return dictionary;
		}

		/// <summary>
		/// Returns a single episode given an episode id
		/// </summary>
		/// <param name="id">Episode Id</param>
		/// <returns>Details for a single episode</returns>
		public AudiosearchNet.Models.EpisodeById GetEpisodeById(int id)
		{
			string endpoint = string.Concat(Endpoint.EPISODE_BY_ID, id);
			string response = GetApiResponse(endpoint);
			var result = JsonConvert.DeserializeObject<EpisodeById>(response);
			if (result.Audio_files.Count == 0 || String.IsNullOrEmpty(result.Audio_files[0].Url))
				return null;    // result.Image_files == null || result.Image_files.Count == 0 ||

			return result;
		}

		/// <summary>
		/// Return a collection of episodes given a list of episode Ids
		/// </summary>
		/// <param name="episodeIds">A list of episode ids</param>
		/// <returns>A collection of episodes</returns>
		public Dictionary<int, AudiosearchNet.Models.EpisodeById> GetEpisodesById(List<int> episodeIds)
		{
			var episodes = new Dictionary<int, AudiosearchNet.Models.EpisodeById>();

			foreach (var id in episodeIds)
			{
				var episode = GetEpisodeById(id);
				if (episode != null)
					episodes.Add(episode.Id, episode);
			}
			return episodes;
		}

		/// <summary>
		/// Returns a collection of trending shows
		/// </summary>
		/// <returns>A collection of trending shows</returns>
		public List<AudiosearchNet.Models.ShowById> GetTrendingShows()
		{
			var trendingShows = GetTrending();
			return GetTrendingShows(trendingShows);
		}

		/// <summary>
		/// Returns a collection of Trending shows given the results of the chart call containing title and id of the trending shows.
		/// </summary>
		/// <param name="trendingShows"></param>
		/// <returns>A collection of trending shows</returns>
		private List<AudiosearchNet.Models.ShowById> GetTrendingShows(Charts trendingShows)
		{
			var shows = new List<AudiosearchNet.Models.ShowById>();

			foreach (var trendingShow in trendingShows.Shows)
			{
				shows.Add(GetShowById(trendingShow.Key));
			}

			return shows;
		}

		/// <summary>
		/// Returns a cached response based on the given ID.
		/// Cache is in %TEMP%/AudioSearch
		/// </summary>
		/// <returns>Returns null if the cache file does not exist, else returns the contents of the response in the file identified by the id</returns>
		private string GetApiResponse(string endpoint)
		{
			string response = ReadResponse(endpoint);
			if (string.IsNullOrEmpty(response))
			{
				response = this.GetJsonResponse(endpoint);
				WriteResponse(endpoint, response);
			}

			return response;
		}

		#endregion

		#region Helper functions

		/// <summary>
		/// While dealing with charts, the returned json is malformed. This pulls out the important information about the title and the ID of the show.
		/// </summary>
		/// <param name="input">The malformed json</param>
		/// <param name="id">the returned id.</param>
		/// <param name="title">the title of the show.</param>
		private void GetTitleAndIdFromMalformedJson(string input, out int id, out string title)
		{
			string pattern = @"""(?<Title>.+)"": {.*\r\n.*id"": (?<Id>\d+),";
			Regex regex = new Regex(pattern);
			Match match = regex.Match(input);


			title = match.Groups["Title"].Value;
			string ids = match.Groups["Id"].Value;
			id = int.Parse(ids);
		}

		#endregion

		#region Caching results of a response to file

		/// <summary>
		/// Write a JSON response to a temp file so it can be interrogated later
		/// </summary>
		/// <returns></returns>
		private bool WriteResponse(string endpoint, string response)
		{
			string resultsFile = TempFilenameFromEndpoint(endpoint);

			if (!System.IO.File.Exists(resultsFile))
				System.IO.File.WriteAllText(resultsFile, response);

			return true;
		}

		private string ReadResponse(string endpoint)
		{
			string resultsFile = TempFilenameFromEndpoint(endpoint);
			if (System.IO.File.Exists(resultsFile))
			{
				var response = System.IO.File.ReadAllText(resultsFile);
				return response;
			}

			return null;
		}

		private string TempFilenameFromEndpoint(string endpoint)
		{
			var tempPath = Directory.CreateDirectory(Path.GetTempPath() + @"\Audiosear.ch\");
			var path = tempPath.FullName + "AudioSearchLogShow_" + endpoint.Replace('/', '_').Replace('\\', '_').Replace('?', '.') + ".json";

			return path;
		}

		#endregion
	}
}
