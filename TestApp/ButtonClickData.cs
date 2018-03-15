using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkUtility.Tcp.Transport;

namespace TestApp
{
    [Serializable]
    public class ButtonClickData : BaseDataPackage
    {
        public string ButtonClicked { get; set; }

        public ButtonClickData(string buttonName)
        {
            ButtonClicked = buttonName;
        }
    }
}
