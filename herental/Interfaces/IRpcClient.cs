using System;

namespace herental.Interfaces
{
    public interface IRpcClient
    {
        TimeSpan Timeout { get; }

        TObject Call<TObject>(string methodName, object[] args);
    }
}