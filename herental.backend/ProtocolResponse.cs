namespace herental.backend
{
    internal class ProtocolResponse
    {
        private object result;
        private string status;
        private string message;

        public object Result { get { return result; } }

        public string Status { get { return status; } }

        public string Message { get { return message; } }

        public ProtocolResponse(string status, object result, string message = null)
        {
            this.result = result;
            this.status = status;
            this.message = message;
        }
    }
}