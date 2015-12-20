using herental.BL.Interfaces;
using log4net;
using System;

namespace herental.BL.Commands
{
    public class UpdateCart : ICommand
    {
        public object Result
        {
            get
            {
                return null;
            }
        }

        private readonly ILog Log = LogManager.GetLogger(typeof(AddToCart));

        public void Handle(object[] args)
        {
            using (var db = new HerentalBL())
            {
                var orderedProduct = db.OrderedProducts.Find(Convert.ToInt32(args[0]));
                orderedProduct.Period = Convert.ToInt32(args[1]);
                db.SaveChanges();
            }
        }
    }
}
