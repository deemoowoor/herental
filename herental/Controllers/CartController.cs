using herental.Interfaces;
using herental.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace herental.Controllers
{
    public class CartController : Controller
    {
        private IRpcClient _client;

        public CartController(IRpcClient client)
        {
            _client = client;
        }

        // GET: Cart
        public ActionResult Index()
        {
            var productOrders = _client.Call<List<ProductOrder>>("ListCart", null);
            return View(productOrders);
        }

        public ActionResult Add(int id, int period)
        {
            var result = _client.Call<Nullable<Boolean>>("AddToCart", new object[2] { id, period });
            return RedirectToAction("Index", result);
        }

        public ActionResult Update(int id, int period)
        {
            var result = _client.Call<Nullable<Boolean>>("UpdateCart", new object[2] { id, period });
            return RedirectToAction("Index", result);
        }

        public ActionResult Delete(int orderedProductId)
        {
            var result = _client.Call<Nullable<Boolean>>("DeleteFromCart", new object[1] { orderedProductId });
            return RedirectToAction("Index", result);
        }
    }
}