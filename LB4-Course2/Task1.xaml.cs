using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace LB4_Course2;

public partial class Task1 : Window
{
    private static readonly char[] Operators = { '+', '-', '*', '/', '.'};
    public Task1()
    {
        InitializeComponent();
        
        MainRoot.AddHandler(Button.ClickEvent, new RoutedEventHandler(ButtonClick));
    }

    private void ButtonClick(object sender, RoutedEventArgs e)
    {
        string? input = ((Button)e.OriginalSource).Content.ToString();
        string current = Result.Text;

        if (input != null && Operators.Contains(input[0]))
        {
            if(string.IsNullOrEmpty(current)&& input != "-")return;
            if(!string.IsNullOrEmpty(current)&& Operators.Contains(current.Last()))return;
        }

        if (input is not ("CE" or "C" or "Del"))
        {
            Result.Text += input;
        }
    }
    
    private void Button_Calculator_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(Result.Text))
            {
                History.Text = Result.Text;
                Result.Text = new DataTable().Compute(Result.Text, null).ToString();
            }
        }
        catch
        {
            Result.Text = "Error";
        }
    }
    

    private void Button_CE_Click(object sender, RoutedEventArgs e)
    {
        Result.Text = "";
        History.Text = "";
    }
    private void Button_C_Click(object sender, RoutedEventArgs e)
    {
        Result.Text = History.Text;
        History.Text = "";
    }
    private void Button_Del_Click(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(Result.Text))
            Result.Text = Result.Text.Substring(0, Result.Text.Length - 1);
    }
}