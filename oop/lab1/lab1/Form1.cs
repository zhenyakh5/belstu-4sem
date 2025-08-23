using System;
using System.Windows.Forms;

namespace lab1
{
    public partial class Form1 : Form
    {
        private Calculator calculator;

        public Form1()
        {
            InitializeComponent();
            calculator = new Calculator();
            calculator.CalculationCompleted += Calculator_CalculationCompleted;
        }

        private void Calculator_CalculationCompleted(object sender, CalculationEventArgs e)
        {
            labelResult.Text = e.Result;
        }

        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxGender.SelectedItem == null ||
                    string.IsNullOrWhiteSpace(textBoxWeight.Text) ||
                    string.IsNullOrWhiteSpace(textBoxHeight.Text) ||
                    string.IsNullOrWhiteSpace(textBoxAge.Text) ||
                    comboBoxGoal.SelectedItem == null)
                {
                    throw new Exception("Все поля должны быть заполнены.");
                }

                string gender = comboBoxGender.SelectedItem.ToString();
                double weight;
                double height;
                int age;

                if (!double.TryParse(textBoxWeight.Text, out weight) ||
                    !double.TryParse(textBoxHeight.Text, out height) ||
                    !int.TryParse(textBoxAge.Text, out age))
                {
                    throw new Exception("Вес, рост и возраст должны быть корректными числовыми значениями.");
                }

                if (weight <= 0 || height <= 0 || age <= 0)
                {
                    throw new Exception("Вес, рост и возраст должны быть положительными значениями.");
                }

                string goal = comboBoxGoal.SelectedItem.ToString();

                calculator.CalculateCalories(gender, weight, height, age, goal);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }

    interface ICalculator
    {
        void CalculateCalories(string gender, double weight, double height, int age, string goal);
        string DetermineCondition(double bmi);
        void OnCalculationCompleted(CalculationEventArgs e);
    }

    public class Calculator : ICalculator
    {
        public event EventHandler<CalculationEventArgs> CalculationCompleted;

        public void CalculateCalories(string gender, double weight, double height, int age, string goal)
        {
            double bmr = 0;
            if (gender.ToLower() == "мужской")
            {
                bmr = 88.36 + (13.4 * weight) + (4.8 * height) - (5.7 * age);
            }
            else
            {
                bmr = 447.6 + (9.2 * weight) + (3.1 * height) - (4.3 * age);
            }

            double dailyCalories = bmr;
            if (goal.ToLower() == "снижение веса")
            {
                dailyCalories -= 500;
            }
            else if (goal.ToLower() == "увеличение веса")
            {
                dailyCalories += 500;
            }

            double heightInMeters = height / 100;
            double bmi = weight / (heightInMeters * heightInMeters);
            string condition = DetermineCondition(bmi);

            string result = $"Суточная потребность в калориях: {dailyCalories} ккал\nСостояние: {condition}";
            OnCalculationCompleted(new CalculationEventArgs(result));
        }

        public string DetermineCondition(double bmi)
        {
            if (bmi < 18.5)
            {
                return "Недостаток веса";
            }
            else if (bmi < 25)
            {
                return "Нормальный вес";
            }
            else if (bmi < 30)
            {
                return "Избыточный вес";
            }
            else
            {
                return "Ожирение";
            }
        }

        public void OnCalculationCompleted(CalculationEventArgs e)
        {
            CalculationCompleted?.Invoke(this, e);
        }
    }

    public class CalculationEventArgs : EventArgs
    {
        public string Result { get; }

        public CalculationEventArgs(string result)
        {
            Result = result;
        }
    }
}
