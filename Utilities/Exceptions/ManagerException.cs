using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Exceptions
{
    public class ManagerException: Exception
    {
        public ManagerException()
        {
        }

        public ManagerException(string message)
            : base(message)
        {
        }

        public ManagerException(string message, ManagerException inner)
            : base(message, inner)
        {
        }
    }
}
