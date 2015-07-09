using ScFix.Utility.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ScFix.Utility.StaticMethods
{
    /// <summary>
    /// This is a basic service to display a dialog with the a given ViewModel
    /// <see cref="http://stackoverflow.com/questions/3801681/good-or-bad-practice-for-dialogs-in-wpf-with-mvvm"/>
    /// </summary>
    static public class ViewModelDialogService
    {

        public static bool? ShowDialog(String title, object viewModel, Window parent = null, bool borderless = true)
        { 
            Window win;
            if (borderless)
            {
                win = new BorderlessWindowDialog();
            }
            else 
            {
                win = new WindowDialog();
            }
            win.Title = title;
            win.DataContext = viewModel;
            if (parent != null)
            {
                win.Owner = parent;
            }
            return win.ShowDialog();
        }

        
    }
}
