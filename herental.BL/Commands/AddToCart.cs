using herental.BL.Interfaces;
using herental.BL.Model;

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

        public void Handle(object[] args)
        {
            // TODO: Validate arguments
            //ProductOrder order = new ProductOrder(args[0]);
            // Add product refernce to cart along with quantity
            using(var db = new HerentalBL())
            {
                // Using a single cart
                Cart cart = db.Carts.Create();
                cart.Id = 1;
                db.Carts.Attach(cart);
            }
        }
    }
}
