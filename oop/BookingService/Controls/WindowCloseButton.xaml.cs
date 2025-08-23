using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookingService.Controls
{
    public partial class WindowCloseButton : UserControl
    {
        public static readonly DependencyProperty TargetWindowProperty =
            DependencyProperty.Register(
                "TargetWindow",
                typeof(Window),
                typeof(WindowCloseButton),
                new FrameworkPropertyMetadata(
                    null,
                    null,
                    CoerceWindow),
                ValidateWindow);

        public static readonly RoutedEvent PreviewCloseEvent =
            EventManager.RegisterRoutedEvent(
                "PreviewClose",
                RoutingStrategy.Tunnel,
                typeof(RoutedEventHandler),
                typeof(WindowCloseButton));

        public static readonly DependencyProperty CloseCommandProperty =
            DependencyProperty.Register(
                "CloseCommand",
                typeof(RoutedUICommand),
                typeof(WindowCloseButton));

        public event RoutedEventHandler PreviewClose
        {
            add => AddHandler(PreviewCloseEvent, value);
            remove => RemoveHandler(PreviewCloseEvent, value);
        }

        public Window TargetWindow
        {
            get => (Window)GetValue(TargetWindowProperty);
            set => SetValue(TargetWindowProperty, value);
        }

        public RoutedUICommand CloseCommand
        {
            get => (RoutedUICommand)GetValue(CloseCommandProperty);
            set => SetValue(CloseCommandProperty, value);
        }

        public WindowCloseButton() => InitializeComponent();

        private static bool ValidateWindow(object value)
            => value == null || value is Window;

        private static object CoerceWindow(DependencyObject d, object value)
            => value ?? Window.GetWindow((WindowCloseButton)d);

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            var args = new RoutedEventArgs(PreviewCloseEvent, this);
            RaiseEvent(args);

            if (args.Handled) return;

            CloseCommand?.Execute(null, TargetWindow);

            TargetWindow?.Close();
        }
    }
}
