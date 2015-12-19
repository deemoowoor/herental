namespace herental.backend
{
    internal class ProtocolResponse
    {
        private object result;

        public object Result { get { return result; } }

        public ProtocolResponse(object result)
        {
            this.result = result;
        }
    }
}