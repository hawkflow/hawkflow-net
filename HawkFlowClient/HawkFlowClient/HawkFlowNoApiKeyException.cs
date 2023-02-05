using System;

namespace HawkFlowClient
{
    internal class HawkFlowNoApiKeyException : Exception
    {
        private static readonly String docsUrl = "Please see docs at https://docs.hawkflow.ai/integration/index.html";
        private static readonly String message = "No HawkFlow API Key set. " + docsUrl;

        public HawkFlowNoApiKeyException() : base(message) { }
    }
}

