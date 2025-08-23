using System;
using System.Linq;
using System.Windows;

namespace BookingService
{
    public static class LanguageManager
    {
        private static string _currentLanguage = "ru-RU";

        public static string CurrentLanguage => _currentLanguage;

        public static void ChangeLanguage(string newLanguage)
        {
            var dict = new ResourceDictionary
            {
                Source = new Uri($"/BookingService;component/Resources/Languages/Lang_{newLanguage}.xaml",
                              UriKind.Relative)
            };

            var oldDict = Application.Current.Resources.MergedDictionaries
                .FirstOrDefault(d => d.Source?.OriginalString.Contains("Lang_") == true);

            if (oldDict != null)
            {
                Application.Current.Resources.MergedDictionaries.Remove(oldDict);
            }

            Application.Current.Resources.MergedDictionaries.Add(dict);
            _currentLanguage = newLanguage;
        }
    }
}
