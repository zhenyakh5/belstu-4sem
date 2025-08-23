using Microsoft.Win32;
using System.Windows;
using System.Windows.Input;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System;

namespace BookingService
{
    public partial class AddRoom : Window
    {
        private readonly AppDbContext _context;

        public static RoutedCommand BrowseCommand { get; } = new RoutedCommand();
        public static RoutedCommand AddRoomCommand { get; } = new RoutedCommand();
        public static RoutedCommand CancelCommand { get; } = new RoutedCommand();

        public AddRoom(AppDbContext context)
        {
            InitializeComponent();
            _context = context;
            CategoryComboBox.SelectedIndex = 0;

            CommandBindings.Add(new CommandBinding(BrowseCommand, ExecuteBrowseCommand));
            CommandBindings.Add(new CommandBinding(AddRoomCommand, ExecuteAddRoomCommand));
            CommandBindings.Add(new CommandBinding(CancelCommand, ExecuteCancelCommand));
        }

        private void ExecuteBrowseCommand(object sender, ExecutedRoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                ImagePathTextBox.Text = openFileDialog.FileName;
                UpdateImagePreview(openFileDialog.FileName);
            }
        }

        private void ImagePathTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateImagePreview(ImagePathTextBox.Text);
        }

        private void UpdateImagePreview(string imagePath)
        {
            try
            {
                if (File.Exists(imagePath))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(imagePath);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    ImagePreview.Source = bitmap;
                }
                else
                {
                    ImagePreview.Source = null;
                }
            }
            catch
            {
                ImagePreview.Source = null;
            }
        }

        private void ExecuteAddRoomCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (!ValidateInputs()) return;

            var newRoom = new HotelRoom
            {
                Name = NameTextBox.Text,
                ShortDescription = ShortDescriptionTextBox.Text,
                FullDescription = FullDescriptionTextBox.Text,
                ImagePath = ImagePathTextBox.Text,
                Category = (CategoryComboBox.SelectedItem as ComboBoxItem)?.Content.ToString(),
                Rating = double.Parse(RatingTextBox.Text),
                Price = (double)decimal.Parse(PriceTextBox.Text),
                NumberOfBeds = int.Parse(NumberOfBedsTextBox.Text),
                IsAvailable = true,
                Amenities = AmenitiesTextBox.Text,
                Stars = int.Parse(StarsTextBox.Text),
                HasBalcony = HasBalconyCheckBox.IsChecked ?? false,
                IsNonSmoking = IsNonSmokingCheckBox.IsChecked ?? false
            };

            var command = new AddRoomCommand(_context, newRoom);
            _context.ExecuteCommand(command);

            MessageBox.Show("Номер успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите название номера", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(ShortDescriptionTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите краткое описание", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(FullDescriptionTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите полное описание", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(ImagePathTextBox.Text) || !File.Exists(ImagePathTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, выберите корректное изображение", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!double.TryParse(RatingTextBox.Text, out double rating) || rating < 0 || rating > 5)
            {
                MessageBox.Show("Рейтинг должен быть числом от 0 до 5", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!decimal.TryParse(PriceTextBox.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Цена должна быть положительным числом", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!int.TryParse(NumberOfBedsTextBox.Text, out int beds) || beds <= 0)
            {
                MessageBox.Show("Количество кроватей должно быть положительным числом", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!int.TryParse(StarsTextBox.Text, out int stars) || stars < 1 || stars > 5)
            {
                MessageBox.Show("Количество звёзд должно быть от 1 до 5", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(AmenitiesTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, укажите удобства", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void ExecuteCancelCommand(object sender, ExecutedRoutedEventArgs e)
        {
            new AdminPanel().Show();
            this.Close();
        }
    }
}
