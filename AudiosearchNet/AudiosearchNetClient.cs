using AudiosearchNet.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.IO;

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
		public string ApplicationId { get; private set; }

		/// <summary>
		/// Audiosear.ch Application Secret.
		/// </summary>
		public string ApplicationSecret { get; private set; }

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

		public dynamic GetShowById_Dynamic(int id)
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
		
		#endregion

		#region Categories

		public dynamic GetCategoriesDynamic()
		{
			string response = GetApiResponse(Endpoint.CATEGORIES);
			dynamic results = JsonConvert.DeserializeObject<dynamic>(response);

			return results;
		}

		public List<Category> GetCategories()
		{
			string response = GetApiResponse(Endpoint.CATEGORIES);
			var results = JsonConvert.DeserializeObject<List<Category>>(response);

			return results;
		}

		public List<AudiosearchNet.Models.EpisodeById> GetEpisodes(List<int> episodeIds)
		{
			var episodes = new List<AudiosearchNet.Models.EpisodeById>();

			foreach (var id in episodeIds)
			{
				string endpoint = string.Concat(Endpoint.EPISODES, id);
				string response = GetApiResponse(endpoint);
				episodes.Add(JsonConvert.DeserializeObject<EpisodeById>(response));
			}
			return episodes;
		}

		/// <summary>
		/// Returns a cached response based on the given ID.
		/// Cache is in %TEMP%/AudioSearch
		/// </summary>
		/// <returns>Returns null if the cache file does not exist, else returns the contents of the response in the file identified by the id</returns>
		private string GetApiResponse(string endpoint)
		{
			string response = ReadResponse(EndpointToFileNamePostfix(endpoint));
			if (string.IsNullOrEmpty(response))
			{
				response = this.GetJsonResponse(endpoint);
				WriteResponse(endpoint, response);
			}

			return response;
		}

		#endregion

		#region Caching results of a response to file

		/// <summary>
		/// Write a JSON response to a temp file so it can be interrogated later
		/// </summary>
		/// <returns></returns>
		private bool WriteResponse(string endpoint, string response)
		{
			DirectoryInfo directoryInfo = GetTempCacheFileName();
			string resultsFile = directoryInfo.FullName + "AudioSearchLogShow_" + EndpointToFileNamePostfix(endpoint) + ".json";

			WriteToFile(response, resultsFile);

			return true;
		}

		private string ReadResponse(string endpoint)
		{
			DirectoryInfo directoryInfo = GetTempCacheFileName();
			string resultsFile = directoryInfo.FullName + "AudioSearchLogShow_" + EndpointToFileNamePostfix(endpoint) + ".json";
			if (System.IO.File.Exists(resultsFile))
			{
				var response = System.IO.File.ReadAllText(resultsFile);
				return response;
			}

			return null;
		}

		private static void WriteToFile(string response, string filename)
		{
			if (System.IO.File.Exists(filename))
			{
				System.IO.File.Delete(filename);
			}

			System.IO.File.WriteAllText(filename, response);
		}

		private string EndpointToFileNamePostfix(string endpoint)
		{
			return endpoint.Replace('/', '_').Replace('\\', '_');
		}

		private static DirectoryInfo GetTempCacheFileName()
		{
			return Directory.CreateDirectory(Path.GetTempPath() + @"\Audiosear.ch\");
			// Note: This is just temporary until the results are stored in the DB
		}

		#endregion
	}
}
