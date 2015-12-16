using herental.BL.Interfaces;
using System.Collections.Generic;

namespace herental.BL
{
    public class Dispatcher : IDispatcher
    {
        private Dictionary<string, ICommand> registry;

        /// <summary>
        /// Register a method handler
        /// </summary>
        /// <param name="methodName">Method name to use for handler lookup</param>
        /// <param name="command">Method handler</param>
        public void RegisterHandler(string methodName, ICommand command)
        {
            registry.Add(methodName, command);
        }

        /// <summary>
        /// Dispatch a method request
        /// </summary>
        /// <param name="methodName">Method name to look up the handler with</param>
        /// <returns></returns>
        public ICommand Dispatch(string methodName)
        {
            return registry[methodName];
        }

        /// <summary>
        /// Dispatch and invoke the method handler
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="arguments">Method arguments</param>
        /// <returns>Handler's return object</returns>
        public object Invoke(string methodName, object[] arguments)
        {
            var command = this.Dispatch(methodName);
            command.Handle(arguments);
            return command.Result;

        }

        #region Singleton interface

        private Dispatcher()
        {
            registry = new Dictionary<string, ICommand>();
        }

        private static Dispatcher instance = new Dispatcher();

        public static Dispatcher Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion
    }
}
