using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace HawkFlowClient
{
    internal class Endpoints
    {
        public static JObject timedData(String process, String meta, String uid)
        {
            Validation.validateTimedData(process, meta, uid);

            JObject map = new JObject();
            map["process"] = process;
            map["meta"] = meta;
            map["uid"] = uid;
            return map;
        }

        public static JObject metricData(String process, String meta, List<Dictionary<string, float>> items)
        {
            Validation.validateMetricData(process, meta, items);

            JArray jsonArray = new JArray();
            foreach (Dictionary<string, float> map in items)
            {
                JObject vals = new JObject();
                foreach (KeyValuePair<string, float> kvp in map)
                {
                    vals[kvp.Key] = kvp.Value;
                }
                jsonArray.Add(vals);
            }

            JObject jsonObject = new JObject();
            jsonObject["process"] = process;
            jsonObject["meta"] = meta;
            jsonObject["items"] = jsonArray;

            return jsonObject;
        }

        public static JObject exceptionData(String process, String meta, String exceptionText)
        {
            Validation.validateExceptionData(process, meta, exceptionText);

            JObject map = new JObject();
            map["process"] = process;
            map["meta"] = meta;
            map["exception"] = exceptionText;
            return map;
        }
    }
}