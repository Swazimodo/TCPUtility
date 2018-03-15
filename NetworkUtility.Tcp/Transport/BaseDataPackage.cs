using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkUtility.Tcp.Transport
{
    /// <summary>
    /// use this class to serialize all comunication to and from the server
    /// </summary>
    [Serializable]
    public abstract class BaseDataPackage
    {
        #region public methods

        /// <summary>
        /// Returns the class that inherited this base class
        /// </summary>
        /// <returns>child class</returns>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="OverflowException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public dynamic Unbox()
        {
            return Convert.ChangeType(this, this.GetType());
        }

        #endregion
    }
}
