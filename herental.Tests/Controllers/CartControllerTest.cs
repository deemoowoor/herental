using herental.Controllers;
using herental.Models;
using herental.Services;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Xunit;

namespace herental.Tests.Controllers
{
    public class CartControllerTest
    {
        [Fact]
        public void Index()
        {
            var client = new Mock<RpcClient>();
            client.Setup(m => m.Call<List<ProductOrder>>("ListCart", null)).Returns(new List<ProductOrder>());

            var controller = new CartController(client.Object);
            var result = controller.Index() as ViewResult;
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
        }

        [Fact]
        public void Add()
        {
            var client = new Mock<RpcClient>();
            client.Setup(m => m.Call<Nullable<Boolean>>("AddToCart", null)).Returns(new Nullable<Boolean>(true));

            var controller = new CartController(client.Object);
            var result = controller.Add(1, 1) as RedirectToRouteResult;
            Assert.NotNull(result);
        }

        [Fact]
        public void Update()
        {
            var client = new Mock<RpcClient>();
            client.Setup(m => m.Call<List<ProductOrder>>("UpdateCart", null)).Returns(new List<ProductOrder>());

            var controller = new CartController(client.Object);
            var result = controller.Update(1, 3) as RedirectToRouteResult;
            Assert.NotNull(result);
        }

        [Fact]
        public void Delete()
        {
            var client = new Mock<RpcClient>();
            client.Setup(m => m.Call<List<ProductOrder>>("DeleteFromCart", null)).Returns(new List<ProductOrder>());

            var controller = new CartController(client.Object);
            var result = controller.Delete(1) as RedirectToRouteResult;
            Assert.NotNull(result);
        }
    }
}
