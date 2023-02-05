using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HawkFlowClient
{
    internal class Validation
    {
        private static readonly Regex pattern = new Regex("^[a-zA-Z0-9_-]*$");

        public static void validateApiKey(String apiKey)
        {
            if (apiKey == null)
                apiKey = "";

            if (Environment.GetEnvironmentVariable("HAWKFLOW_API_KEY") == null)
                throw new HawkFlowNoApiKeyException();

            if (apiKey == "")
            {
                apiKey = Environment.GetEnvironmentVariable("HAWKFLOW_API_KEY");
            }

            if (apiKey.Length > 50)
            {
                throw new HawkFlowApiKeyFormatException();
            }

            if (!pattern.IsMatch(apiKey))
            {
                throw new HawkFlowApiKeyFormatException();
            }            
        }

        public static void validateTimedData(String process, String meta, String uid)
        {
            validateCore(process, meta);
            validateUid(uid);
        }

        public static void validateMetricData(String process, String meta, List<Dictionary<string, float>> items)
        {
            validateCore(process, meta);
            validateMetricItems(items);
        }

        public static void validateExceptionData(String process, String meta, String exceptionText)
        {
            validateCore(process, meta);
            validateExceptionText(exceptionText);
        }

        public static void validateCore(String process, String meta)
        {
            validateProcess(process);
            validateMeta(meta);
        }

        public static void validateProcess(String process)
        {
            if (process.Length > 249)
            {
                throw new HawkFlowDataTypesException("HawkFlow API process parameter exceeded max length of 249.");
            }

            if (!pattern.IsMatch(process))
            {
                throw new HawkFlowDataTypesException("HawkFlow API process parameter incorrect format.");
            }
        }

        public static void validateMeta(String meta)
        {
            if (meta.Length > 499)
            {
                throw new HawkFlowDataTypesException("HawkFlow API meta parameter exceeded max length of 300.");
            }

            if (!pattern.IsMatch(meta))
            {
                throw new HawkFlowDataTypesException("HawkFlow API meta parameter incorrect format.");
            }
        }

        public static void validateUid(String uid)
        {
            if (uid.Length > 50)
            {
                throw new HawkFlowDataTypesException("HawkFlow API uid parameter exceeded max length of 50.");
            }

            if (!pattern.IsMatch(uid))
            {
                throw new HawkFlowDataTypesException("HawkFlow API uid parameter incorrect format.");
            }
        }

        public static void validateExceptionText(String exceptionText)
        {
            if (exceptionText.Length > 15000)
            {
                throw new HawkFlowDataTypesException("HawkFlow API exceptionText parameter exceeded max length of 15000.");
            }
        }

        public static void validateMetricItems(List<Dictionary<string, float>> items)
        {
            foreach (Dictionary<string, float> map in items)
            {
                foreach (KeyValuePair<string, float> kvp in map)
                {
                    String name = kvp.Key;

                    if (name != "name")
                    {
                        throw new HawkFlowDataTypesException("HawkFlow API metric items parameter HashMap key must be called 'name'.");
                    }

                    if (name.Length > 50)
                    {
                        throw new HawkFlowDataTypesException("HawkFlow API metric items parameter name exceeded max length of 50.");
                    }

                    if (!pattern.IsMatch(name))
                    {
                        throw new HawkFlowDataTypesException("HawkFlow API metric items parameter name is in incorrect format.");
                    }
                }
            }
        }
    }
}