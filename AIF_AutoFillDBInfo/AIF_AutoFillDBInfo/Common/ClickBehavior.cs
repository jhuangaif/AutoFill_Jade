using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AIFAutoFillDB.Common
{
    public class ClickBehavior
    {
        //doubleclick
        public static DependencyProperty DoubleClickCommandProperty = DependencyProperty.RegisterAttached("DoubleClick",
           typeof(ICommand),
           typeof(ClickBehavior),
           new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ClickBehavior.DoubleClickChanged)));

        public static void SetDoubleClick(DependencyObject target, ICommand value)
        {
            target.SetValue(ClickBehavior.DoubleClickCommandProperty, value);
        }

        public static ICommand GetDoubleClick(DependencyObject target)
        {
            return (ICommand)target.GetValue(DoubleClickCommandProperty);
        }

        private static void DoubleClickChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            ListBoxItem element = target as ListBoxItem;
            if (element != null)
            {
                if ((e.NewValue != null) && (e.OldValue == null))
                {
                    element.MouseDoubleClick += element_MouseDoubleClick;
                }
                else if ((e.NewValue == null) && (e.OldValue != null))
                {
                    element.MouseDoubleClick -= element_MouseDoubleClick;
                }
            }
        }

        static void element_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UIElement element = (UIElement)sender;
            ICommand command = (ICommand)element.GetValue(ClickBehavior.DoubleClickCommandProperty);
            command.Execute(element);
        }

        //Click
        public static DependencyProperty ClickCommandProperty = DependencyProperty.RegisterAttached("LeftClick",
           typeof(ICommand),
           typeof(ClickBehavior),
           new FrameworkPropertyMetadata(null, new PropertyChangedCallback(LeftClickChanged)));

        public static void SetLeftClick(DependencyObject target, ICommand value)
        {
            target.SetValue(ClickCommandProperty, value);
        }

        public static ICommand GetLeftClick(DependencyObject target)
        {
            return (ICommand)target.GetValue(ClickCommandProperty);
        }

        private static void LeftClickChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            ListBoxItem element = target as ListBoxItem;
            if (element != null)
            {
                if ((e.NewValue != null) && (e.OldValue == null))
                {
                    element.MouseLeftButtonUp += element_MouseClick;
                }
                else if ((e.NewValue == null) && (e.OldValue != null))
                {
                    element.MouseLeftButtonUp -= element_MouseClick;
                }
            }
        }

        static void element_MouseClick(object sender, MouseButtonEventArgs e)
        {
            UIElement element = (UIElement)sender;
            ICommand command = (ICommand)element.GetValue(ClickCommandProperty);
            command.Execute(element);
        }

        //RightClick
        public static DependencyProperty RightClickCommandProperty = DependencyProperty.RegisterAttached("RightClick",
           typeof(ICommand),
           typeof(ClickBehavior),
           new FrameworkPropertyMetadata(null, new PropertyChangedCallback(RightClickChanged)));

        public static void SetRightClick(DependencyObject target, ICommand value)
        {
            target.SetValue(RightClickCommandProperty, value);
        }

        public static ICommand GetRightClick(DependencyObject target)
        {
            return (ICommand)target.GetValue(RightClickCommandProperty);
        }

        private static void RightClickChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            ListBoxItem element = target as ListBoxItem;
            if (element != null)
            {
                if ((e.NewValue != null) && (e.OldValue == null))
                {
                    element.MouseRightButtonUp += element_MouseRightClick;
                }
                else if ((e.NewValue == null) && (e.OldValue != null))
                {
                    element.MouseRightButtonUp -= element_MouseRightClick;
                }
            }
        }

        static void element_MouseRightClick(object sender, MouseButtonEventArgs e)
        {
            UIElement element = (UIElement)sender;
            ICommand command = (ICommand)element.GetValue(RightClickCommandProperty);
            command.Execute(element);
        }

    }
}
