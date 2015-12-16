using herental.BL.Commands;
using System;
using System.Collections.Generic;

namespace herental.BL.Interfaces
{
    public class ListProducts : ICommand
    {
        private object result;

        public object Result
        {
            get
            {
                return result;
            }
        }

        public void Handle(object[] args)
        {
            // This would get the list of products and store it in result
            throw new NotImplementedException();
        }
    }
}
