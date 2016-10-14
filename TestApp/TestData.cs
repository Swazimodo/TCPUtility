using System;
using TCPUtility.Transport;

namespace TestApp
{
    [Serializable]
    public class TestData : BaseDataPackage
    {
        public int MyNum { get; set; }

        public TestData(int num) : base(typeof(TestData))
        {
            MyNum = num;
        }
    }
}
