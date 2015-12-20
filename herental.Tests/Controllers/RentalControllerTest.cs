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
    public class RentalControllerTest
    {
        [Fact]
        public void Index()
        {
            var client = new Mock<RpcClient>();
            client.Setup(m => m.Call<List<Product>>("ListProducts", null)).Returns(new List<Product>());

            RentalController controller = new RentalController(client.Object);
            ViewResult result = controller.Index() as ViewResult;
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
        }

        #region test classes for JsonDeserializeResponseMessage()
        [JsonObject()]
        internal class Simple
        {
            [JsonProperty(PropertyName = "Id")]
            public int Id { get; set; }
        }

        [JsonObject()]
        internal class ResponseMessage<TObject>
        {
            [JsonProperty(PropertyName = "Status")]
            public string Status { get; set; }

            [JsonProperty(PropertyName = "Result")]
            public TObject Result { get; set; }
        }

        #endregion

        [Fact]
        public void JsonDeserializeResponseMessage()
        {
            var response = @"{ ""Status"": ""OK"", ""Result"": [{""Id"": 1}] }";
            var result = JsonConvert.DeserializeObject<ResponseMessage<List<Simple>>>(response);
            Assert.NotNull(result.Result);
            Assert.Equal(1, result.Result[0].Id);
        }
    }
}
