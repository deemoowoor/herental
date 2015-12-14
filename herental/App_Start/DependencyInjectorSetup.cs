using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimpleInjector;

namespace herental.App_Start
{
    public class DependencyInjectorSetup
    {
        public static void Setup()
        {
            var container = new Container();
        }
    }
}