using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;

namespace lab2
{
    public partial class Form1 : Form
    {
        private List<Author> authors = new List<Author>();
        private BookFile bookFile;

        public Form1()
        {
            InitializeComponent();
            bookFile = new BookFile();
        }

        private void btnAddAuthor_Click(object sender, EventArgs e)
        {
            using (Form2 authorForm = new Form2())
            {
                if (authorForm.ShowDialog() == DialogResult.OK)
                {
                    authors.Add(authorForm.Author);
                    lstAuthors.Items.Add($"{authorForm.Author.FullName}, {authorForm.Author.Country}, {authorForm.Author.ID}");
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bookFile.Title = txtTitle.Text;
                bookFile.Publisher = txtPublisher.Text;
                bookFile.Year = int.Parse(txtYear.Text);
                bookFile.FileSize = (int)numFileSize.Value;
                bookFile.Pages = (int)numPages.Value;
                bookFile.Cost = decimal.Parse(txtCost.Text);
                bookFile.Genre = cmbGenre.SelectedItem.ToString();
                bookFile.Authors = authors;

                XmlSerializer serializer = new XmlSerializer(typeof(BookFile));
                using (StreamWriter writer = new StreamWriter("bookfile.xml"))
                {
                    serializer.Serialize(writer, bookFile);
                }

                MessageBox.Show("Данные успешно сохранены!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения данных: {ex.Message}");
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(BookFile));
                using (StreamReader reader = new StreamReader("bookfile.xml"))
                {
                    bookFile = (BookFile)serializer.Deserialize(reader);
                }

                txtTitle.Text = bookFile.Title;
                txtPublisher.Text = bookFile.Publisher;
                txtYear.Text = bookFile.Year.ToString();
                numFileSize.Value = bookFile.FileSize;
                numPages.Value = bookFile.Pages;
                txtCost.Text = bookFile.Cost.ToString();
                cmbGenre.SelectedItem = bookFile.Genre;

                authors = bookFile.Authors;
                lstAuthors.Items.Clear();
                foreach (var author in authors)
                {
                    lstAuthors.Items.Add($"{author.FullName}, {author.Country}, {author.ID}");
                }

                MessageBox.Show("Данные успешно загружены!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                decimal profit = bookFile.Cost * bookFile.Pages;
                lblResult.Text = $"Результат: {profit}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка расчета: {ex.Message}");
            }
        }

        private void progressBar2_Click(object sender, EventArgs e)
        {
            if (progressBar2.Value >= 100)
            {
                progressBar2.Value = 0;
            }
            else
            {
                progressBar2.Value += 10;
            }
        }
    }

    public class BookFile
    {
        public string Title { get; set; }
        public string Publisher { get; set; }
        public int Year { get; set; }
        public int FileSize { get; set; }
        public int Pages { get; set; }
        public decimal Cost { get; set; }
        public string Genre { get; set; }
        public List<Author> Authors { get; set; }
    }
}
