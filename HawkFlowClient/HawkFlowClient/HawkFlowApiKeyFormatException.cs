using System;

namespace HawkFlowClient
{
    internal class HawkFlowApiKeyFormatException : Exception
    {
        private static readonly String docsUrl = "Please see docs at https://docs.hawkflow.ai/integration/index.html";
        private static readonly String message = "Invalid API Key format. " + docsUrl;

        public HawkFlowApiKeyFormatException() : base(message) { }
    }
}

