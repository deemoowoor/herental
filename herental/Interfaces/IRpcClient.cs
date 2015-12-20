using System;

namespace herental.Interfaces
{
    public interface IRpcClient
    {
        TimeSpan Timeout { get; }
        object Call(string methodName, object[] args);
    }
}