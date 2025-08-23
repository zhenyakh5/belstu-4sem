using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace HotelManagement
{
    public partial class RoomsView : UserControl
    {
        private SqlConnection connection;
        private DataTable roomsTable;

        public RoomsView()
        {
            InitializeComponent();
            connection = new SqlConnection("Data Source=(local); Initial Catalog=HotelManagement; Integrated Security=True; TrustServerCertificate=True");
            LoadCategories();
            LoadRooms();
        }

        private void LoadCategories()
        {
            try
            {
                connection.Open();
                string query = "SELECT Id, Name FROM RoomCategories";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable categoriesTable = new DataTable();
                adapter.Fill(categoriesTable);

                CategoryComboBox.ItemsSource = categoriesTable.DefaultView;
                CategoryComboBox.DisplayMemberPath = "Name";
                CategoryComboBox.SelectedValuePath = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки категорий: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void LoadRooms()
        {
            using (SqlConnection connection = new SqlConnection("Data Source = (local); Initial Catalog = HotelManagement; Integrated Security = True; TrustServerCertificate = True"))
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT r.*, c.Name AS CategoryName, c.BasePrice AS Price 
                           FROM HotelRooms r 
                           JOIN RoomCategories c ON r.CategoryId = c.Id";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable roomsTable = new DataTable();
                    adapter.Fill(roomsTable);
                    RoomsDataGrid.ItemsSource = roomsTable.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки номеров: " + ex.Message);
                }
            }
        }

        private BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;

            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
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
            return image;
        }

        private void AddRoom_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new EditRoomDialog();
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
                                string query = @"INSERT INTO HotelRooms 
                                          (RoomNumber, CategoryId, Floor, Capacity, 
                                           Description, Amenities, Status, Image) 
                                          VALUES 
                                          (@RoomNumber, @CategoryId, @Floor, @Capacity, 
                                           @Description, @Amenities, @Status, @Image)";

                                SqlCommand command = new SqlCommand(query, connection, transaction);
                                command.Parameters.AddWithValue("@RoomNumber", dialog.RoomNumber);
                                command.Parameters.AddWithValue("@CategoryId", dialog.CategoryId);
                                command.Parameters.AddWithValue("@Floor", dialog.Floor);
                                command.Parameters.AddWithValue("@Capacity", dialog.Capacity);
                                command.Parameters.AddWithValue("@Description", dialog.Description);
                                command.Parameters.AddWithValue("@Amenities", dialog.Amenities);
                                command.Parameters.AddWithValue("@Status", dialog.Status);
                                command.Parameters.AddWithValue("@Image", dialog.ImageBytes ?? (object)DBNull.Value);

                                command.ExecuteNonQuery();
                                transaction.Commit();
                                LoadRooms();
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
                    MessageBox.Show("Ошибка добавления номера: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void EditRoom_Click(object sender, RoutedEventArgs e)
        {
            if (RoomsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите номер для редактирования");
                return;
            }

            DataRowView row = (DataRowView)RoomsDataGrid.SelectedItem;
            var dialog = new EditRoomDialog
            {
                RoomNumber = row["RoomNumber"].ToString(),
                CategoryId = (int)row["CategoryId"],
                Floor = (int)row["Floor"],
                Capacity = (int)row["Capacity"],
                Description = row["Description"].ToString(),
                Amenities = row["Amenities"].ToString(),
                Status = row["Status"].ToString(),
                ImageBytes = row["Image"] != DBNull.Value ? (byte[])row["Image"] : null
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
                                string query = @"UPDATE HotelRooms SET 
                                          RoomNumber = @RoomNumber, 
                                          CategoryId = @CategoryId, 
                                          Floor = @Floor, 
                                          Capacity = @Capacity, 
                                          Description = @Description, 
                                          Amenities = @Amenities, 
                                          Status = @Status, 
                                          Image = @Image 
                                          WHERE Id = @Id";

                                SqlCommand command = new SqlCommand(query, connection, transaction);
                                command.Parameters.AddWithValue("@Id", row["Id"]);
                                command.Parameters.AddWithValue("@RoomNumber", dialog.RoomNumber);
                                command.Parameters.AddWithValue("@CategoryId", dialog.CategoryId);
                                command.Parameters.AddWithValue("@Floor", dialog.Floor);
                                command.Parameters.AddWithValue("@Capacity", dialog.Capacity);
                                command.Parameters.AddWithValue("@Description", dialog.Description);
                                command.Parameters.AddWithValue("@Amenities", dialog.Amenities);
                                command.Parameters.AddWithValue("@Status", dialog.Status);
                                command.Parameters.AddWithValue("@Image", dialog.ImageBytes ?? (object)DBNull.Value);

                                command.ExecuteNonQuery();
                                transaction.Commit();
                                LoadRooms();
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
                    MessageBox.Show("Ошибка редактирования номера: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void DeleteRoom_Click(object sender, RoutedEventArgs e)
        {
            if (RoomsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите номер для удаления");
                return;
            }

            if (MessageBox.Show("Вы уверены, что хотите удалить этот номер?",
                "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                DataRowView row = (DataRowView)RoomsDataGrid.SelectedItem;
                try
                {
                    using (SqlConnection connection = new SqlConnection("Data Source=(local); Initial Catalog=HotelManagement; Integrated Security=True; TrustServerCertificate=True"))
                    {
                        connection.Open();
                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                string query = "DELETE FROM HotelRooms WHERE Id = @Id";
                                SqlCommand command = new SqlCommand(query, connection, transaction);
                                command.Parameters.AddWithValue("@Id", row["Id"]);
                                command.ExecuteNonQuery();
                                transaction.Commit();
                                LoadRooms();
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
                    MessageBox.Show("Ошибка удаления номера: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void SearchAvailable_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_FindAvailableRooms", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CheckInDate", DateTime.Today);
                command.Parameters.AddWithValue("@CheckOutDate", DateTime.Today.AddDays(1));

                if (CategoryComboBox.SelectedValue != null)
                    command.Parameters.AddWithValue("@CategoryId", CategoryComboBox.SelectedValue);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                roomsTable = new DataTable();
                adapter.Fill(roomsTable);

                DataTable displayTable = roomsTable.Clone();

                displayTable.Columns["Image"].DataType = typeof(BitmapImage);

                foreach (DataRow row in roomsTable.Rows)
                {
                    DataRow newRow = displayTable.NewRow();

                    foreach (DataColumn col in roomsTable.Columns)
                    {
                        if (col.ColumnName == "Image" && row["Image"] != DBNull.Value)
                        {
                            byte[] imageData = (byte[])row["Image"];
                            newRow["Image"] = LoadImage(imageData);
                        }
                        else
                        {
                            newRow[col.ColumnName] = row[col.ColumnName];
                        }
                    }

                    displayTable.Rows.Add(newRow);
                }

                RoomsDataGrid.ItemsSource = displayTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка поиска доступных номеров: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void ResetSearch_Click(object sender, RoutedEventArgs e)
        {
            CategoryComboBox.SelectedIndex = -1;
            LoadRooms();
        }

        private void RoomsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { }

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoryComboBox.SelectedValue == null) return;

            using (SqlConnection connection = new SqlConnection("Data Source = (local); Initial Catalog = HotelManagement; Integrated Security = True; TrustServerCertificate = True"))
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT r.*, c.Name AS CategoryName, c.BasePrice AS Price 
                          FROM HotelRooms r 
                          JOIN RoomCategories c ON r.CategoryId = c.Id
                          WHERE r.CategoryId = @CategoryId";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CategoryId", CategoryComboBox.SelectedValue);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable roomsTable = new DataTable();
                    adapter.Fill(roomsTable);

                    DataTable displayTable = roomsTable.Clone();

                    displayTable.Columns["Image"].DataType = typeof(BitmapImage);

                    foreach (DataRow row in roomsTable.Rows)
                    {
                        DataRow newRow = displayTable.NewRow();

                        foreach (DataColumn col in roomsTable.Columns)
                        {
                            if (col.ColumnName == "Image" && row["Image"] != DBNull.Value)
                            {
                                byte[] imageData = (byte[])row["Image"];
                                newRow["Image"] = LoadImage(imageData);
                            }
                            else
                            {
                                newRow[col.ColumnName] = row[col.ColumnName];
                            }
                        }

                        displayTable.Rows.Add(newRow);
                    }

                    RoomsDataGrid.ItemsSource = displayTable.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка фильтрации по категории: " + ex.Message);
                }
            }
        }
    }
}