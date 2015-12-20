using herental.BL;
using herental.BL.Commands;
using herental.BL.Interfaces;
using herental.BL.Services;
using SimpleInjector;

namespace herental.backend
{
    public class DependencyInjectorSetup
    {
        public static Container Setup()
        {
            var container = new Container();
            container.RegisterSingleton<IDispatcher>(new Dispatcher(container)
                {
                    { "Test", typeof(Test) },
                    { "ListProducts", typeof(ListProducts) },
                    { "AddToCart", typeof(AddToCart) },
                    { "ListCart", typeof(ListCart) },
                    { "DeleteFromCart", typeof(DeleteFromCart) },
                    { "UpdateCart", typeof(UpdateCart) }
                }
            );
            
            container.RegisterSingleton<IPriceFormulaManager>(new PriceFormulaManager());

            container.Verify();

            return container;
        }
    }
}