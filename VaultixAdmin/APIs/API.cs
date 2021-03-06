﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace VaultixAdmin.ServerAPIs
{
    public class API
    {
        static HttpClient client = new HttpClient();

        public static void Run()
        {
            string uri = "http://vaultixserver.azurewebsites.net/";
#if DEBUG
            uri = "http://localhost:16554/";
#endif
            client.BaseAddress = new Uri(uri);
            client.DefaultRequestHeaders.Accept.Clear();
        }
        public static async Task<Uri> HttpTrainNeuralNetwork_MethodAsync(string classifier, string base64String)
        {
            if (!String.IsNullOrEmpty(classifier) && !String.IsNullOrEmpty(base64String))
            {
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var content = new StringContent($"\"{base64String}\"", Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync($"api/images/HttpTrainImageNetworkAsync/{classifier}", content);
                response.EnsureSuccessStatusCode();

                // Return the URI of the created resource.
                return response.Headers.Location;
            }
            return null;
        }
    }
}
