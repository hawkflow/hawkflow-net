namespace HawkFlowAPI;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using System.Text;

public class HawkFlowApi {
    private static String hawkFlowApiUrl = "https://api.hawkflow.ai/v1";
    private static int MAX_RETRIES = 3;
    private static int WAIT_TIME_SECONDS = 1000;

    public static void Metrics(String process, String meta, List<Dictionary<string, float>> items, String apiKey) {
        if(items == null) {
            items = new List<Dictionary<string, float>>();
        }

        try {
            String url = hawkFlowApiUrl + "/metrics";
            JObject data = Endpoints.metricData(process, meta, items);
            Task<String> task = hawkFlowPost(url, data, apiKey);
        } catch(HawkFlowDataTypesException ex) {
            Console.Write(ex.Message);
        }
    }

    public static void Metrics(String process, String meta, List<Dictionary<string, float>> items) {
        Metrics(process, meta, items, "");
    }

    public static void Exception(String process, String meta, String exceptionText, String apiKey) {
        try {
            String url = hawkFlowApiUrl + "/exception";
            JObject data = Endpoints.exceptionData(process, meta, exceptionText);
            Task<String> task = hawkFlowPost(url, data, apiKey);
        } catch(HawkFlowDataTypesException ex) {
            Console.Write(ex.Message);
        }
    }

    public static void Exception(String process, String meta, String exceptionText) {
        Exception(process, meta, exceptionText, "");
    }

    public static void Start(String process, String meta, String uid, String apiKey) {
        try {
            String url = hawkFlowApiUrl + "/timed/start";
            JObject data = Endpoints.exceptionData(process, meta, uid);
            Task<String> task = hawkFlowPost(url, data, apiKey);
        } catch(HawkFlowDataTypesException ex) {
            Console.Write(ex.Message);
        }
    }

    public static void Start(String process, String meta, String uid) {
        Start(process, meta, uid, "");
    }

    public static void End(String process, String meta, String uid, String apiKey) {
        try {
            String url = hawkFlowApiUrl + "/timed/end";
            JObject data = Endpoints.exceptionData(process, meta, uid);
            Task<String> task = hawkFlowPost(url, data, apiKey);
        } catch(HawkFlowDataTypesException ex) {
            Console.Write(ex.Message);
        }
    }

    public static void End(String process, String meta, String uid) {
        End(process, meta, uid, "");
    }

    private static async Task<String> hawkFlowPost(String url, JObject data, String apiKey) {
        try {
            Validation.validateApiKey(apiKey);
        } catch (Exception ex) {
            Console.Write(ex.Message);
        }

        int retries = 0;
        bool success = false;

        try {
            string jsonData = data.ToString(Formatting.Indented);
       
            while (!success && retries < MAX_RETRIES) {
                try {
                    using (var client = new HttpClient()) {
                        client.DefaultRequestHeaders.Add("Content-type", "application/json");
                        client.DefaultRequestHeaders.Add("hawkflow-api-key", apiKey);

                        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                        var response = await client.PostAsync(url, content);
                        success = response.IsSuccessStatusCode;
                        return await response.Content.ReadAsStringAsync();
                    }
                }
                catch (Exception ex) {
                    retries++;
                    Console.WriteLine($"Error: {ex.Message}. Retrying...");
                    System.Threading.Thread.Sleep(WAIT_TIME_SECONDS);
                }
            }
        }
        catch (Exception ex) {
            Console.WriteLine($"HawkFlowAPI Json serialisation error: {ex.Message}.");
        }

        return "";
    }
}
