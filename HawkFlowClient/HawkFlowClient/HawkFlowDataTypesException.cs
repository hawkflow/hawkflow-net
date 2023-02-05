using System;

namespace HawkFlowClient
{

    internal class HawkFlowDataTypesException : Exception
    {
        private static readonly String docsUrl = "Please see docs at https://docs.hawkflow.ai/integration/index.html";
        private static readonly String message = "HawkFlow data types not set correctly. " + docsUrl;

        public HawkFlowDataTypesException(String errorMessage) : base(errorMessage + " " + message) { }
    }

}