namespace herental.backend
{
    internal class ProtocolResponse
    {
        private object result;
        private string status;

        public object Result { get { return result; } }

        public string Status { get { return status; } }

        public ProtocolResponse(string status, object result)
        {
            this.result = result;
            this.status = status;
        }
    }
}