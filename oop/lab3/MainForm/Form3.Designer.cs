namespace lab2
{
    partial class SearchForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtPattern;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblPattern;

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
            this.txtPattern = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblPattern = new System.Windows.Forms.Label();
            this.SuspendLayout();
            this.txtPattern.Location = new System.Drawing.Point(12, 30);
            this.txtPattern.Name = "txtPattern";
            this.txtPattern.Size = new System.Drawing.Size(260, 20);
            this.txtPattern.TabIndex = 0;
            this.btnSearch.Location = new System.Drawing.Point(197, 60);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "Поиск";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            this.lblPattern.AutoSize = true;
            this.lblPattern.Location = new System.Drawing.Point(12, 13);
            this.lblPattern.Name = "lblPattern";
            this.lblPattern.Size = new System.Drawing.Size(124, 13);
            this.lblPattern.TabIndex = 2;
            this.lblPattern.Text = "Введите ваш запрос или регулярное выражение";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 95);
            this.Controls.Add(this.lblPattern);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtPattern);
            this.Name = "SearchForm";
            this.Text = "Поиск";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
