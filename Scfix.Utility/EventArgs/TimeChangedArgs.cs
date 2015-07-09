using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScFix.Utility.Events
{
    public class TimeChangedArgs : EventArgs
    {
        public DateTime NewTime { get; set; }
    }
}
