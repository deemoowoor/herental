using System.Web.Mvc;
using SimpleInjector;
using SimpleInjector.Integration.Web.Mvc;

namespace herental.App_Start
{
    public class DependencyInjectorSetup
    {
        public static void Setup()
        {
            var container = new Container();

            // TODO: register all interfaces here
            //container.Register<>();

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
    }
}