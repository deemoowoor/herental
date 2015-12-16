using herental.BL;
using herental.BL.Interfaces;
using SimpleInjector;

namespace herental.backend
{
    public class DependencyInjectorSetup
    {
        public static Container Setup()
        {
            var container = new Container();

            //container.Register<ICommand, BL.Commands.Test>(Lifestyle.Singleton);

            Dispatcher.Instance.RegisterHandler("Test", new BL.Commands.Test());

            container.RegisterSingleton<IDispatcher>(Dispatcher.Instance);

            container.Verify();

            return container;
        }
    }
}