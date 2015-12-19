namespace herental.BL.Interfaces
{
    /// <summary>
    /// Command pattern
    /// </summary>
    public interface ICommand
    {
        object Result { get; }

        void Handle(object[] args);
    }

}
