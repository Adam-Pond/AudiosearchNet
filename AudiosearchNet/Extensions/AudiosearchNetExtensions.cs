﻿using AudiosearchNet.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;

namespace AudiosearchNet
{
    /// <summary>
    /// Extends AudiosearchNetClient class adding some util methods.
    /// </summary>
    public static class AudiosearchNetExtensions
    {
        /// <summary>
        /// Creates a basic signature for Authorization headers.
        /// </summary>
        /// <returns>A basic signature for Authorization headers.</returns>
        public static string CreateSignature(this AudiosearchNetClient client)
        {
            return string.Format("Basic {0}",
                    string.Concat(client.ApplicationId, ":", client.ApplicationSecret).ToBase64String()
                );
        }

        /// <summary>
        /// Creates an AccessToken OAuth object.
        /// </summary>
        /// <param name="path">The Audiosear.ch endpoint authorization path.</param>
        /// <returns>An AccessToken object</returns>
        public static AccessToken Authorize(this AudiosearchNetClient client, string path)
        {
            try
            {
                var parameters = new Dictionary<string, string> { { "grant_type", "client_credentials" } };
                var request = (HttpWebRequest)WebRequest.Create(string.Concat(Config.HOST_BASE_URL, path));

                request.Headers.Add(
                        HttpRequestHeader.Authorization, 
                        client.CreateSignature()
                    );

                return JsonConvert.DeserializeObject<AccessToken>(
                        request.Post(parameters)
                    );
            }
            catch (Exception ex)
            {
                throw new OperationCanceledException("Failure in authorization process", ex);
            }
        }

        //        httpClient.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", accessToken));
        //        httpClient.DefaultRequestHeaders.Add("User-Agent", "audiosearch-client-dotNet/1.0.0");
    }
}
