using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
 
namespace AIFAutoFillDB.Common
{
    public class WaterMarkTextHelper : DependencyObject
    {
        #region Attached Properties

        public static bool GetIsMonitoring(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsMonitoringProperty);
        }

        public static void SetIsMonitoring(DependencyObject obj, bool value)
        {
            obj.SetValue(IsMonitoringProperty, value);
        }

        public static readonly DependencyProperty IsMonitoringProperty =
                                DependencyProperty.RegisterAttached("IsMonitoring", 
                                                                    typeof(bool), 
                                                                    typeof(WaterMarkTextHelper), 
                                                                    new UIPropertyMetadata(false, OnIsMonitoringChanged));


        public static bool GetWatermarkText(DependencyObject obj)
        {
            return (bool)obj.GetValue(WatermarkTextProperty);
        }

        public static void SetWatermarkText(DependencyObject obj, string value)
        {
            obj.SetValue(WatermarkTextProperty, value);
        }

        public static readonly DependencyProperty WatermarkTextProperty =
            DependencyProperty.RegisterAttached("WatermarkText", typeof(string), typeof(WaterMarkTextHelper), new UIPropertyMetadata(string.Empty));


        public static int GetTextLength(DependencyObject obj)
        {
            return (int)obj.GetValue(TextLengthProperty);
        }

        public static void SetTextLength(DependencyObject obj, int value)
        {
            obj.SetValue(TextLengthProperty, value);

            if (value >= 1)
                obj.SetValue(HasTextProperty, true);
            else
                obj.SetValue(HasTextProperty, false);
        }

        public static readonly DependencyProperty TextLengthProperty =
            DependencyProperty.RegisterAttached("TextLength", typeof(int), typeof(WaterMarkTextHelper), new UIPropertyMetadata(0));

        #endregion

        #region Internal DependencyProperty

        public bool HasText
        {
            get { return (bool)GetValue(HasTextProperty); }
            set { SetValue(HasTextProperty, value); }
        }

        private static readonly DependencyProperty HasTextProperty =
            DependencyProperty.RegisterAttached("HasText", typeof(bool), typeof(WaterMarkTextHelper), new FrameworkPropertyMetadata(false));

        #endregion

        #region Implementation

