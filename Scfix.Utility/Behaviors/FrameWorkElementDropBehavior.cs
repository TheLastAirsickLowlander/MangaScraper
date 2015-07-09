using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace ScFix.Utility.Behaviors
{
    /// <summary>
    /// <see cref="http://www.wpfsharp.com/2012/03/22/mvvm-and-drag-and-drop-command-binding-with-an-attached-behavior/"/>
    /// </summary>
    class FrameWorkElementDropBehavior : Behavior<FrameworkElement>
    {
        #region DependedncyProperties
        public static readonly DependencyProperty CreateDragObjectProperty = DependencyProperty.Register("CreateDragObject", typeof(Object), typeof(FrameworkElementDragBehavior), new PropertyMetadata(CreateDragObjectPropertyChanged));
        public Object CreateDragObject
        {
            get
            {
                return (Object)GetValue(CreateDragObjectProperty);
            }
            set
            {
                if (value != CreateDragObject)
                {
                    SetValue(CreateDragObjectProperty, value);
                }
            }
        }
        private static void CreateDragObjectPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion //DependencyProperties

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.MouseLeftButtonDown += new MouseButtonEventHandler(AssociatedObject_MouseLeftButtonDown);
            this.AssociatedObject.MouseMove += new MouseEventHandler(AssociatedObject_MouseMove);

        }

        private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AssociatedObject_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
