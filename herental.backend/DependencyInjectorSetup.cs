using herental.BL;
using herental.BL.Commands;
using herental.BL.Interfaces;
using SimpleInjector;

namespace herental.backend
{
    public class DependencyInjectorSetup
    {
        public static Container Setup()
        {
            Dispatcher.Instance.RegisterHandler("Test", typeof(Test));
            Dispatcher.Instance.RegisterHandler("ListProducts", typeof(ListProducts));

            var container = new Container();
            container.RegisterSingleton<IDispatcher>(Dispatcher.Instance);

            container.Verify();

            return container;
        }
    }
}