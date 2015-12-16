using herental.BL.Interfaces;

namespace herental.BL.Commands
{
    internal class TestResult : ICommandResult
    {
        private string v;

        public TestResult(string v)
        {
            this.v = v;
        }

        public override string ToString()
        {
            return v;
        }
    }
}