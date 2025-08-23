using System;
using System.Windows;
using System.Windows.Controls;

namespace BookingService.Controls
{
    public partial class WindowMinimizeButton : UserControl
    {
        public static readonly DependencyProperty TargetWindowProperty =
            DependencyProperty.Register(
                "TargetWindow",
                typeof(Window),
                typeof(WindowMinimizeButton),
                new PropertyMetadata(null, OnTargetWindowChanged),
                ValidateWindow);

        public static readonly RoutedEvent MinimizeEvent =
            EventManager.RegisterRoutedEvent(
                "Minimize",
                RoutingStrategy.Bubble, 
                typeof(RoutedEventHandler),
                typeof(WindowMinimizeButton));

        public event RoutedEventHandler Minimize
        {
            add => AddHandler(MinimizeEvent, value);
            remove => RemoveHandler(MinimizeEvent, value);
        }

        public Window TargetWindow
        {
            get => (Window)GetValue(TargetWindowProperty);
            set => SetValue(TargetWindowProperty, value);
        }

        public WindowMinimizeButton() => InitializeComponent();

        private static bool ValidateWindow(object value)
            => value == null || value is Window;

        private static void OnTargetWindowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Console.WriteLine("Окно изменилось.");
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            if (TargetWindow != null)
            {
                TargetWindow.WindowState = WindowState.Minimized;
                RaiseEvent(new RoutedEventArgs(MinimizeEvent, this));
            }
        }
    }
}
