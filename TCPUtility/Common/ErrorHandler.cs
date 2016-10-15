using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPUtility.Common
{
    class ErrorHandler
    {
        public static DelLogError CustomErrorHandler = null;

        #region methods

        public static void LogError(string method, string message, Exception ex)
        {
            if (CustomErrorHandler != null)
                CustomErrorHandler(method, message, ex);
        }
        public static void LogError(string method, string message)
        {
            LogError(method, message, null);
        }

        #endregion

        #region pubilc definitions

        public delegate void DelLogError(string method, string message, Exception ex);

        #endregion
    }
}
