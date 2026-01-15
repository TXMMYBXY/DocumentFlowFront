using System.Windows;
using System.Windows.Controls;

namespace DocumentFlowing.Common
{
    public static class PasswordBoxHelper
    {
        private static readonly DependencyProperty _isUpdatingProperty =
            DependencyProperty.RegisterAttached("_isUpdating", typeof(bool), typeof(PasswordBoxHelper), new PropertyMetadata(false));
        private static bool _GetIsUpdating(DependencyObject dp) => (bool)dp.GetValue(_isUpdatingProperty);
        private static void _SetIsUpdating(DependencyObject dp, bool value) => dp.SetValue(_isUpdatingProperty, value);

        public static readonly DependencyProperty BoundPasswordProperty =
            DependencyProperty.RegisterAttached(
                "BoundPassword",
                typeof(string),
                typeof(PasswordBoxHelper),
                new PropertyMetadata(string.Empty, _OnBoundPasswordChanged));

        public static string GetBoundPassword(DependencyObject d)
        {
            return (string)d.GetValue(BoundPasswordProperty);
        }

        public static void SetBoundPassword(DependencyObject d, string value)
        {
            d.SetValue(BoundPasswordProperty, value);
        }

        private static void _OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox box)
            {
                box.PasswordChanged -= _PasswordChanged; 

                if (!_GetIsUpdating(box))
                {
                    box.Password = (string)e.NewValue;
                }
                
                box.PasswordChanged += _PasswordChanged;
            }
        }

        private static void _PasswordChanged(object sender, RoutedEventArgs e)
        {
            var box = sender as PasswordBox;
            _SetIsUpdating(box, true);
            SetBoundPassword(box, box.Password);
            _SetIsUpdating(box, false);
        }
    }
}