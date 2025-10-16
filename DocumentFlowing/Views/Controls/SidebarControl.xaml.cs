using System;
using System.Windows;
using System.Windows.Controls;

namespace DocumentFlowing.Views.Controls
{
    public partial class SidebarControl : UserControl
    {
        public SidebarControl()
        {
            InitializeComponent();
        }

        //public string Title
        //{
        //    get => (string)GetValue(TitleProperty);
        //    set => SetValue(TitleProperty, value);
        //}

        //public static readonly DependencyProperty TitleProperty =
        //    DependencyProperty.Register(nameof(Title), typeof(string), typeof(SidebarControl), new PropertyMetadata("Panel"));

        public event RoutedEventHandler? LogoutClicked;
        public event RoutedEventHandler? SettingsClicked;

        private void logout_Click(object sender, RoutedEventArgs e)
        {
            LogoutClicked?.Invoke(this, e);
        }

        private void settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsClicked?.Invoke(this, e);
        }

        public void AddMenuItem(string name, RoutedEventHandler clickHandler)
        {
            var btn = new Button
            {
                Content = name,
                Margin = new Thickness(10, 5, 10, 5),
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            btn.Click += clickHandler;
            MenuItems.Items.Add(btn);
        }

        public void ClearMenu() => MenuItems.Items.Clear();
    }
}
