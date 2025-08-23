using System.Linq;
using System.Windows;
using System;

public static class ThemeManager
{
    public static readonly string[] Themes = { "Light", "Dark" };
    private static string _currentTheme = "Light";

    public static string CurrentTheme => _currentTheme;

    public static void ChangeTheme(string themeName)
    {
        if (!Themes.Contains(themeName))
            throw new ArgumentException($"Unknown theme: {themeName}");

        var dictionaries = Application.Current.Resources.MergedDictionaries;
        var themeDict = dictionaries.FirstOrDefault(d =>
            d.Source?.OriginalString?.StartsWith("/Resources/Themes/") == true);

        if (themeDict != null)
        {
            dictionaries.Remove(themeDict);
        }

        var newTheme = new ResourceDictionary
        {
            Source = new Uri($"/BookingService;component/Resources/Themes/{themeName}Theme.xaml",
                           UriKind.Relative)
        };

        dictionaries.Add(newTheme);
        _currentTheme = themeName;
    }
}