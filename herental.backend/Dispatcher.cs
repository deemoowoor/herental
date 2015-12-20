using herental.BL.Interfaces;
using System;
using System.Collections.Generic;
using SimpleInjector;

namespace herental.BL
{
    /// <summary>
    /// Dispatches commands by name
    /// </summary>
    public class Dispatcher : Dictionary<string, Type>, IDispatcher
    {
        private readonly Container container;

        public Dispatcher(Container container)
        {
            this.container = container;
        }
        
        /// <summary>
        /// Register a method handler
        /// </summary>
        /// <param name="commandName">Name of command to register</param>
        /// <param name="command">Command type</param>
        public void RegisterHandler(string commandName, Type command)
        {
            if (command.GetInterface(typeof(ICommand).Name) != null)
            {
                Add(commandName, command);
            }
            else
            {
                throw new InvalidOperationException("Invalid command class. Must have ICommand interface!");
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
            try {
                return (ICommand)container.GetInstance(this[commandName]);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException("Herental backend: Unknown method name");
            }
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

    }
}
