using herental.BL.Interfaces;
using herental.BL.Model;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace herental.BL.Commands
{
    public class ListCart : ICommand
    {
        private IEnumerable<ProductOrderItem> list;
        private IPriceFormulaManager handler;

        public object Result
        {
            get
            {
                return list;
            }
        }

        public ListCart(IPriceFormulaManager mgr)
        {
            handler = mgr;
        }

        public void Handle(object[] args)
        {
            using (var db = new HerentalBL())
            {
                list = db.OrderedProducts.Where(item => item.Deleted == false)
                    .Include(item => item.Product)
                    .Include(item => item.Product.Type).ToList();
            }
        }
    }
}
