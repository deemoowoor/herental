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
            // Add product refernce to cart along with quantity
            using(var db = new HerentalBL())
            {
                // Using a single cart
                Cart cart = db.Carts.Create();
                cart.Id = 1;
                db.Carts.Attach(cart);

                Product product = new Product() { Id = (int)args[0] };
                db.Products.Attach(product);

                ProductOrder order = db.ProductOrders.Create();
                order.Product = product;
                order.Quantity = (int)args[1];

                cart.ProductOrders.Add(order);
            }
        }
    }
}
