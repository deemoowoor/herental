namespace herental.BL.Interfaces
{
    public interface IDispatcher
    {
        void RegisterHandler(string methodName, ICommand del);

        ICommand Dispatch(string methodName);

        object Invoke(string methodName, object[] arguments);
    }
}
