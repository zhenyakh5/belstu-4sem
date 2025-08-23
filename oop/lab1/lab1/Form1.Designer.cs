namespace lab1
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

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
            this.buttonCalculate = new System.Windows.Forms.Button();
            this.labelGender = new System.Windows.Forms.Label();
            this.labelWeight = new System.Windows.Forms.Label();
            this.labelHeight = new System.Windows.Forms.Label();
            this.labelAge = new System.Windows.Forms.Label();
            this.labelGoal = new System.Windows.Forms.Label();
            this.comboBoxGender = new System.Windows.Forms.ComboBox();
            this.textBoxWeight = new System.Windows.Forms.TextBox();
            this.textBoxHeight = new System.Windows.Forms.TextBox();
            this.textBoxAge = new System.Windows.Forms.TextBox();
            this.comboBoxGoal = new System.Windows.Forms.ComboBox();
            this.labelResult = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonCalculate
            // 
            this.buttonCalculate.Location = new System.Drawing.Point(150, 200);
            this.buttonCalculate.Name = "buttonCalculate";
            this.buttonCalculate.Size = new System.Drawing.Size(75, 23);
            this.buttonCalculate.TabIndex = 0;
            this.buttonCalculate.Text = "Рассчитать";
            this.buttonCalculate.UseVisualStyleBackColor = true;
            this.buttonCalculate.Click += new System.EventHandler(this.buttonCalculate_Click);
            // 
            // labelGender
            // 
            this.labelGender.AutoSize = true;
            this.labelGender.Location = new System.Drawing.Point(13, 13);
            this.labelGender.Name = "labelGender";
            this.labelGender.Size = new System.Drawing.Size(30, 13);
            this.labelGender.TabIndex = 1;
            this.labelGender.Text = "Пол:";
            // 
            // labelWeight
            // 
            this.labelWeight.AutoSize = true;
            this.labelWeight.Location = new System.Drawing.Point(13, 40);
            this.labelWeight.Name = "labelWeight";
            this.labelWeight.Size = new System.Drawing.Size(29, 13);
            this.labelWeight.TabIndex = 2;
            this.labelWeight.Text = "Вес:";
            // 
            // labelHeight
            // 
            this.labelHeight.AutoSize = true;
            this.labelHeight.Location = new System.Drawing.Point(13, 67);
            this.labelHeight.Name = "labelHeight";
            this.labelHeight.Size = new System.Drawing.Size(34, 13);
            this.labelHeight.TabIndex = 3;
            this.labelHeight.Text = "Рост:";
            // 
            // labelAge
            // 
            this.labelAge.AutoSize = true;
            this.labelAge.Location = new System.Drawing.Point(13, 94);
            this.labelAge.Name = "labelAge";
            this.labelAge.Size = new System.Drawing.Size(52, 13);
            this.labelAge.TabIndex = 4;
            this.labelAge.Text = "Возраст:";
            // 
            // labelGoal
            // 
            this.labelGoal.AutoSize = true;
            this.labelGoal.Location = new System.Drawing.Point(13, 121);
            this.labelGoal.Name = "labelGoal";
            this.labelGoal.Size = new System.Drawing.Size(36, 13);
            this.labelGoal.TabIndex = 5;
            this.labelGoal.Text = "Цель:";
            // 
            // comboBoxGender
            // 
            this.comboBoxGender.FormattingEnabled = true;
            this.comboBoxGender.Items.AddRange(new object[] {
            "Мужской",
            "Женский"});
            this.comboBoxGender.Location = new System.Drawing.Point(74, 10);
            this.comboBoxGender.Name = "comboBoxGender";
            this.comboBoxGender.Size = new System.Drawing.Size(132, 21);
            this.comboBoxGender.TabIndex = 6;
            // 
            // textBoxWeight
            // 
            this.textBoxWeight.Location = new System.Drawing.Point(74, 37);
            this.textBoxWeight.Name = "textBoxWeight";
            this.textBoxWeight.Size = new System.Drawing.Size(132, 20);
            this.textBoxWeight.TabIndex = 7;
            // 
            // textBoxHeight
            // 
            this.textBoxHeight.Location = new System.Drawing.Point(74, 64);
            this.textBoxHeight.Name = "textBoxHeight";
            this.textBoxHeight.Size = new System.Drawing.Size(132, 20);
            this.textBoxHeight.TabIndex = 8;
            // 
            // textBoxAge
            // 
            this.textBoxAge.Location = new System.Drawing.Point(74, 91);
            this.textBoxAge.Name = "textBoxAge";
            this.textBoxAge.Size = new System.Drawing.Size(132, 20);
            this.textBoxAge.TabIndex = 9;
            // 
            // comboBoxGoal
            // 
            this.comboBoxGoal.FormattingEnabled = true;
            this.comboBoxGoal.Items.AddRange(new object[] {
            "Поддержание веса",
            "Снижение веса",
            "Увеличение веса"});
            this.comboBoxGoal.Location = new System.Drawing.Point(74, 118);
            this.comboBoxGoal.Name = "comboBoxGoal";
            this.comboBoxGoal.Size = new System.Drawing.Size(132, 21);
            this.comboBoxGoal.TabIndex = 10;
            // 
            // labelResult
            // 
            this.labelResult.AutoSize = true;
            this.labelResult.Location = new System.Drawing.Point(13, 170);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(59, 13);
            this.labelResult.TabIndex = 11;
            this.labelResult.Text = "Результат";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(289, 235);
            this.Controls.Add(this.labelResult);
            this.Controls.Add(this.comboBoxGoal);
            this.Controls.Add(this.textBoxAge);
            this.Controls.Add(this.textBoxHeight);
            this.Controls.Add(this.textBoxWeight);
            this.Controls.Add(this.comboBoxGender);
            this.Controls.Add(this.labelGoal);
            this.Controls.Add(this.labelAge);
            this.Controls.Add(this.labelHeight);
            this.Controls.Add(this.labelWeight);
            this.Controls.Add(this.labelGender);
            this.Controls.Add(this.buttonCalculate);
            this.Name = "Form1";
            this.Text = "Калькулятор калорий";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Button buttonCalculate;
        private System.Windows.Forms.Label labelGender;
        private System.Windows.Forms.Label labelWeight;
        private System.Windows.Forms.Label labelHeight;
        private System.Windows.Forms.Label labelAge;
        private System.Windows.Forms.Label labelGoal;
        private System.Windows.Forms.ComboBox comboBoxGender;
        private System.Windows.Forms.TextBox textBoxWeight;
        private System.Windows.Forms.TextBox textBoxHeight;
        private System.Windows.Forms.TextBox textBoxAge;
        private System.Windows.Forms.ComboBox comboBoxGoal;
        private System.Windows.Forms.Label labelResult;
    }
}
