using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace herental.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "A study project to implement a system with ASP.NET MVC 5, with separate web frontend and backend servers and connected via AMQP (RabbitMQ)";
            ViewBag.Author = "Andrei Sosnin <andrei.sosnin[[at]]gmail.com>";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Author = "Andrei Sosnin";
            ViewBag.Email = "andrei.sosnin@gmail.com";

            return View();
        }
    }
}