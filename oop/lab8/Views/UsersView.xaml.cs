using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace HotelManagement
{
    public partial class UsersView : UserControl
    {
        private SqlConnection connection;
        private DataTable usersTable;

        public UsersView()
        {
            InitializeComponent();
            connection = new SqlConnection("Data Source=(local); Initial Catalog=HotelManagement; Integrated Security=True; TrustServerCertificate=True");
            LoadUsers();
        }

        private void LoadUsers()
        {
            SqlConnection.ClearAllPools();
            using (SqlConnection connection = new SqlConnection("Data Source=(local); Initial Catalog=HotelManagement; Integrated Security=True; TrustServerCertificate=True"));
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM Users";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable usersTable = new DataTable();
                    adapter.Fill(usersTable);
                    UsersDataGrid.ItemsSource = usersTable.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки пользователей: " + ex.Message);
                }
            }
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new EditUserDialog();
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
                                string query = @"INSERT INTO Users 
                                          (Username, Password, FullName, Email, Phone, IsAdmin) 
                                          VALUES 
                                          (@Username, @Password, @FullName, @Email, @Phone, @IsAdmin)";

                                SqlCommand command = new SqlCommand(query, connection, transaction);
                                command.Parameters.AddWithValue("@Username", dialog.Username);
                                command.Parameters.AddWithValue("@Password", dialog.Password);
                                command.Parameters.AddWithValue("@FullName", dialog.FullName);
                                command.Parameters.AddWithValue("@Email", dialog.Email);
                                command.Parameters.AddWithValue("@Phone", dialog.Phone);
                                command.Parameters.AddWithValue("@IsAdmin", dialog.IsAdmin);

                                command.ExecuteNonQuery();
                                transaction.Commit();
                                connection.Close();
                                LoadUsers();
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
                    MessageBox.Show("Ошибка добавления пользователя: " + ex.Message);
                }
            }
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите пользователя для редактирования");
                return;
            }

            DataRowView row = (DataRowView)UsersDataGrid.SelectedItem;
            var dialog = new EditUserDialog
            {
                Username = row["Username"].ToString(),
                FullName = row["FullName"].ToString(),
                Email = row["Email"].ToString(),
                Phone = row["Phone"].ToString(),
                IsAdmin = (bool)row["IsAdmin"]
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
                                string query = @"UPDATE Users SET 
                                          Username = @Username, 
                                          Password = @Password, 
                                          FullName = @FullName, 
                                          Email = @Email, 
                                          Phone = @Phone, 
                                          IsAdmin = @IsAdmin 
                                          WHERE Id = @Id";

                                SqlCommand command = new SqlCommand(query, connection, transaction);
                                command.Parameters.AddWithValue("@Id", row["Id"]);
                                command.Parameters.AddWithValue("@Username", dialog.Username);
                                command.Parameters.AddWithValue("@Password", dialog.Password);
                                command.Parameters.AddWithValue("@FullName", dialog.FullName);
                                command.Parameters.AddWithValue("@Email", dialog.Email);
                                command.Parameters.AddWithValue("@Phone", dialog.Phone);
                                command.Parameters.AddWithValue("@IsAdmin", dialog.IsAdmin);

                                command.ExecuteNonQuery();
                                transaction.Commit();
                                connection.Close();
                                LoadUsers();
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
                    MessageBox.Show("Ошибка редактирования пользователя: " + ex.Message);
                }
            }
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите пользователя для удаления");
                return;
            }

            if (MessageBox.Show("Вы уверены, что хотите удалить этого пользователя?",
                "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                DataRowView row = (DataRowView)UsersDataGrid.SelectedItem;
                try
                {
                    using (SqlConnection connection = new SqlConnection("Data Source=(local); Initial Catalog=HotelManagement; Integrated Security=True; TrustServerCertificate=True"))
                    {
                        connection.Open();
                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                string query = "DELETE FROM Users WHERE Id = @Id";
                                SqlCommand command = new SqlCommand(query, connection, transaction);
                                command.Parameters.AddWithValue("@Id", row["Id"]);
                                command.ExecuteNonQuery();
                                transaction.Commit();
                                connection.Close();
                                LoadUsers();
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
                    MessageBox.Show("Ошибка удаления пользователя: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void SearchUsers_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchTextBox.Text.Trim();
            if (string.IsNullOrEmpty(searchText))
            {
                LoadUsers();
                return;
            }

            try
            {
                connection.Open();
                string query = @"SELECT * FROM Users 
                               WHERE Username LIKE @Search OR 
                                     FullName LIKE @Search OR 
                                     Email LIKE @Search OR 
                                     Phone LIKE @Search";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Search", "%" + searchText + "%");

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                usersTable = new DataTable();
                adapter.Fill(usersTable);
                UsersDataGrid.ItemsSource = usersTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка поиска: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void UsersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        { }
    }
}