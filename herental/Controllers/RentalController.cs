﻿using herental.Interfaces;
using herental.Models;
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
            var products = _client.Call<List<ProductViewModel>>("ListProducts", null);
            return View(products);
        }
    }
}