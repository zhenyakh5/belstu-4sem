namespace lab2
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.TextBox txtPublisher;
        private System.Windows.Forms.TextBox txtYear;
        private System.Windows.Forms.TextBox txtCost;
        private System.Windows.Forms.ComboBox cmbGenre;
        private System.Windows.Forms.Button btnAddAuthor;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnCalculate;
        private System.Windows.Forms.ListBox lstAuthors;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblPublisher;
        private System.Windows.Forms.Label lblYear;
        private System.Windows.Forms.Label lblFileSize;
        private System.Windows.Forms.Label lblPages;
        private System.Windows.Forms.Label lblCost;
        private System.Windows.Forms.Label lblGenre;
        private System.Windows.Forms.Label lblAuthors;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.NumericUpDown numFileSize;
        private System.Windows.Forms.NumericUpDown numPages;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.txtPublisher = new System.Windows.Forms.TextBox();
            this.txtYear = new System.Windows.Forms.TextBox();
            this.txtCost = new System.Windows.Forms.TextBox();
            this.cmbGenre = new System.Windows.Forms.ComboBox();
            this.btnAddAuthor = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnCalculate = new System.Windows.Forms.Button();
            this.lstAuthors = new System.Windows.Forms.ListBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblPublisher = new System.Windows.Forms.Label();
            this.lblYear = new System.Windows.Forms.Label();
            this.lblFileSize = new System.Windows.Forms.Label();
            this.lblPages = new System.Windows.Forms.Label();
            this.lblCost = new System.Windows.Forms.Label();
            this.lblGenre = new System.Windows.Forms.Label();
            this.lblAuthors = new System.Windows.Forms.Label();
            this.lblResult = new System.Windows.Forms.Label();
            this.numFileSize = new System.Windows.Forms.NumericUpDown();
            this.numPages = new System.Windows.Forms.NumericUpDown();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numFileSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPages)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(120, 20);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(200, 20);
            this.txtTitle.TabIndex = 0;
            // 
            // txtPublisher
            // 
            this.txtPublisher.Location = new System.Drawing.Point(120, 60);
            this.txtPublisher.Name = "txtPublisher";
            this.txtPublisher.Size = new System.Drawing.Size(200, 20);
            this.txtPublisher.TabIndex = 1;
            // 
            // txtYear
            // 
            this.txtYear.Location = new System.Drawing.Point(120, 100);
            this.txtYear.Name = "txtYear";
            this.txtYear.Size = new System.Drawing.Size(200, 20);
            this.txtYear.TabIndex = 2;
            // 
            // txtCost
            // 
            this.txtCost.Location = new System.Drawing.Point(120, 220);
            this.txtCost.Name = "txtCost";
            this.txtCost.Size = new System.Drawing.Size(200, 20);
            this.txtCost.TabIndex = 5;
            // 
            // cmbGenre
            // 
            this.cmbGenre.FormattingEnabled = true;
            this.cmbGenre.Items.AddRange(new object[] {
            "Художественная",
            "Нон-фикшн",
            "Наука",
            "История"});
            this.cmbGenre.Location = new System.Drawing.Point(120, 260);
            this.cmbGenre.Name = "cmbGenre";
            this.cmbGenre.Size = new System.Drawing.Size(200, 21);
            this.cmbGenre.TabIndex = 6;
            // 
            // btnAddAuthor
            // 
            this.btnAddAuthor.Location = new System.Drawing.Point(10, 339);
            this.btnAddAuthor.Name = "btnAddAuthor";
            this.btnAddAuthor.Size = new System.Drawing.Size(100, 23);
            this.btnAddAuthor.TabIndex = 7;
            this.btnAddAuthor.Text = "Добавить автора";
            this.btnAddAuthor.UseVisualStyleBackColor = true;
            this.btnAddAuthor.Click += new System.EventHandler(this.btnAddAuthor_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(442, 20);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 23);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(442, 60);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(100, 23);
            this.btnLoad.TabIndex = 9;
            this.btnLoad.Text = "Загрузить";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnCalculate
            // 
            this.btnCalculate.Location = new System.Drawing.Point(442, 100);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(100, 23);
            this.btnCalculate.TabIndex = 10;
            this.btnCalculate.Text = "Рассчитать";
            this.btnCalculate.UseVisualStyleBackColor = true;
            this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);
            // 
            // lstAuthors
            // 
            this.lstAuthors.FormattingEnabled = true;
            this.lstAuthors.Location = new System.Drawing.Point(120, 300);
            this.lstAuthors.Name = "lstAuthors";
            this.lstAuthors.Size = new System.Drawing.Size(200, 95);
            this.lstAuthors.TabIndex = 11;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(61, 13);
            this.lblTitle.TabIndex = 12;
            this.lblTitle.Text = "Заголовок";
            // 
            // lblPublisher
            // 
            this.lblPublisher.AutoSize = true;
            this.lblPublisher.Location = new System.Drawing.Point(20, 60);
            this.lblPublisher.Name = "lblPublisher";
            this.lblPublisher.Size = new System.Drawing.Size(79, 13);
            this.lblPublisher.TabIndex = 13;
            this.lblPublisher.Text = "Издательство";
            // 
            // lblYear
            // 
            this.lblYear.AutoSize = true;
            this.lblYear.Location = new System.Drawing.Point(20, 100);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(25, 13);
            this.lblYear.TabIndex = 14;
            this.lblYear.Text = "Год";
            // 
            // lblFileSize
            // 
            this.lblFileSize.AutoSize = true;
            this.lblFileSize.Location = new System.Drawing.Point(20, 140);
            this.lblFileSize.Name = "lblFileSize";
            this.lblFileSize.Size = new System.Drawing.Size(81, 13);
            this.lblFileSize.TabIndex = 15;
            this.lblFileSize.Text = "Размер файла";
            // 
            // lblPages
            // 
            this.lblPages.AutoSize = true;
            this.lblPages.Location = new System.Drawing.Point(20, 180);
            this.lblPages.Name = "lblPages";
            this.lblPages.Size = new System.Drawing.Size(85, 13);
            this.lblPages.TabIndex = 16;
            this.lblPages.Text = "Кол-во страниц";
            // 
            // lblCost
            // 
            this.lblCost.AutoSize = true;
            this.lblCost.Location = new System.Drawing.Point(20, 220);
            this.lblCost.Name = "lblCost";
            this.lblCost.Size = new System.Drawing.Size(62, 13);
            this.lblCost.TabIndex = 17;
            this.lblCost.Text = "Стоимость";
            // 
            // lblGenre
            // 
            this.lblGenre.AutoSize = true;
            this.lblGenre.Location = new System.Drawing.Point(20, 260);
            this.lblGenre.Name = "lblGenre";
            this.lblGenre.Size = new System.Drawing.Size(36, 13);
            this.lblGenre.TabIndex = 18;
            this.lblGenre.Text = "Жанр";
            // 
            // lblAuthors
            // 
            this.lblAuthors.AutoSize = true;
            this.lblAuthors.Location = new System.Drawing.Point(20, 300);
            this.lblAuthors.Name = "lblAuthors";
            this.lblAuthors.Size = new System.Drawing.Size(45, 13);
            this.lblAuthors.TabIndex = 19;
            this.lblAuthors.Text = "Авторы";
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(439, 147);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(62, 13);
            this.lblResult.TabIndex = 20;
            this.lblResult.Text = "Результат:";
            // 
            // numFileSize
            // 
            this.numFileSize.Location = new System.Drawing.Point(120, 140);
            this.numFileSize.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numFileSize.Name = "numFileSize";
            this.numFileSize.Size = new System.Drawing.Size(200, 20);
            this.numFileSize.TabIndex = 3;
            // 
            // numPages
            // 
            this.numPages.Location = new System.Drawing.Point(120, 180);
            this.numPages.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numPages.Name = "numPages";
            this.numPages.Size = new System.Drawing.Size(200, 20);
            this.numPages.TabIndex = 4;
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(418, 300);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(420, 23);
            this.progressBar2.TabIndex = 22;
            this.progressBar2.Click += new System.EventHandler(this.progressBar2_Click);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(61, 418);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(187, 17);
            this.radioButton1.TabIndex = 23;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Выберите, если хотите выбрать";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(920, 246);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(394, 278);
            this.pictureBox1.TabIndex = 24;
            this.pictureBox1.TabStop = false;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(595, 12);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(719, 228);
            this.webBrowser1.TabIndex = 25;
            this.webBrowser1.Url = new System.Uri("https://google.com/", System.UriKind.Absolute);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(321, 144);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 26;
            this.label1.Text = "МБ";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1326, 536);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.progressBar2);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.lblAuthors);
            this.Controls.Add(this.lblGenre);
            this.Controls.Add(this.lblCost);
            this.Controls.Add(this.lblPages);
            this.Controls.Add(this.lblFileSize);
            this.Controls.Add(this.lblYear);
            this.Controls.Add(this.lblPublisher);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lstAuthors);
            this.Controls.Add(this.btnCalculate);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnAddAuthor);
            this.Controls.Add(this.cmbGenre);
            this.Controls.Add(this.txtCost);
            this.Controls.Add(this.numPages);
            this.Controls.Add(this.numFileSize);
            this.Controls.Add(this.txtYear);
            this.Controls.Add(this.txtPublisher);
            this.Controls.Add(this.txtTitle);
            this.Name = "Form1";
            this.Text = "Файл книги";
            ((System.ComponentModel.ISupportInitialize)(this.numFileSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPages)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Label label1;
    }
}
