using System;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace HawkFlowClient
{
    public class HawkFlowApi
    {
        private static String hawkFlowApiUrl = "https://api.hawkflow.ai/v1";
        public String apiKey = "";
        public int maxRetries = 3;
        public int waitTime = 100;

        public HawkFlowApi(String apiKey, int maxRetries, int waitTime) {
            this.apiKey = apiKey;
            this.maxRetries = maxRetries;
            this.waitTime = waitTime;
        }

        public HawkFlowApi(String apiKey) {
            this.apiKey = apiKey;
        }

        public HawkFlowApi() {
            this.apiKey = "";
        }

        public void Metrics(String process, String meta, List<Dictionary<string, float>> items)
        {
            if (items == null)
            {
                items = new List<Dictionary<string, float>>();
            }

            try
            {
                String url = hawkFlowApiUrl + "/metrics";
                JObject data = Endpoints.metricData(process, meta, items);
                Task<String> task = hawkFlowPost(url, data);
            }
            catch (HawkFlowDataTypesException ex)
            {
                Console.Write(ex.Message);
            }
        }

        public void Exception(String process, String meta, String exceptionText)
        {
            try
            {
                String url = hawkFlowApiUrl + "/exception";
                JObject data = Endpoints.exceptionData(process, meta, exceptionText);
                Task<String> task = hawkFlowPost(url, data);
            }
            catch (HawkFlowDataTypesException ex)
            {
                Console.Write(ex.Message);
            }
        }

        public void Start(String process, String meta, String uid)
        {
            try
            {
                String url = hawkFlowApiUrl + "/timed/start";
                JObject data = Endpoints.exceptionData(process, meta, uid);
                Task<String> task = hawkFlowPost(url, data);
            }
            catch (HawkFlowDataTypesException ex)
            {
                Console.Write(ex.Message);
            }
        }

        public void End(String process, String meta, String uid)
        {
            try
            {
                String url = hawkFlowApiUrl + "/timed/end";
                JObject data = Endpoints.exceptionData(process, meta, uid);
                Task<String> task = hawkFlowPost(url, data);
            }
            catch (HawkFlowDataTypesException ex)
            {
                Console.Write(ex.Message);
            }
        }

        private async Task<String> hawkFlowPost(String url, JObject data)
        {
            try
            {
                Validation.validateApiKey(this.apiKey);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            int retries = 0;
            bool success = false;

            try
            {
                string jsonData = data.ToString(Formatting.Indented);

                while (!success && retries < this.maxRetries)
                {
                    try
                    {
                        using (var client = new HttpClient())
                        {
                            client.DefaultRequestHeaders.Add("Content-type", "application/json");
                            client.DefaultRequestHeaders.Add("hawkflow-api-key", apiKey);

                            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                            var response = await client.PostAsync(url, content);
                            success = response.IsSuccessStatusCode;
                            return await response.Content.ReadAsStringAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        retries++;
                        Console.WriteLine($"Error: {ex.Message}. Retrying...");
                        System.Threading.Thread.Sleep(this.waitTime);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HawkFlowAPI Json serialisation error: {ex.Message}.");
            }

            return "";
        }
    }
}