using BLL.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace BLL.Test.Helpers
{
    public sealed class ExpectedUserActionException : ExpectedExceptionBaseAttribute
    {
        private readonly string _expectedExceptionMessage;
        private readonly string _expectedExceptionError;

        public ExpectedUserActionException(string expectedExceptionMessage)
        {
            _expectedExceptionMessage = expectedExceptionMessage;
        }

        public ExpectedUserActionException(string expectedExceptionMessage, string expectedExceptionError)
        {
            _expectedExceptionMessage = expectedExceptionMessage;
            _expectedExceptionError = expectedExceptionError;
        }

        protected override void Verify(Exception exception)
        {
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(exception, typeof(UserActionException), "Wrong type of exception was thrown.");

            var userActionException = (UserActionException)exception;

            if (!_expectedExceptionMessage.Length.Equals(0))
            {
                Assert.IsTrue(userActionException.Message.Contains(_expectedExceptionMessage), "Wrong exception message was returned.");
            }

            if (!_expectedExceptionError.Length.Equals(0))
            {
                var keys = userActionException.Errors.Keys.Cast<string>().ToList();
                var doesKeyIncludeError = keys.Any(k => k.ToLower().Contains(_expectedExceptionError.ToLower()));
                Assert.IsTrue(doesKeyIncludeError, "Exception doesn't contain the correct error.");
            }
        }
    }
}
