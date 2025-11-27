using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LB4_Course2;


public partial class MainWindow : Window
{
    public Task1 task1;
    public Task2 task2;
    public MainWindow()
    {
        InitializeComponent();
    }

    private void ButtonBase_OnClick1(object sender, RoutedEventArgs e)
    {
        if (task1 == null)
        {
            task1 = new Task1();
            task1.Show();
        }
    }
    private void ButtonBase_OnClick2(object sender, RoutedEventArgs e)
    {
        if (task2 == null)
        {
            task2 = new Task2();
            task2.Show();
        }
    }
}