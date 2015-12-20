using herental.Interfaces;
using herental.Models;
using Newtonsoft.Json.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

namespace herental.Controllers
{
    public class RentalController : Controller
    {
        private IRpcClient _client;

        public RentalController(IRpcClient client)
        {
            _client = client;
        }

        // GET: Rental
        public ActionResult Index()
        {
            // TODO: move parsing to a separate class
            var products = (JArray)_client.Call("ListProducts", null);
            List<ProductViewModel> vProducts = new List<ProductViewModel>();

            foreach (var product in products)
            {
                vProducts.Add(new ProductViewModel()
                {
                    Id = (int)product["Id"],
                    Name = (string)product["Name"],
                    ProductTypeName = (string)product["Type"]["Name"],
                    ProductTypeId = (int)product["Type"]["Id"]
                });
            }
            
            return View(vProducts);
        }
    }
}