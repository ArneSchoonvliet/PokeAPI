using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Shared.Test.Helpers
{
    public sealed class ExpectedUserActionException : ExpectedExceptionBaseAttribute
    {
        private readonly string _expectedExceptionMessage;
        private readonly string[] _expectedExceptionErrors;

        public ExpectedUserActionException(string expectedExceptionMessage)
        {
            _expectedExceptionMessage = expectedExceptionMessage;
        }

        public ExpectedUserActionException(string expectedExceptionMessage, params string[] expectedExceptionError)
        {
            _expectedExceptionMessage = expectedExceptionMessage;
            _expectedExceptionErrors = expectedExceptionError;
        }

        protected override void Verify(Exception exception)
        {
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(exception, typeof(UserActionException), "Wrong type of exception was thrown.");

            var userActionException = (UserActionException)exception;

            if (!string.IsNullOrEmpty(_expectedExceptionMessage))
            {
                Assert.IsTrue(userActionException.Message.Contains(_expectedExceptionMessage), "Wrong exception message was returned.");
            }

            if (_expectedExceptionErrors.Length > 0)
            {
                var errors = userActionException.Errors.Values.Cast<IEnumerable<string>>().SelectMany(e => e).ToList();
                var doesErrosIncludeExpcetedError = _expectedExceptionErrors.All(expected => errors.Any(k => k.ToLower().Contains(expected.ToLower())));
                Assert.IsTrue(doesErrosIncludeExpcetedError, "Exception doesn't contain the correct error.");
            }
        }
    }
}
