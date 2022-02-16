using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AIFAutoFillDB.Common
{
    public class TextChangedBehavior
    {

        public static DependencyProperty TextChangedCommandProperty = DependencyProperty.RegisterAttached("TextChanged",
           typeof(ICommand),
           typeof(TextChangedBehavior),
           new FrameworkPropertyMetadata(null, new PropertyChangedCallback(TextChangedBehavior.TextChangedCommandChanged)));

        public static void SetTextChanged(DependencyObject target, ICommand value)
        {
            target.SetValue(TextChangedBehavior.TextChangedCommandProperty, value);
        }

        public static ICommand GetTextChanged(DependencyObject target)
        {
            return (ICommand)target.GetValue(TextChangedCommandProperty);
        }

        private static void TextChangedCommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            TextBox element = target as TextBox;
            if (element != null)
            {
                if ((e.NewValue != null) && (e.OldValue == null))
                {
                    element.TextChanged += element_TextChanged;
                }
                else if ((e.NewValue == null) && (e.OldValue != null))
                {
                    element.TextChanged -= element_TextChanged;
                }
            }
        }

        static void element_TextChanged(object sender, RoutedEventArgs e)
        {
            UIElement element = (UIElement)sender;
            ICommand command = (ICommand)element.GetValue(TextChangedBehavior.TextChangedCommandProperty);
            command.Execute(element);
        }

        ////SearchBox lostfocus behaviour

        //public static DependencyProperty SearchBoxlostfocusCommandProperty = DependencyProperty.RegisterAttached("SearchBoxLostfocus",
        //   typeof(ICommand),
        //   typeof(TextChangedBehavior),
        //   new FrameworkPropertyMetadata(null, new PropertyChangedCallback(TextChangedBehavior.SearchBoxLostfocusCommandChanged)));

        //public static void SetSearchBoxLostfocus(DependencyObject target, ICommand value)
        //{
        //    target.SetValue(SearchBoxlostfocusCommandProperty, value);
        //}

        //public static ICommand GetSearchBoxLostfocus(DependencyObject target)
        //{
        //    return (ICommand)target.GetValue(SearchBoxlostfocusCommandProperty);
        //}

        //private static void SearchBoxLostfocusCommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        //{
        //    TextBox element = target as TextBox;
        //    if (element != null)
        //    {
        //        if ((e.NewValue != null) && (e.OldValue == null))
        //        {
        //            element.LostFocus += element_LostFocus;
        //        }
        //        else if ((e.NewValue == null) && (e.OldValue != null))
        //        {
        //            element.LostFocus -= element_LostFocus;
        //        }
        //    }
        //}

        //static void element_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    UIElement element = (UIElement)sender;
        //    ICommand command = (ICommand)element.GetValue(TextChangedBehavior.SearchBoxlostfocusCommandProperty);
        //    command.Execute(element);
        //}

        //listviewer get mouse click behaviour

        public static DependencyProperty ScrollviewerClickCommandProperty = DependencyProperty.RegisterAttached("ScrollviewerClick",
           typeof(ICommand),
           typeof(TextChangedBehavior),
           new FrameworkPropertyMetadata(null, new PropertyChangedCallback(TextChangedBehavior.ScrollviewerClickCommandChanged)));

        public static void SetScrollviewerClick(DependencyObject target, ICommand value)
        {
            target.SetValue(ScrollviewerClickCommandProperty, value);
        }

        public static ICommand GetScrollviewerClick(DependencyObject target)
        {
            return (ICommand)target.GetValue(ScrollviewerClickCommandProperty);
        }

        private static void ScrollviewerClickCommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            //ScrollViewer element = target as ScrollViewer;
            Grid element = target as Grid;
            if (element != null)
            {
                if ((e.NewValue != null) && (e.OldValue == null))
                {
                    element.MouseLeftButtonUp += element_Click;
                }
                else if ((e.NewValue == null) && (e.OldValue != null))
                {
                    element.MouseLeftButtonUp -= element_Click;
                }
            }
        }

        static void element_Click(object sender, RoutedEventArgs e)
        {
            UIElement element = (UIElement)sender;
            ICommand command = (ICommand)element.GetValue(TextChangedBehavior.ScrollviewerClickCommandProperty);
            command.Execute(element);
        }
    }

}

