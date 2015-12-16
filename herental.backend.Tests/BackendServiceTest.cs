using Newtonsoft.Json;
using System;
using System.ServiceProcess;
using Xunit;

namespace herental.backend.Tests
{
    public class BackendServiceTest
    {
        private static TimeSpan timeout = TimeSpan.FromMilliseconds(3000);
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
            
            try {
                var rpcClient = new BasicRabbitMqClient(timeout);
                var result = rpcClient.Call(JsonConvert.SerializeObject(new { MethodName = "Test", Arguments = new object[1] }));
                rpcClient.Close();
                Assert.Equal("ACK", JsonConvert.DeserializeObject(result));
            }
            finally
            {
                ClassCleanup();
            }
        }

    }
}