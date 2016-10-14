using System;
using System.Collections.Generic;
using TCPUtility.Transport;

namespace TCPUtility.Client
{
    public class DataRouting
    {
        /// <summary>
        /// list of all functions that can be called by different data packages
        /// </summary>
        public Dictionary<Type, IncomingData> Functions { get; private set; }

        public DataRouting()
        {
            Functions = new Dictionary<Type, IncomingData>();
        }

        /// <summary>
        /// This registers differnet data types that are supported.
        /// </summary>
        /// <param name="type">type of data that can come from server. This class must inherit from BaseDataPackage.</param>
        /// <param name="handler">Method to call when data shows up</param>
        public void RegisterHandler(Type type, IncomingData handler)
        {
            //check if type inherit from BaseDataPackage
            if (typeof(BaseDataPackage).IsAssignableFrom(type))
                Functions.Add(type, handler);
            else
                throw new ArgumentException("type must inherit from BaseDataPackage");
        }
        
        public void CallMethod(BaseDataPackage data)
        {
            Functions[data.DataType].Invoke(data);
        }

        public delegate void IncomingData(BaseDataPackage data);
    }
}
