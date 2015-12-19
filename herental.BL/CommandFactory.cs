using herental.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace herental.BL
{
    public class CommandFactory
    {
        public static ICommand Create(Type commandType)
        {
            //return new commandType;
            return (ICommand)Activator.CreateInstance(commandType);
        }
    }
}
