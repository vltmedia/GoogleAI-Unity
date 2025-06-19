using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GenerativeAI
{
    [System.Serializable]
    public class GApi
    {
        public static string ApiKey;
        public  string Model;
        public GenerationConfiguration generationConfiguration;
        private static readonly HttpClient httpClient = new HttpClient();
        public Action<string> OnLog;
        public Action<string> OnResponseReceived;
        public Action<string> OnErrorReceived;
        public static void SetStaticApiKey(string apiKey)
        {
            ApiKey = apiKey;
        }
        public  void SetApiKey(string apiKey)
        {
            ApiKey = apiKey;
        }
        public  void SetModel(string model)
        {
            Model = model;
            if (generationConfiguration == null)
            {
                generationConfiguration = new GenerationConfiguration();
            }
        }

        public async virtual Task<string> SendRequest(Uri baseUrl, string relativeUrl, string requestBody)
        {
            httpClient.BaseAddress = baseUrl;

            try
            {
                var response = await httpClient.PostAsync(relativeUrl, new StringContent(requestBody, Encoding.UTF8, "application/json"));
                if (!response.IsSuccessStatusCode)
                {
                    OnErrorReceived?.Invoke("Response not successful");
                    return "";
                }
                var responseText = await response.Content.ReadAsStringAsync();

                OnResponseReceived?.Invoke(responseText);
                return responseText;
            }
            catch (Exception e)
            {
                OnErrorReceived?.Invoke(e.Message);
                return "";
            }

            }
        public async virtual Task<string> SendRequest(string url, string requestBody)
        {

            try
            {
                var response = await httpClient.PostAsync(url, new StringContent(requestBody, Encoding.UTF8, "application/json"));
                if (!response.IsSuccessStatusCode)
                {
                    OnErrorReceived?.Invoke("Response not successful");
                    return "";
                }
                var responseText = await response.Content.ReadAsStringAsync();

                OnResponseReceived?.Invoke(responseText);
                return responseText;
            }
            catch (Exception e)
            {
                OnErrorReceived?.Invoke(e.Message);
                return "";
            }

            }
    }
}
