using herental.BL;
using herental.BL.Commands;
using herental.BL.Model;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using Xunit;

namespace henental.BL.Tests
{
    public class ListProductsTest
    {
        [Fact]
        public void TestListProducts()
        {
            var mockProducts = new Mock<DbSet<Product>>();
            mockProducts.DefaultValue = DefaultValue.Mock;
            mockProducts.SetupSequence(m => new List<Product>());
            var mockContext = new Mock<HerentalBL>();
            mockContext.Setup(m => m.Products).Returns(mockProducts.Object);

            var command = new ListProducts();

            command.Handle(null);
            Assert.NotNull(command.Result);
        }
    }
}
