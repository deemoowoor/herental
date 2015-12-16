namespace herental.BL.Interfaces
{
    /// <summary>
    /// Command pattern
    /// </summary>
    /// <typeparam name="TResult">Operation result type</typeparam>
    /// <typeparam name="TArgument">Argument type</typeparam>
    public interface ICommand
    {
        object Result { get; }

        void Handle(object[] args);
    }

}
