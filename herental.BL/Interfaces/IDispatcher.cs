using System;

namespace herental.BL.Interfaces
{
    public interface IDispatcher
    {
        void RegisterHandler(string methodName, Type commandClass);

        ICommand Dispatch(string methodName);

        object Invoke(string methodName, object[] arguments);
    }
}
