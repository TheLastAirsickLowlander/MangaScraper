using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScFix.Utility.Classes
{
    /// <summary>
    /// This passes the arguments back from the ViewModel driven dialgo
    /// </summary>
    public class RequestCloseEventArgs : EventArgs
    {
        public RequestCloseEventArgs(bool dialogResult, object item = null)
        {
            this.DialogResult = dialogResult;
            DialogItem = item;
        }

        public bool DialogResult { get; private set; }

        public object DialogItem { get; private set; }
    }
}
