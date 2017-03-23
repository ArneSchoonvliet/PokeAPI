using BLL.Extensions.Dictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BLL.Exceptions
{
    /// <summary>
    /// Exception that can be used to notify the user with an error message
    /// Is convertable to a 'ModelState' error
    /// Example: When registring, the user uses a too weak password. This exception is caused by the user!
    /// </summary>
    public class UserActionException : Exception
    {
        public Hashtable Errors { get; }

        public UserActionException(string key, string error, string message = null) : base(message)
        {
            var errors = new [] {error};
            Errors = new Hashtable
            {
                { key, errors }
            };
        }

        public UserActionException(string key, IEnumerable<string> errors, string message = null) : base(message)
        {
            Errors = new Hashtable
            {
                { key, errors }
            };
        }

        public UserActionException(Hashtable errors, string message = null) : base(message)
        {
            Errors = errors;
        }

        public override string Message => $"{base.Message} {Environment.NewLine} ERRORS: {JsonConvert.SerializeObject(Errors)}";

        // TODO Add method to add message to a specific 'key'
    }
}
