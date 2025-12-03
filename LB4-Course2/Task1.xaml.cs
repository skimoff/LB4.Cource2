using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace LB4_Course2;

public partial class Task1 : Window
{
    private const string BinaryOperators = "+-*/";
    private const string DecimalSeparator = ".";
    
    public Task1()
    {
        InitializeComponent();
        
        MainRoot.AddHandler(Button.ClickEvent, new RoutedEventHandler(ButtonClick));
        Result.Text = "0";
    }

    private void ButtonClick(object sender, RoutedEventArgs e)
    {
        if(e.OriginalSource is not Button button) return;
        string? input = ((Button)e.OriginalSource).Content.ToString();
        if(input == null) return;
        
        string current = Result.Text;

        if(input is "CE" or "C" or "Del" or "=") return;
        
        if (current == "0" && char.IsDigit(input[0]))
        {
            Result.Text = input;
            return;
        }
        
        char lastChar = current.Last();
        
        if (BinaryOperators.Contains(input))
        {
            // Не позволяем начинать выражение с оператора, кроме минуса
            if (string.IsNullOrEmpty(current) || current == "0")
            {
                if (input == "-")
                {
                    Result.Text = input;
                }
                return;
            }
            
            // Заменяем предыдущий оператор, если он есть
            if (BinaryOperators.Contains(lastChar))
            {
                Result.Text = current.Remove(current.Length - 1) + input;
                return;
            }
            
            // Не позволяем вводить оператор сразу после точки (например, 5.+ не сработает)
            if (lastChar.ToString() == DecimalSeparator)
            {
                // Заменяем точку на оператор, если это возможно, или ничего не делаем
                return;
            }

            // Добавляем оператор
            Result.Text += input;
            return;
        }
        
        if (input[0].ToString() == DecimalSeparator)
        {
            // Проверка: не добавлять точку, если она уже есть в текущем числе
            // Находим позицию последнего оператора
            int lastOperatorIndex = current.LastIndexOfAny(BinaryOperators.ToCharArray());
            
            // Если операторов нет, ищем точку с начала строки.
            // Если операторы есть, ищем точку после последнего оператора.
            string currentOperand = (lastOperatorIndex == -1) ? current : current.Substring(lastOperatorIndex + 1);
            
            if (currentOperand.Contains(DecimalSeparator))
            {
                return; // Точка уже есть в текущем операнде
            }
            
            // Если строка пуста или последний символ - оператор, добавляем "0."
            if (string.IsNullOrEmpty(current) || BinaryOperators.Contains(current.Last()))
            {
                Result.Text += "0.";
            }
            else
            {
                Result.Text += input;
            }
            return;
        }
        
        Result.Text += input;
    }
    
    private void Button_Calculator_Click(object sender, RoutedEventArgs e)
    {
        // Убираем потенциальный оператор в конце выражения перед вычислением
        string expression = Result.Text.Trim();
        if (expression.Length > 0 && BinaryOperators.Contains(expression.Last()))
        {
            expression = expression.Remove(expression.Length - 1);
        }

        try
        {
            if (!string.IsNullOrWhiteSpace(expression))
            {
                // Используем Data.DataTable для простого вычисления выражения
                string result = new DataTable().Compute(expression, null).ToString();
                
                History.Text = expression + " =";
                Result.Text = result;
            }
        }
        catch
        {
            History.Text = expression;
            Result.Text = "Error";
        }
    }
    

    private void Button_CE_Click(object sender, RoutedEventArgs e)
    {
        Result.Text = "0";
    }
    private void Button_C_Click(object sender, RoutedEventArgs e)
    {
        Result.Text = "0";
        History.Text = "";
    }
    private void Button_Del_Click(object sender, RoutedEventArgs e)
    {
        if (Result.Text == "0" || string.IsNullOrEmpty(Result.Text)) return;
        
        if (Result.Text.Length > 1)
        {
            Result.Text = Result.Text.Substring(0, Result.Text.Length - 1);
        }
        else
        {
            // Если остался один символ (или удалили последний), ставим "0"
            Result.Text = "0";
        }
    }
}