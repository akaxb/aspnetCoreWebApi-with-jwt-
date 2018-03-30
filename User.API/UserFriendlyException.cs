using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.API
{
    public class UserFriendlyException : Exception
    {
        public UserFriendlyException()
        {

        }

        public UserFriendlyException(string message) : base(message)
        {

        }

        public UserFriendlyException(string message, Exception exception) : base(message, exception)
        {

        }
    }
}
