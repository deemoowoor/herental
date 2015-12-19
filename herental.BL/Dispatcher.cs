using herental.BL.Interfaces;
using System;
using System.Collections.Generic;

namespace herental.BL
{
    /// <summary>
    /// Dispatches commands by name, creating concrete instances of ICommand interface
    /// </summary>
    public class Dispatcher : IDispatcher
    {
        private Dictionary<string, Type> registry;

        /// <summary>
        /// Register a method handler
        /// </summary>
        /// <param name="commandName">Name of command to register</param>
        /// <param name="command">Command type</param>
        public void RegisterHandler(string commandName, Type command)
        {
            if (command.GetInterface("ICommand") != null)
            {
                registry.Add(commandName, command);
            }
        }

        /// <summary>
        /// Dispatch a method request
        /// </summary>
        /// <param name="commandName">Method name to look up the handler with</param>
        /// <returns>Command if found</returns>
        /// <exception cref="KeyNotFoundException">Raised if command is not found</exception>
        public ICommand Dispatch(string commandName)
        {
            return (ICommand)Activator.CreateInstance(registry[commandName]);
        }

        /// <summary>
        /// Dispatch and invoke the method handler
        /// </summary>
        /// <param name="commmandName">Name of command to invoke</param>
        /// <param name="arguments">Method arguments</param>
        /// <returns>Handler's return object</returns>
        public object Invoke(string commmandName, object[] arguments)
        {
            var command = Dispatch(commmandName);
            command.Handle(arguments);
            return command.Result;
        }

        #region Singleton interface

        private Dispatcher()
        {
            registry = new Dictionary<string, Type>();
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
