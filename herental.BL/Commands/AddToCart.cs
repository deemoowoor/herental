using herental.BL.Interfaces;
using log4net;
using System;

namespace herental.BL.Commands
{
    public class AddToCart : ICommand
    {
        public object Result
        {
            get
            {
                return null;
            }
        }

        private readonly ILog Log = LogManager.GetLogger(typeof(AddToCart));
        private readonly IPriceFormulaManager handler;
        
        public AddToCart(IPriceFormulaManager mgr)
        {
            handler = mgr;
        }

        public void Handle(object[] args)
        {
            using(var db = new HerentalBL())
            {
                var product = db.Products.Find(Convert.ToInt32(args[0]));

                var order = db.OrderedProducts.Create();
                order.Product = product;
                order.Period = Convert.ToInt32(args[1]);
                order.UpdatePriceQuote(db, handler);
                db.OrderedProducts.Add(order);
                
                db.SaveChanges();
            }
        }
    }
}
