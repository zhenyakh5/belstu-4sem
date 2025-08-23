using Microsoft.Win32;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace HotelManagement
{
    public partial class EditRoomDialog : Window
    {
        public string RoomNumber { get; set; }
        public int CategoryId { get; set; }
        public int Floor { get; set; }
        public int Capacity { get; set; }
        public string Description { get; set; }
        public string Amenities { get; set; }
        public string Status { get; set; }
        public byte[] ImageBytes { get; set; }

        private SqlConnection connection;

        public EditRoomDialog()
        {
            InitializeComponent();
            connection = new SqlConnection("Data Source=(local); Initial Catalog=HotelManagement; Integrated Security=True; TrustServerCertificate=True");
            LoadCategories();
            StatusComboBox.ItemsSource = new string[] { "Available", "Booked", "Maintenance" };
        }

        private void LoadCategories()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=(local); Initial Catalog=HotelManagement; Integrated Security=True; TrustServerCertificate=True"))
                {
                    connection.Open();
                    string query = "SELECT Id, Name FROM RoomCategories";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DataTable categoriesTable = new DataTable();
                            categoriesTable.Load(reader);
                            CategoryComboBox.ItemsSource = categoriesTable.DefaultView;
                            CategoryComboBox.DisplayMemberPath = "Name";
                            CategoryComboBox.SelectedValuePath = "Id";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки категорий: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    ImageBytes = File.ReadAllBytes(openFileDialog.FileName);
                    var image = new BitmapImage();
                    using (var mem = new MemoryStream(ImageBytes))
                    {
                        mem.Position = 0;
                        image.BeginInit();
                        image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.UriSource = null;
                        image.StreamSource = mem;
                        image.EndInit();
                    }
                    image.Freeze();
                    RoomImage.Source = image;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(RoomNumberTextBox.Text))
            {
                MessageBox.Show("Введите номер комнаты", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (CategoryComboBox.SelectedValue == null)
            {
                MessageBox.Show("Выберите категорию", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(FloorTextBox.Text, out int floor) || floor < 1)
            {
                MessageBox.Show("Введите корректный этаж (целое число больше 0)", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(CapacityTextBox.Text, out int capacity) || capacity < 1)
            {
                MessageBox.Show("Введите корректную вместимость (целое число больше 0)", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            RoomNumber = RoomNumberTextBox.Text;
            CategoryId = (int)CategoryComboBox.SelectedValue;
            Floor = floor;
            Capacity = capacity;
            Description = DescriptionTextBox.Text;
            Amenities = AmenitiesTextBox.Text;
            Status = StatusComboBox.SelectedItem?.ToString() ?? "Available";

            DialogResult = true;
        }
    }
}