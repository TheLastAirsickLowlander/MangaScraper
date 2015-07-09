using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for FileSelectionControl.xaml
    /// </summary>
    public partial class FileSelectionControl : UserControl
    {
        #region File
        public static readonly DependencyProperty FileProperty = DependencyProperty.Register("File", typeof(FileInfo), typeof(FileSelectionControl), new PropertyMetadata(FilePropertyChanged));
        public FileInfo File
        {
            get
            {
                return (FileInfo)GetValue(FileProperty);
            }
            set
            {
                if (value != File)
                {
                    SetValue(FileProperty, value);
                }
            }
        }
        private static void FilePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
        #endregion //File
        #region FileText
        public static readonly DependencyProperty FileTextProperty = DependencyProperty.Register("FileText", typeof(String), typeof(FileSelectionControl), new PropertyMetadata(FileTextPropertyChanged));
        public String FileText
        {
            get
            {
                return (String)GetValue(FileTextProperty);
            }
            set
            {
                if (value != FileText)
                {
                    SetValue(FileTextProperty, value);
                }
            }
        }
        private static void FileTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
        #endregion //FileText
        #region FileType
        public static readonly DependencyProperty FileTypeProperty = DependencyProperty.Register("FileType", typeof(string), typeof(FileSelectionControl), new PropertyMetadata(FileTypePropertyChanged));
        public string FileType
        {
            get
            {
                return (string)GetValue(FileTypeProperty);
            }
            set
            {
                if (value != FileType)
                {
                    SetValue(FileTypeProperty, value);
                }
            }
        }
        private static void FileTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
        #endregion //FileType

        public FileSelectionControl()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (FileType != "")
                dlg.DefaultExt = FileType;
           bool? value = dlg.ShowDialog();

           if (value.HasValue && value.Value)
           {
               FileText = dlg.FileName;
               File = new FileInfo(FileText);
           }
        }
    }
}
