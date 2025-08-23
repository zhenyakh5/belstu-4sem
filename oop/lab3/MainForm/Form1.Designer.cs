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
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem searchMenu;
        private System.Windows.Forms.ToolStripMenuItem regexSearchMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exactSearchMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sortMenu;
        private System.Windows.Forms.ToolStripMenuItem sortByNameMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sortByCountryMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutMenu;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripSearchButton;
        private System.Windows.Forms.ToolStripButton toolStripSortButton;
        private System.Windows.Forms.ToolStripButton toolStripClearButton;
        private System.Windows.Forms.ToolStripButton toolStripDeleteButton;
        private System.Windows.Forms.ToolStripButton toolStripForwardButton;
        private System.Windows.Forms.ToolStripButton toolStripBackButton;
        private System.Windows.Forms.ToolStripButton toolStripToggleToolbarButton;

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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.searchMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.regexSearchMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exactSearchMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.sortByNameMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortByCountryMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSearchButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSortButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripClearButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripDeleteButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripForwardButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripBackButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripToggleToolbarButton = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.numFileSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPages)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            this.txtTitle.Location = new System.Drawing.Point(119, 62);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(200, 20);
            this.txtTitle.TabIndex = 0;
            this.txtPublisher.Location = new System.Drawing.Point(119, 102);
            this.txtPublisher.Name = "txtPublisher";
            this.txtPublisher.Size = new System.Drawing.Size(200, 20);
            this.txtPublisher.TabIndex = 1;
            this.txtYear.Location = new System.Drawing.Point(119, 142);
            this.txtYear.Name = "txtYear";
            this.txtYear.Size = new System.Drawing.Size(200, 20);
            this.txtYear.TabIndex = 2;
            this.txtCost.Location = new System.Drawing.Point(119, 262);
            this.txtCost.Name = "txtCost";
            this.txtCost.Size = new System.Drawing.Size(200, 20);
            this.txtCost.TabIndex = 5;
            this.cmbGenre.FormattingEnabled = true;
            this.cmbGenre.Items.AddRange(new object[] {
            "Художественная",
            "Нон-фикшн",
            "Наука",
            "История"});
            this.cmbGenre.Location = new System.Drawing.Point(119, 302);
            this.cmbGenre.Name = "cmbGenre";
            this.cmbGenre.Size = new System.Drawing.Size(200, 21);
            this.cmbGenre.TabIndex = 6;
            this.btnAddAuthor.Location = new System.Drawing.Point(9, 381);
            this.btnAddAuthor.Name = "btnAddAuthor";
            this.btnAddAuthor.Size = new System.Drawing.Size(100, 23);
            this.btnAddAuthor.TabIndex = 7;
            this.btnAddAuthor.Text = "Добавить автора";
            this.btnAddAuthor.UseVisualStyleBackColor = true;
            this.btnAddAuthor.Click += new System.EventHandler(this.btnAddAuthor_Click);
            this.btnSave.Location = new System.Drawing.Point(441, 62);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 23);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.btnLoad.Location = new System.Drawing.Point(441, 102);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(100, 23);
            this.btnLoad.TabIndex = 9;
            this.btnLoad.Text = "Загрузить";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            this.btnCalculate.Location = new System.Drawing.Point(441, 142);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(100, 23);
            this.btnCalculate.TabIndex = 10;
            this.btnCalculate.Text = "Рассчитать";
            this.btnCalculate.UseVisualStyleBackColor = true;
            this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);
            this.lstAuthors.FormattingEnabled = true;
            this.lstAuthors.Location = new System.Drawing.Point(119, 342);
            this.lstAuthors.Name = "lstAuthors";
            this.lstAuthors.Size = new System.Drawing.Size(200, 95);
            this.lstAuthors.TabIndex = 11;
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(19, 62);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(61, 13);
            this.lblTitle.TabIndex = 12;
            this.lblTitle.Text = "Заголовок";
            this.lblPublisher.AutoSize = true;
            this.lblPublisher.Location = new System.Drawing.Point(19, 102);
            this.lblPublisher.Name = "lblPublisher";
            this.lblPublisher.Size = new System.Drawing.Size(79, 13);
            this.lblPublisher.TabIndex = 13;
            this.lblPublisher.Text = "Издательство";
            this.lblYear.AutoSize = true;
            this.lblYear.Location = new System.Drawing.Point(19, 142);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(25, 13);
            this.lblYear.TabIndex = 14;
            this.lblYear.Text = "Год";
            this.lblFileSize.AutoSize = true;
            this.lblFileSize.Location = new System.Drawing.Point(19, 182);
            this.lblFileSize.Name = "lblFileSize";
            this.lblFileSize.Size = new System.Drawing.Size(81, 13);
            this.lblFileSize.TabIndex = 15;
            this.lblFileSize.Text = "Размер файла";
            this.lblPages.AutoSize = true;
            this.lblPages.Location = new System.Drawing.Point(19, 222);
            this.lblPages.Name = "lblPages";
            this.lblPages.Size = new System.Drawing.Size(85, 13);
            this.lblPages.TabIndex = 16;
            this.lblPages.Text = "Кол-во страниц";
            this.lblCost.AutoSize = true;
            this.lblCost.Location = new System.Drawing.Point(19, 262);
            this.lblCost.Name = "lblCost";
            this.lblCost.Size = new System.Drawing.Size(62, 13);
            this.lblCost.TabIndex = 17;
            this.lblCost.Text = "Стоимость";
            this.lblGenre.AutoSize = true;
            this.lblGenre.Location = new System.Drawing.Point(19, 302);
            this.lblGenre.Name = "lblGenre";
            this.lblGenre.Size = new System.Drawing.Size(36, 13);
            this.lblGenre.TabIndex = 18;
            this.lblGenre.Text = "Жанр";
            this.lblAuthors.AutoSize = true;
            this.lblAuthors.Location = new System.Drawing.Point(19, 342);
            this.lblAuthors.Name = "lblAuthors";
            this.lblAuthors.Size = new System.Drawing.Size(45, 13);
            this.lblAuthors.TabIndex = 19;
            this.lblAuthors.Text = "Авторы";
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(438, 189);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(62, 13);
            this.lblResult.TabIndex = 20;
            this.lblResult.Text = "Результат:";
            this.numFileSize.Location = new System.Drawing.Point(119, 182);
            this.numFileSize.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numFileSize.Name = "numFileSize";
            this.numFileSize.Size = new System.Drawing.Size(200, 20);
            this.numFileSize.TabIndex = 3;
            this.numPages.Location = new System.Drawing.Point(119, 222);
            this.numPages.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numPages.Name = "numPages";
            this.numPages.Size = new System.Drawing.Size(200, 20);
            this.numPages.TabIndex = 4;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.searchMenu,
            this.sortMenu,
            this.aboutMenu});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(677, 24);
            this.menuStrip1.TabIndex = 21;
            this.menuStrip1.Text = "menuStrip1";
            this.searchMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.regexSearchMenuItem,
            this.exactSearchMenuItem});
            this.searchMenu.Name = "searchMenu";
            this.searchMenu.Size = new System.Drawing.Size(54, 20);
            this.searchMenu.Text = "Поиск";
            this.regexSearchMenuItem.Name = "regexSearchMenuItem";
            this.regexSearchMenuItem.Size = new System.Drawing.Size(273, 22);
            this.regexSearchMenuItem.Text = "Поиск по регулярным выражениям";
            this.regexSearchMenuItem.Click += new System.EventHandler(this.RegexSearchMenuItem_Click);
            this.exactSearchMenuItem.Name = "exactSearchMenuItem";
            this.exactSearchMenuItem.Size = new System.Drawing.Size(273, 22);
            this.exactSearchMenuItem.Text = "Поиск по полному совпадению";
            this.exactSearchMenuItem.Click += new System.EventHandler(this.ExactSearchMenuItem_Click);
            this.sortMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sortByNameMenuItem,
            this.sortByCountryMenuItem});
            this.sortMenu.Name = "sortMenu";
            this.sortMenu.Size = new System.Drawing.Size(102, 20);
            this.sortMenu.Text = "Сортировка по";
            this.sortByNameMenuItem.Name = "sortByNameMenuItem";
            this.sortByNameMenuItem.Size = new System.Drawing.Size(112, 22);
            this.sortByNameMenuItem.Text = "Имени";
            this.sortByNameMenuItem.Click += new System.EventHandler(this.SortByNameMenuItem_Click);
            this.sortByCountryMenuItem.Name = "sortByCountryMenuItem";
            this.sortByCountryMenuItem.Size = new System.Drawing.Size(112, 22);
            this.sortByCountryMenuItem.Text = "Стране";
            this.sortByCountryMenuItem.Click += new System.EventHandler(this.SortByCountryMenuItem_Click);
            this.aboutMenu.Name = "aboutMenu";
            this.aboutMenu.Size = new System.Drawing.Size(94, 20);
            this.aboutMenu.Text = "О программе";
            this.aboutMenu.Click += new System.EventHandler(this.AboutMenu_Click);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 444);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(677, 22);
            this.statusStrip1.TabIndex = 23;
            this.statusStrip1.Text = "statusStrip1";
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(45, 17);
            this.statusLabel.Text = "Готово";
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSearchButton,
            this.toolStripSortButton,
            this.toolStripClearButton,
            this.toolStripDeleteButton,
            this.toolStripForwardButton,
            this.toolStripBackButton,
            this.toolStripToggleToolbarButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(677, 25);
            this.toolStrip1.TabIndex = 24;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStripSearchButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSearchButton.Name = "toolStripSearchButton";
            this.toolStripSearchButton.Size = new System.Drawing.Size(48, 22);
            this.toolStripSearchButton.Text = "🔍 Поиск";
            this.toolStripSearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            this.toolStripSortButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSortButton.Name = "toolStripSortButton";
            this.toolStripSortButton.Size = new System.Drawing.Size(66, 22);
            this.toolStripSortButton.Text = "🔄 Сортировка";
            this.toolStripSortButton.Click += new System.EventHandler(this.SortByNameMenuItem_Click);
            this.toolStripClearButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripClearButton.Name = "toolStripClearButton";
            this.toolStripClearButton.Size = new System.Drawing.Size(56, 22);
            this.toolStripClearButton.Text = "🗑 Очистить";
            this.toolStripClearButton.Click += new System.EventHandler(this.toolStripClearButton_Click);
            this.toolStripDeleteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDeleteButton.Name = "toolStripDeleteButton";
            this.toolStripDeleteButton.Size = new System.Drawing.Size(56, 22);
            this.toolStripDeleteButton.Text = "❌ Удалить";
            this.toolStripDeleteButton.Click += new System.EventHandler(this.toolStripDeleteButton_Click);
            this.toolStripForwardButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripForwardButton.Name = "toolStripForwardButton";
            this.toolStripForwardButton.Size = new System.Drawing.Size(56, 22);
            this.toolStripForwardButton.Text = "⏩ Вперед";
            this.toolStripForwardButton.Click += new System.EventHandler(this.toolStripForwardButton_Click);
            this.toolStripBackButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripBackButton.Name = "toolStripBackButton";
            this.toolStripBackButton.Size = new System.Drawing.Size(48, 22);
            this.toolStripBackButton.Text = "⏪ Назад";
            this.toolStripBackButton.Click += new System.EventHandler(this.toolStripBackButton_Click);
            this.toolStripToggleToolbarButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripToggleToolbarButton.Name = "toolStripToggleToolbarButton";
            this.toolStripToggleToolbarButton.Size = new System.Drawing.Size(108, 22);
            this.toolStripToggleToolbarButton.Text = "🔄 Скрыть";
            this.toolStripToggleToolbarButton.Click += new System.EventHandler(this.toolStripToggleToolbarButton_Click);
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(677, 466);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
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
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Файл книги";
            ((System.ComponentModel.ISupportInitialize)(this.numFileSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPages)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
