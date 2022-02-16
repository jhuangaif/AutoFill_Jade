using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AIFAutoFillDB.Common
{
    public class LostFocusBehavior
    {

        public static DependencyProperty LostFocusCommandProperty = DependencyProperty.RegisterAttached("LostFocus",
           typeof(ICommand),
           typeof(LostFocusBehavior),
           new FrameworkPropertyMetadata(null, new PropertyChangedCallback(LostFocusBehavior.LostFocusChanged)));

        public static void SetLostFocus(DependencyObject target, ICommand value)
        {
            target.SetValue(LostFocusBehavior.LostFocusCommandProperty, value);
        }

        public static ICommand GetLostFocus(DependencyObject target)
        {
            return (ICommand)target.GetValue(LostFocusCommandProperty);
        }

        private static void LostFocusChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            TextBox element = target as TextBox;
            if (element != null)
            {
                if ((e.NewValue != null) && (e.OldValue == null))
                {
                    element.LostFocus += element_LostFocus;
                }
                else if ((e.NewValue == null) && (e.OldValue != null))
                {
                    element.LostFocus -= element_LostFocus;
                }
            }
        }

        static void element_LostFocus(object sender, RoutedEventArgs e)
        {
            UIElement element = (UIElement)sender;
            ICommand command = (ICommand)element.GetValue(LostFocusBehavior.LostFocusCommandProperty);
            command.Execute(element);
        }


    }
    public class TextBoxGetFocusBehavior
    {

        public static DependencyProperty GetFocusCommandProperty = DependencyProperty.RegisterAttached("TextBoxGetFocus",
           typeof(ICommand),
           typeof(TextBoxGetFocusBehavior),
           new FrameworkPropertyMetadata(null, new PropertyChangedCallback(TextBoxGetFocusChanged)));

        public static void SetTextBoxGetFocus(DependencyObject target, ICommand value)
        {
            target.SetValue(GetFocusCommandProperty, value);
        }

        public static ICommand GetTextBoxGetFocus(DependencyObject target)
        {
            return (ICommand)target.GetValue(GetFocusCommandProperty);
        }

        private static void TextBoxGetFocusChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            TextBox element = target as TextBox;
            if (element != null)
            {
                if ((e.NewValue != null) && (e.OldValue == null))
                {
                    element.GotFocus += element_GotFocus;
                }
                else if ((e.NewValue == null) && (e.OldValue != null))
                {
                    element.GotFocus -= element_GotFocus;
                }
            }
        }

        static void element_GotFocus(object sender, RoutedEventArgs e)
        {
            UIElement element = (UIElement)sender;
            ICommand command = (ICommand)element.GetValue(TextBoxGetFocusBehavior.GetFocusCommandProperty);
            command.Execute(element);
        }


    }

    // Give first focus
    public class FocusBehavior
    {
        public static DependencyProperty FocusFirstProperty =
            DependencyProperty.RegisterAttached(
                "FocusFirst",
                typeof(bool),
                typeof(FocusBehavior),
                new PropertyMetadata(false, OnFocusFirstPropertyChanged));

        public static bool GetFocusFirst(DependencyObject control)
        {
            return (bool)control.GetValue(FocusFirstProperty);
        }

        public static void SetFocusFirst(DependencyObject control, bool value)
        {
            control.SetValue(FocusFirstProperty, value);
        }

        static void OnFocusFirstPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj == null || !(args.NewValue is bool))
            {
                return;
            } 
            if (obj is TextBox)
            {
                TextBox control = obj as TextBox;

                if ((bool)args.NewValue)
                {
                    control.Loaded += (sender, e) =>
                        control.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
                }
            }
            else if (obj is PasswordBox)
            {
                PasswordBox control = obj as PasswordBox;

                if ((bool)args.NewValue)
                {
                    control.Loaded += (sender, e) =>
                        control.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
                }
            }
        }
    }
}