        static void OnIsMonitoringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox)
            {
                TextBox txtBox = d as TextBox;

                if ((bool)e.NewValue)
                    txtBox.TextChanged += TextChanged;
                else
                    txtBox.TextChanged -= TextChanged;
            }
            else if (d is PasswordBox)
            {
                PasswordBox passBox = d as PasswordBox;

                if ((bool)e.NewValue)
                    passBox.PasswordChanged += PasswordChanged;
                else
                    passBox.PasswordChanged -= PasswordChanged;
            }
        }

        static void TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txtBox = sender as TextBox;
            if (txtBox == null) return;
            SetTextLength(txtBox, txtBox.Text.Length);
        }

        static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passBox = sender as PasswordBox;
            if (passBox == null) return;
            SetTextLength(passBox, passBox.Password.Length);
        }

        #endregion
    }
    public static class PasswordBoxHelper
    {
        public static readonly DependencyProperty BoundPassword =
            DependencyProperty.RegisterAttached("BoundPassword", typeof(string), typeof(PasswordBoxHelper), new PropertyMetadata(string.Empty, OnBoundPasswordChanged));

        public static readonly DependencyProperty BindPassword = DependencyProperty.RegisterAttached(
            "BindPassword", typeof(bool), typeof(PasswordBoxHelper), new PropertyMetadata(false, OnBindPasswordChanged));

        public static readonly DependencyProperty UpdatingPassword =
            DependencyProperty.RegisterAttached("UpdatingPassword", typeof(bool), typeof(PasswordBoxHelper), new PropertyMetadata(false));

        private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox box = d as PasswordBox;

            // only handle this event when the property is attached to a PasswordBox
            // and when the BindPassword attached property has been set to true
            if (d == null || !GetBindPassword(d))
            {
                return;
            }

            // avoid recursive updating by ignoring the box's changed event
            box.PasswordChanged -= HandlePasswordChanged;

            string newPassword = (string)e.NewValue;

            if (!GetUpdatingPassword(box))
            {
                box.Password = newPassword;
            }

            box.PasswordChanged += HandlePasswordChanged;
        }

        private static void OnBindPasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            // when the BindPassword attached property is set on a PasswordBox,
            // start listening to its PasswordChanged event

            PasswordBox box = dp as PasswordBox;

            if (box == null)
            {
                return;
            }

            bool wasBound = (bool)(e.OldValue);
            bool needToBind = (bool)(e.NewValue);

            if (wasBound)
            {
                box.PasswordChanged -= HandlePasswordChanged;
            }

            if (needToBind)
            {
                box.PasswordChanged += HandlePasswordChanged;
            }
        }

        private static void HandlePasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox box = sender as PasswordBox;

            // set a flag to indicate that we're updating the password
            SetUpdatingPassword(box, true);
            // push the new password into the BoundPassword property
            SetBoundPassword(box, box.Password);
            SetUpdatingPassword(box, false);
        }

        public static void SetBindPassword(DependencyObject dp, bool value)
        {
            dp.SetValue(BindPassword, value);
        }

        public static bool GetBindPassword(DependencyObject dp)
        {
            return (bool)dp.GetValue(BindPassword);
        }

        public static string GetBoundPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(BoundPassword);
        }

        public static void SetBoundPassword(DependencyObject dp, string value)
        {
            dp.SetValue(BoundPassword, value);
        }

        private static bool GetUpdatingPassword(DependencyObject dp)
        {
            return (bool)dp.GetValue(UpdatingPassword);
        }

        private static void SetUpdatingPassword(DependencyObject dp, bool value)
        {
            dp.SetValue(UpdatingPassword, value);
        }


        //PasswordBox get focus
        public static DependencyProperty GetPasswordFocusCommandProperty = DependencyProperty.RegisterAttached("PasswordBoxGetFocus",
           typeof(ICommand),
           typeof(PasswordBoxHelper),
           new FrameworkPropertyMetadata(null, new PropertyChangedCallback(PasswordBoxGetFocus)));

        public static void SetPasswordBoxGetFocus(DependencyObject target, ICommand value)
        {
            target.SetValue(GetPasswordFocusCommandProperty, value);
        }

        public static ICommand GetPasswordBoxGetFocus(DependencyObject target)
        {
            return (ICommand)target.GetValue(GetPasswordFocusCommandProperty);
        }

        private static void PasswordBoxGetFocus(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox element = target as PasswordBox;
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
            ICommand command = (ICommand)element.GetValue(GetPasswordFocusCommandProperty);
            command.Execute(element);
        }

        //
        //PasswordBox lost focus
        public static DependencyProperty LostPasswordFocusCommandProperty = DependencyProperty.RegisterAttached("PasswordBoxLostFocus",
           typeof(ICommand),
           typeof(PasswordBoxHelper),
           new FrameworkPropertyMetadata(null, new PropertyChangedCallback(PasswordBoxLostFocus)));

        public static void SetPasswordBoxLostFocus(DependencyObject target, ICommand value)
        {
            target.SetValue(LostPasswordFocusCommandProperty, value);
        }

        public static ICommand GetPasswordBoxLostFocus(DependencyObject target)
        {
            return (ICommand)target.GetValue(LostPasswordFocusCommandProperty);
        }

        private static void PasswordBoxLostFocus(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox element = target as PasswordBox;
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
            ICommand command = (ICommand)element.GetValue(LostPasswordFocusCommandProperty);
            command.Execute(element);
        }


        //password changed

        //PasswordBox get focus
        public static DependencyProperty PasswordChangedCommandProperty = DependencyProperty.RegisterAttached("PasswordBoxChanged",
           typeof(ICommand),
           typeof(PasswordBoxHelper),
           new FrameworkPropertyMetadata(null, new PropertyChangedCallback(PasswordChanged)));

        public static void SetPasswordBoxChanged(DependencyObject target, ICommand value)
        {
            target.SetValue(GetPasswordFocusCommandProperty, value);
        }

        public static ICommand GetPasswordBoxChanged(DependencyObject target)
        {
            return (ICommand)target.GetValue(GetPasswordFocusCommandProperty);
        }

        private static void PasswordChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox element = target as PasswordBox;
            if (element != null)
            {
                if ((e.NewValue != null) && (e.OldValue == null))
                {
                    element.PasswordChanged += element_PasswordChanged;
                }
                else if ((e.NewValue == null) && (e.OldValue != null))
                {
                    element.PasswordChanged -= element_PasswordChanged;
                }
            }
        }

        static void element_PasswordChanged(object sender, RoutedEventArgs e)
        {
            UIElement element = (UIElement)sender;
            ICommand command = (ICommand)element.GetValue(PasswordChangedCommandProperty);
            command.Execute(element);
        }
    }

  //public class WaterMarkTextHelper : DependencyObject
  //{
  //    public static bool GetIsMonitoring(DependencyObject obj)
  //    {
  //        return (bool)obj.GetValue(IsMonitoringProperty);
  //    }
  //    public static void SetIsMonitoring(DependencyObject obj, bool value)
  //    {
  //        obj.SetValue(IsMonitoringProperty, value);
  //    }
  //    public static readonly DependencyProperty IsMonitoringProperty =
  //       DependencyProperty.RegisterAttached("IsMonitoring", typeof(bool), typeof(WaterMarkTextHelper), new UIPropertyMetadata(false, OnIsMonitoringChanged));
  //    public static int GetTextLength(DependencyObject obj)
  //    {
  //        return (int)obj.GetValue(TextLengthProperty);
  //    }
  //    public static void SetTextLength(DependencyObject obj, int value)
  //    {
  //        obj.SetValue(TextLengthProperty, value); if (value >= 1)
  //            obj.SetValue(HasTextProperty, true); else obj.SetValue(HasTextProperty, false);
  //    }
  //    public static readonly DependencyProperty TextLengthProperty =
  //       DependencyProperty.RegisterAttached("TextLength", typeof(int), typeof(WaterMarkTextHelper), new UIPropertyMetadata(0));
  //    private static readonly DependencyProperty HasTextProperty =
  //        DependencyProperty.RegisterAttached("HasText", typeof(bool), typeof(WaterMarkTextHelper), new FrameworkPropertyMetadata(false));
  //    public bool HasText
  //    {
  //        get { return (bool)GetValue(HasTextProperty); }
  //        set { SetValue(HasTextProperty, value); }
  //    }
  //    private static void OnIsMonitoringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  //    {
  //        if (d is TextBox)
  //        {
  //            TextBox txtBox = d as TextBox; if ((bool)e.NewValue)
  //                txtBox.TextChanged += TextChanged; else txtBox.TextChanged -= TextChanged;
  //        }
  //        else if (d is PasswordBox)
  //        {
  //            PasswordBox passBox = d as PasswordBox; if ((bool)e.NewValue)
  //                passBox.PasswordChanged += PasswordChanged; else passBox.PasswordChanged -= PasswordChanged;
  //        }
  //    }
  //    static void TextChanged(object sender, TextChangedEventArgs e)
  //    {
  //        TextBox txtBox = sender as TextBox; if (txtBox == null) return;
  //        SetTextLength(txtBox, txtBox.Text.Length);
  //    }
  //    static void PasswordChanged(object sender, RoutedEventArgs e)
  //    {
  //        PasswordBox passBox = sender as PasswordBox; if (passBox == null) return;
  //        SetTextLength(passBox, passBox.Password.Length);
  //    }
  //}
}
