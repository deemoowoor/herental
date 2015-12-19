using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ServiceProcess;
using Xunit;

namespace herental.backend.Tests
{
    public class BackendServiceTest
    {
        private static TimeSpan timeout = TimeSpan.FromMilliseconds(5000);
        private static ServiceController service = new ServiceController("herental.backend");

        public static void ClassInitialize()
        {
            // XXX: service must be installed prior to running this test!
            ServiceController service = new ServiceController("herental.backend");
            
            if (ServiceControllerStatus.Stopped == service.Status)
            {
                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }

            Assert.Equal(ServiceControllerStatus.Running, service.Status);
        }

        public static void ClassCleanup()
        {
            if (ServiceControllerStatus.Running == service.Status)
            {
                service.Stop();
            }

            service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
            Assert.Equal(ServiceControllerStatus.Stopped, service.Status);
        }

        [Fact]
        public void TestRPCFunction()
        {
            ClassInitialize();
            
            var rpcClient = new BasicRabbitMqClient(timeout);
            var result = rpcClient.Call("Test", new object[1]);
            rpcClient.Close();
            Assert.Equal("ACK", result);
            
        }

        [Fact]
        public void TestListProducts()
        {
            ClassInitialize();

            var rpcClient = new BasicRabbitMqClient(timeout);
            var result = rpcClient.Call("ListProducts", new object[1]);
            rpcClient.Close();
            Assert.NotEmpty((JArray)result);
        }
    }
}