using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace HotelManagement
{
    public partial class BookingsView : UserControl
    {
        private SqlConnection connection;

        public BookingsView()
        {
            InitializeComponent();
            connection = new SqlConnection("Data Source=(local); Initial Catalog=HotelManagement; Integrated Security=True; TrustServerCertificate=True");
            LoadBookings();
            StartDatePicker.SelectedDate = DateTime.Today;
            EndDatePicker.SelectedDate = DateTime.Today.AddDays(7);
        }

        private void LoadBookings()
        {
            try
            {
                connection.Open();
                string query = @"SELECT b.Id, r.RoomNumber, u.FullName AS GuestName, 
                               b.CheckInDate, b.CheckOutDate, b.TotalPrice, b.Status
                               FROM Bookings b
                               JOIN HotelRooms r ON b.RoomId = r.Id
                               JOIN Users u ON b.UserId = u.Id";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable bookingsTable = new DataTable();
                adapter.Fill(bookingsTable);
                BookingsDataGrid.ItemsSource = bookingsTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки бронирований: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void AddBooking_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new EditBookingDialog();
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    connection.Open();
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            string query = @"INSERT INTO Bookings 
                                          (RoomId, UserId, CheckInDate, CheckOutDate, TotalPrice, Status) 
                                          VALUES 
                                          (@RoomId, @UserId, @CheckInDate, @CheckOutDate, @TotalPrice, @Status)";

                            SqlCommand command = new SqlCommand(query, connection, transaction);
                            command.Parameters.AddWithValue("@RoomId", dialog.RoomId);
                            command.Parameters.AddWithValue("@UserId", dialog.UserId);
                            command.Parameters.AddWithValue("@CheckInDate", dialog.CheckInDate);
                            command.Parameters.AddWithValue("@CheckOutDate", dialog.CheckOutDate);
                            command.Parameters.AddWithValue("@TotalPrice", dialog.TotalPrice);
                            command.Parameters.AddWithValue("@Status", "Подтверждено");

                            command.ExecuteNonQuery();
                            transaction.Commit();
                            LoadBookings();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка добавления бронирования: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void EditBooking_Click(object sender, RoutedEventArgs e)
        {
            if (BookingsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите бронирование для редактирования");
                return;
            }

            DataRowView row = (DataRowView)BookingsDataGrid.SelectedItem;
            var dialog = new EditBookingDialog
            {
                BookingId = (int)row["Id"],
                RoomId = GetRoomId(row["RoomNumber"].ToString()),
                UserId = GetUserId(row["GuestName"].ToString()),
                CheckInDate = (DateTime)row["CheckInDate"],
                CheckOutDate = (DateTime)row["CheckOutDate"],
                TotalPrice = Convert.ToDouble(row["TotalPrice"])
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection("Data Source=(local); Initial Catalog=HotelManagement; Integrated Security=True; TrustServerCertificate=True"))
                    {
                        connection.Open();
                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                string query = @"UPDATE Bookings SET 
                                          RoomId = @RoomId, 
                                          CheckInDate = @CheckInDate, 
                                          CheckOutDate = @CheckOutDate, 
                                          TotalPrice = @TotalPrice
                                          WHERE Id = @Id";

                                SqlCommand command = new SqlCommand(query, connection, transaction);
                                command.Parameters.AddWithValue("@Id", dialog.BookingId);
                                command.Parameters.AddWithValue("@RoomId", dialog.RoomId);
                                command.Parameters.AddWithValue("@CheckInDate", dialog.CheckInDate);
                                command.Parameters.AddWithValue("@CheckOutDate", dialog.CheckOutDate);
                                command.Parameters.AddWithValue("@TotalPrice", dialog.TotalPrice);

                                command.ExecuteNonQuery();
                                transaction.Commit();
                                LoadBookings();
                            }
                            catch
                            {
                                transaction.Rollback();
                                throw;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка редактирования бронирования: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void CancelBooking_Click(object sender, RoutedEventArgs e)
        {
            if (BookingsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите бронирование для отмены");
                return;
            }

            if (MessageBox.Show("Вы уверены, что хотите отменить это бронирование?",
                "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                DataRowView row = (DataRowView)BookingsDataGrid.SelectedItem;
                try
                {
                    using (SqlConnection connection = new SqlConnection("Data Source=(local); Initial Catalog=HotelManagement; Integrated Security=True; TrustServerCertificate=True"))
                    {
                        connection.Open();
                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                string cancelQuery = "UPDATE Bookings SET Status = 'Cancelled' WHERE Id = @Id";
                                SqlCommand cancelCommand = new SqlCommand(cancelQuery, connection, transaction);
                                cancelCommand.Parameters.AddWithValue("@Id", row["Id"]);
                                cancelCommand.ExecuteNonQuery();

                                string getRoomQuery = "SELECT RoomId FROM Bookings WHERE Id = @Id";
                                SqlCommand getRoomCommand = new SqlCommand(getRoomQuery, connection, transaction);
                                getRoomCommand.Parameters.AddWithValue("@Id", row["Id"]);
                                int roomId = (int)getRoomCommand.ExecuteScalar();

                                string updateRoomQuery = "UPDATE HotelRooms SET Status = 'Available' WHERE Id = @RoomId";
                                SqlCommand updateRoomCommand = new SqlCommand(updateRoomQuery, connection, transaction);
                                updateRoomCommand.Parameters.AddWithValue("@RoomId", roomId);
                                updateRoomCommand.ExecuteNonQuery();

                                transaction.Commit();
                                LoadBookings();
                                MessageBox.Show("Бронирование успешно отменено");
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                MessageBox.Show("Ошибка при отмене бронирования: " + ex.Message);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка подключения: " + ex.Message);
                }
            }
        }

        private void SearchBookings_Click(object sender, RoutedEventArgs e)
        {
            if (StartDatePicker.SelectedDate == null || EndDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Выберите даты для поиска");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=(local); Initial Catalog=HotelManagement; Integrated Security=True; TrustServerCertificate=True"))
                {
                    connection.Open();
                    string query = @"SELECT b.Id, r.RoomNumber, u.FullName AS GuestName, 
                               b.CheckInDate, b.CheckOutDate, b.TotalPrice, b.Status
                               FROM Bookings b
                               JOIN HotelRooms r ON b.RoomId = r.Id
                               JOIN Users u ON b.UserId = u.Id
                               WHERE b.CheckInDate >= @StartDate AND b.CheckOutDate <= @EndDate";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@StartDate", StartDatePicker.SelectedDate);
                    command.Parameters.AddWithValue("@EndDate", EndDatePicker.SelectedDate);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable bookingsTable = new DataTable();
                    adapter.Fill(bookingsTable);
                    BookingsDataGrid.ItemsSource = bookingsTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка поиска бронирований: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private int GetRoomId(string roomNumber)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source = (local); Initial Catalog = HotelManagement; Integrated Security = True; TrustServerCertificate = True"))
                {
                    connection.Open();
                    string query = "SELECT Id FROM HotelRooms WHERE RoomNumber = @RoomNumber";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@RoomNumber", roomNumber);
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
            finally
            {
                connection.Close();
            }
        }

        private int GetUserId(string fullName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source = (local); Initial Catalog = HotelManagement; Integrated Security = True; TrustServerCertificate = True"))
                {
                    connection.Open();
                    string query = "SELECT Id FROM Users WHERE FullName = @FullName";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@FullName", fullName);
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}