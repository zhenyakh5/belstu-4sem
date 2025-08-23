using System;
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

        private void lblFullName_Click(object sender, EventArgs e)
        {

        }
    }

    public class Author
    {
        public string FullName { get; set; }
        public string Country { get; set; }
        public string ID { get; set; }
    }
}
