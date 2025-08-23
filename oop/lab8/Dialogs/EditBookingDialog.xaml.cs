using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace HotelManagement
{
    public partial class EditBookingDialog : Window
    {
        public int BookingId { get; set; }
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public double TotalPrice { get; set; }

        private SqlConnection connection;

        public EditBookingDialog()
        {
            InitializeComponent();
            connection = new SqlConnection("Data Source=(local); Initial Catalog=HotelManagement; Integrated Security=True; TrustServerCertificate=True");
            LoadRooms();
            LoadUsers();
            CheckInDatePicker.SelectedDate = DateTime.Today;
            CheckOutDatePicker.SelectedDate = DateTime.Today.AddDays(1);
        }

        private void LoadRooms()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=(local); Initial Catalog=HotelManagement; Integrated Security=True; TrustServerCertificate=True"))
                {
                    connection.Open();
                    string query = "SELECT Id, RoomNumber FROM HotelRooms WHERE Status = 'Available'";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable roomsTable = new DataTable();
                    adapter.Fill(roomsTable);
                    RoomComboBox.ItemsSource = roomsTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки номеров: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void LoadUsers()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=(local); Initial Catalog=HotelManagement; Integrated Security=True; TrustServerCertificate=True"))
                {
                    connection.Open();
                    string query = "SELECT Id, FullName FROM Users";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable usersTable = new DataTable();
                    adapter.Fill(usersTable);
                    UserComboBox.ItemsSource = usersTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки пользователей: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void CalculatePrice_Click(object sender, RoutedEventArgs e)
        {
            if (RoomComboBox.SelectedItem == null ||
                CheckInDatePicker.SelectedDate == null ||
                CheckOutDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Заполните все поля для расчета");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=(local); Initial Catalog=HotelManagement; Integrated Security=True; TrustServerCertificate=True"))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("sp_CalculateBookingPrice", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    DataRowView roomRow = (DataRowView)RoomComboBox.SelectedItem;
                    command.Parameters.AddWithValue("@RoomId", roomRow["Id"]);
                    command.Parameters.AddWithValue("@CheckInDate", CheckInDatePicker.SelectedDate);
                    command.Parameters.AddWithValue("@CheckOutDate", CheckOutDatePicker.SelectedDate);

                    TotalPrice = Convert.ToDouble(command.ExecuteScalar());
                    TotalPriceTextBox.Text = TotalPrice.ToString("C");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка расчета стоимости: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (RoomComboBox.SelectedItem == null ||
                UserComboBox.SelectedItem == null ||
                CheckInDatePicker.SelectedDate == null ||
                CheckOutDatePicker.SelectedDate == null ||
                string.IsNullOrEmpty(TotalPriceTextBox.Text))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            DataRowView roomRow = (DataRowView)RoomComboBox.SelectedItem;
            DataRowView userRow = (DataRowView)UserComboBox.SelectedItem;

            RoomId = (int)roomRow["Id"];
            UserId = (int)userRow["Id"];
            CheckInDate = CheckInDatePicker.SelectedDate.Value;
            CheckOutDate = CheckOutDatePicker.SelectedDate.Value;

            DialogResult = true;
        }
    }
}