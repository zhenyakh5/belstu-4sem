using System;
using System.Windows.Forms;

namespace lab2
{
    public partial class SearchForm : Form
    {
        public string SearchPattern { get; private set; }

        public SearchForm()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchPattern = txtPattern.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
