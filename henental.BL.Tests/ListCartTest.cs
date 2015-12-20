using herental.BL;
using herental.BL.Commands;
using herental.BL.Model;
using herental.BL.Services;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using Xunit;

namespace henental.BL.Tests
{
    public class ListCartTest
    {
        [Fact]
        public void Handle()
        {
            var mockProductOrders = new Mock<DbSet<ProductOrderItem>>();
            mockProductOrders.DefaultValue = DefaultValue.Mock;
            mockProductOrders.SetupSequence(m => new List<ProductOrderItem>());
            var mockContext = new Mock<HerentalBL>();
            mockContext.Setup(m => m.OrderedProducts).Returns(mockProductOrders.Object);

            var command = new ListCart(new PriceFormulaManager());
            command.Handle(null);
            Assert.NotNull(command.Result);
            Assert.IsType(typeof(List<ProductOrderItem>), command.Result);
        }
    }
}
