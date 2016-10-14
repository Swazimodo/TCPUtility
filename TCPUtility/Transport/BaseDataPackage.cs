using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPUtility.Transport
{
    /// <summary>
    /// use this class to serialize all comunication to and from the server
    /// </summary>
    [Serializable]
    public abstract class BaseDataPackage
    {
        #region properties

        /// <summary>
        /// Type of package
        /// </summary>
        public Type DataType { get; private set; }

        #endregion

        #region constructors

        public BaseDataPackage(Type TargetType)
        {
            DataType = TargetType;
        }

        #endregion

        #region public methods

        /// <summary>
        /// Returns the class that inherited this base class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>child class</returns>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public T Unbox<T>()
        {
            if (this is T)
                return (T)Convert.ChangeType(this, typeof(T));
            return default(T);
        }

        public dynamic Unbox()
        {
            return Convert.ChangeType(this, DataType);
        }

        #endregion
    }
}
