using ScFix.Utility.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ScFix.Utility.Interfaces
{

    public interface IDialogResultVMHelper
    {
        event EventHandler<RequestCloseEventArgs> RequestCloseDialog;

        ICommand Ok {get;}
        ICommand Cancel{get;}
    }
}
