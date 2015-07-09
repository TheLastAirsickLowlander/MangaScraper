using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ScFix.Utility.StaticMethods
{
    static public class Helpers
    {
        public static T FindAnchestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }
        public static object GetPropertyValue(object src, string PropName)
        {
            try
            {
                var value = src.GetType().GetProperty(PropName).GetValue(src, null);
                return value;
            }
            catch (Exception ex)
            {
                throw new Exception("Get Property Value Exception", ex);
            }
        }
       
    }
}
