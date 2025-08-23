using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using Newtonsoft.Json;

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
            UpdateStatusBar();
        }

        private void btnAddAuthor_Click(object sender, EventArgs e)
        {
            using (Form2 authorForm = new Form2())
            {
                if (authorForm.ShowDialog() == DialogResult.OK)
                {
                    var validationResults = new List<ValidationResult>();
                    var validationContext = new ValidationContext(authorForm.Author);
                    if (Validator.TryValidateObject(authorForm.Author, validationContext, validationResults, true))
                    {
                        authors.Add(authorForm.Author);
                        lstAuthors.Items.Add($"{authorForm.Author.FullName}, {authorForm.Author.Country}, {authorForm.Author.ID}");
                        UpdateStatusBar();
                    }
                    else
                    {
                        foreach (var validationResult in validationResults)
                        {
                            MessageBox.Show(validationResult.ErrorMessage);
                        }
                    }
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
                UpdateStatusBar();
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
                UpdateStatusBar();
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
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка расчета: {ex.Message}");
            }
        }

        private void RegexSearchMenuItem_Click(object sender, EventArgs e)
        {
            using (SearchForm searchForm = new SearchForm())
            {
                if (searchForm.ShowDialog() == DialogResult.OK)
                {
                    string pattern = searchForm.SearchPattern;
                    var matches = authors.Where(a => Regex.IsMatch(a.FullName, pattern));
                    string results = string.Join("\n", matches.Select(a => a.FullName));
                    MessageBox.Show(results);

                    SaveSearchResultsToJson(matches.ToList(), "search_results.json");
                    UpdateStatusBar();
                }
            }
        }

        private void ExactSearchMenuItem_Click(object sender, EventArgs e)
        {
            using (SearchForm searchForm = new SearchForm())
            {
                if (searchForm.ShowDialog() == DialogResult.OK)
                {
                    string pattern = searchForm.SearchPattern;
                    var matches = authors.Where(a => a.FullName.Equals(pattern, StringComparison.OrdinalIgnoreCase));
                    string results = string.Join("\n", matches.Select(a => a.FullName));
                    MessageBox.Show(results);

                    SaveSearchResultsToJson(matches.ToList(), "exact_search_results.json");
                    UpdateStatusBar();
                }
            }
        }

        private void SaveSearchResultsToJson(List<Author> results, string filePath)
        {
            try
            {
                string json = JsonConvert.SerializeObject(results, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения результатов поиска: {ex.Message}");
            }
        }

        private void SortByNameMenuItem_Click(object sender, EventArgs e)
        {
            var sortedAuthors = authors.OrderBy(a => a.FullName).ToList();
            lstAuthors.Items.Clear();
            foreach (var author in sortedAuthors)
            {
                lstAuthors.Items.Add($"{author.FullName}, {author.Country}, {author.ID}");
            }

            SaveSortResultsToXml(sortedAuthors, "sorted_authors_by_name.xml");
            UpdateStatusBar();
        }

        private void SortByCountryMenuItem_Click(object sender, EventArgs e)
        {
            var sortedAuthors = authors.OrderBy(a => a.Country).ToList();
            lstAuthors.Items.Clear();
            foreach (var author in sortedAuthors)
            {
                lstAuthors.Items.Add($"{author.FullName}, {author.Country}, {author.ID}");
            }

            SaveSortResultsToXml(sortedAuthors, "sorted_authors_by_country.xml");
            UpdateStatusBar();
        }

        private void SaveSortResultsToXml(List<Author> results, string filePath)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Author>));
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    serializer.Serialize(writer, results);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения результатов сортировки: {ex.Message}");
            }
        }

        private void AboutMenu_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Версия: 1.0\nРазработчик: zhenyakh_", "О программе");
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            RegexSearchMenuItem_Click(sender, e);
        }

        private void toolStripClearButton_Click(object sender, EventArgs e)
        {
            lstAuthors.Items.Clear();
            authors.Clear();
            UpdateStatusBar();
        }

        private void toolStripDeleteButton_Click(object sender, EventArgs e)
        {
            if (lstAuthors.SelectedItem != null)
            {
                string selectedAuthor = lstAuthors.SelectedItem.ToString();
                var authorToRemove = authors.FirstOrDefault(a => $"{a.FullName}, {a.Country}, {a.ID}" == selectedAuthor);
                if (authorToRemove != null)
                {
                    authors.Remove(authorToRemove);
                    lstAuthors.Items.Remove(selectedAuthor);
                    UpdateStatusBar();
                }
            }
        }

        private void toolStripForwardButton_Click(object sender, EventArgs e)
        {
            if (lstAuthors.SelectedIndex < lstAuthors.Items.Count - 1)
            {
                lstAuthors.SelectedIndex++;
            }
        }

        private void toolStripBackButton_Click(object sender, EventArgs e)
        {
            if (lstAuthors.SelectedIndex > 0)
            {
                lstAuthors.SelectedIndex--;
            }
        }

        private void toolStripToggleToolbarButton_Click(object sender, EventArgs e)
        {
            toolStrip1.Visible = !toolStrip1.Visible;
        }

        private void UpdateStatusBar()
        {
            statusLabel.Text = $"Количество авторов: {authors.Count}. Последнее действие: обновление. Дата и время: {DateTime.Now}";
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

    public class CustomValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || !(value is string strValue) || strValue.Length < 3)
            {
                return new ValidationResult("Длина строки должна быть не менее 3 символов.");
            }
            return ValidationResult.Success;
        }
    }
}
