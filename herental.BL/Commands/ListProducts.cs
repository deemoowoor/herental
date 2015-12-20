using herental.BL.Interfaces;
using System.Linq;

namespace herental.BL.Commands
{
    public class ListProducts : ICommand
    {
        private object result;

        public object Result
        {
            get
            {
                return result;
            }
        }

        public void Handle(object[] args)
        {
            using (var db = new HerentalBL())
            {
                // TODO: implement query filters from args
                result = db.Products.ToList();
            }
        }
    }
}
