using System;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;

namespace lab2
{
    public partial class Form2 : Form
    {
        public Author Author { get; private set; }

        public Form2()
        {
            InitializeComponent();
            Author = new Author();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Author.FullName = txtFullName.Text;
            Author.Country = txtCountry.Text;
            Author.ID = txtID.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }

    public class Author
    {
        [Required(ErrorMessage = "Имя обязательно")]
        [RegularExpression(@"^[a-zA-Zа-яА-Я\s]+$", ErrorMessage = "Имя должно содержать только буквы.")]
        public string FullName { get; set; }

        [RegularExpression(@"^[a-zA-Zа-яА-Я\s]+$", ErrorMessage = "Страна должна содержать только буквы.")]
        [CustomValidation]
        public string Country { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "ID должен содержать только цифры.")]
        public string ID { get; set; }
    }
}
