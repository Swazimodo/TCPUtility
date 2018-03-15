using System;
using NetworkUtility.Tcp.Transport;

namespace TestApp
{
    [Serializable]
    public class TestData : BaseDataPackage
    {
        public int MyNum { get; set; }

        public TestData(int num)
        {
            MyNum = num;
        }
    }
}
