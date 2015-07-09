using ScFix.Utility.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScFix.Utility.Controls
{
    /// <summary>
    /// Interaction logic for BorderlessWindowDlg.xaml
    /// </summary>
    public partial class BorderlessWindowDialog : Window
    {
        public BorderlessWindowDialog()
        {
            InitializeComponent(); 
            this.DialogPresenter.DataContextChanged += DialogPresenterDataContextChanged;
        }

        private void DialogPresenterDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var d = e.NewValue as IDialogResultVMHelper;

            if (d != null)
            {
                d.RequestCloseDialog += DialogResultTrueEvent;
            }
        }

        private void DialogResultTrueEvent(object sender, EventArgs e)
        {
            this.DialogResult = true;
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var obj = this.DataContext as IDialogResultVMHelper;
            if (obj != null)
            {
                //Unregister the event when it closes out
                obj.RequestCloseDialog -= DialogResultTrueEvent;
            }

        }
    }
}
