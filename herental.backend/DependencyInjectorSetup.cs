using herental.BL;
using herental.BL.Interfaces;
using SimpleInjector;

namespace herental.App_Start
{
    public class DependencyInjectorSetup
    {
        public static void Setup()
        {
            var container = new Container();

            container.RegisterSingleton<IDispatcher>(Dispatcher.Instance);

            container.Verify();
        }
    }
}