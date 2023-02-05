using System;

namespace HawkFlowClient
{
    internal class HawkFlowMetricsException : Exception
    {
        private static readonly String docsUrl = "Please see docs at https://docs.hawkflow.ai/integration/index.html";
        private static readonly String message = "@HawkflowMetrics missing items parameter. " + docsUrl;

        public HawkFlowMetricsException() : base(message) { }
    }
}