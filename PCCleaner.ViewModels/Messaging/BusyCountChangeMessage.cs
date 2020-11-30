using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCleaner.ViewModels.Messaging
{
    public class BusyCountChangeMessage
    {
        public BusyCountChangeMessage(int changeValue)
        {
            this.ChangeValue = changeValue;
        }

        public int ChangeValue { get; private set; }

        public bool IsFromLocator { get; set; }
    }
}
