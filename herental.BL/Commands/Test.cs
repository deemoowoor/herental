using herental.BL.Interfaces;

namespace herental.BL.Commands
{
    public class Test : ICommand
    {
        public object Result
        {
            get { return "ACK"; }
        }

        public void Handle(object[] args)
        {
            // NIL
        }
    }
}
