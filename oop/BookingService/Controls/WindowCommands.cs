using System.Windows.Input;

namespace BookingService.Controls
{
    public static class WindowCommands
    {
        public static readonly RoutedUICommand CloseWindow =
            new RoutedUICommand(
                "Close Window",
                "CloseWindow",
                typeof(WindowCommands));
    }
}