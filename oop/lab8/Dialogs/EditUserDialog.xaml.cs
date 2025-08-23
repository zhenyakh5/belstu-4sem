using System.Windows;

namespace HotelManagement
{
    public partial class EditUserDialog : Window
    {
        public string Username
        {
            get => UsernameTextBox.Text;
            set => UsernameTextBox.Text = value;
        }

        public string Password
        {
            get => PasswordBox.Password;
            set => PasswordBox.Password = value;
        }

        public string FullName
        {
            get => FullNameTextBox.Text;
            set => FullNameTextBox.Text = value;
        }

        public string Email
        {
            get => EmailTextBox.Text;
            set => EmailTextBox.Text = value;
        }

        public string Phone
        {
            get => PhoneTextBox.Text;
            set => PhoneTextBox.Text = value;
        }

        public bool IsAdmin
        {
            get => IsAdminCheckBox.IsChecked ?? false;
            set => IsAdminCheckBox.IsChecked = value;
        }

        public EditUserDialog()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                MessageBox.Show("Введите логин");
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Введите пароль");
                return;
            }

            DialogResult = true;
        }
    }
}