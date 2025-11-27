using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LB4_Course2
{
    public partial class Task2 : Window
    {
        private const double PriceA95 = 55.50;
        private const double PriceA92 = 53.00;
        private const double PriceDiesel = 54.50;
        private const double PriceGas = 28.00;

        private const double PriceHotDog = 45.00;
        private const double PriceHamburger = 60.00;
        private const double PriceFries = 35.00;
        private const double PriceCocaCola = 25.00;

        public Task2()
        {
            InitializeComponent(); 
            
        }
        
        private void InitializePrices()
        {
            // Встановлення цін для кафе
            HotDogPriceTextBlock.Text = PriceHotDog.ToString("F2", CultureInfo.InvariantCulture);
            HamburgerPriceTextBlock.Text = PriceHamburger.ToString("F2", CultureInfo.InvariantCulture);
            FriesPriceTextBlock.Text = PriceFries.ToString("F2", CultureInfo.InvariantCulture);
            CocaColaPriceTextBlock.Text = PriceCocaCola.ToString("F2", CultureInfo.InvariantCulture);
        }

        private void Fuel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            double currentPrice = 0.0;
            string fuelType = string.Empty;

            // 1. Безпечно отримуємо обраний елемент
            ComboBoxItem selectedItem = FuelComboBox.SelectedItem as ComboBoxItem;

            if (selectedItem != null)
            {
                // ВИПРАВЛЕНО: Використання ?. для безпечного доступу до Content
                fuelType = selectedItem.Content?.ToString() ?? string.Empty;
            }
            else
            {
                // 2. АЛЬТЕРНАТИВА: Якщо SelectedItem null (під час запуску),
                // використовуємо перший елемент як значення за замовчуванням.
                if (FuelComboBox.Items.Count > 0 && FuelComboBox.Items[0] is ComboBoxItem firstItem)
                {
                    // ВИПРАВЛЕНО: Використання ?. для безпечного доступу до Content
                    fuelType = firstItem.Content?.ToString() ?? string.Empty;
                }
                else
                {
                    // Якщо немає елементів, зупиняємо виконання
                    return; 
                }
            }

            // 3. Логіка встановлення ціни
            switch (fuelType)
            {
                case "A-95":
                    currentPrice = PriceA95;
                    break;
                case "A-92":
                    currentPrice = PriceA92;
                    break;
                case "Дизель":
                    currentPrice = PriceDiesel;
                    break;
                case "Газ":
                    currentPrice = PriceGas;
                    break;
                default:
                    currentPrice = 0.0;
                    break;
            }

            // Відображення ціни
            FuelPriceTextBlock.Text = currentPrice.ToString("F2", CultureInfo.InvariantCulture); 
            CalculateFuelTotal();
        }

        private void FuelMode_Checked(object sender, RoutedEventArgs e)
        {
            if (AmountRadioButton == null || SumRadioButton == null) return;
            // Вмикаємо/вимикаємо поля та очищаємо невикористовуване поле
            if (AmountRadioButton.IsChecked == true)
            {
                AmountTextBox.IsEnabled = true;
                SumTextBox.IsEnabled = false;
                SumTextBox.Text = "0";
            }
            else if (SumRadioButton.IsChecked == true)
            {
                AmountTextBox.IsEnabled = false;
                SumTextBox.IsEnabled = true;
                AmountTextBox.Text = "0";
            }
            CalculateFuelTotal();
        }

        private void FuelInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalculateFuelTotal();
        }

        private void CalculateFuelTotal()
        {
            double currentPrice = 0.0;
            double total = 0.0;

            // Безпечно отримуємо ціну
            if (!double.TryParse(FuelPriceTextBlock.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out currentPrice))
            {
                currentPrice = 0.0;
            }
            
            // Розрахунок залежно від режиму
            if (AmountRadioButton.IsChecked == true)
            {
                // ВАЖЛИВО: Очищаємо текст, якщо він не є числом, щоб уникнути помилок TryParse
                if (string.IsNullOrEmpty(AmountTextBox.Text)) AmountTextBox.Text = "0";
                
                if (double.TryParse(AmountTextBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double amount))
                {
                    total = amount * currentPrice;
                }
            }
            else if (SumRadioButton.IsChecked == true)
            {
                if (string.IsNullOrEmpty(SumTextBox.Text)) SumTextBox.Text = "0";
                
                if (double.TryParse(SumTextBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double sum))
                {
                    total = sum;
                }
            }
            
            FuelTotalTextBlock.Text = total.ToString("F2", CultureInfo.InvariantCulture);
        }

        private void Cafe_Update(object sender, RoutedEventArgs e)
        {
            CalculateCafeTotal();
        }

        private void CalculateCafeTotal()
        {
            double total = 0.0;
    
            // Helper function for safe parsing
            // !!! ЗМІНА: Додано перевірку на null для TextBox
            bool TryGetQuantity(TextBox textBox, out double quantity)
            {
                if (textBox == null) 
                {
                    quantity = 0;
                    return false; // Якщо елемент TextBox ще не ініціалізовано
                }
        
                // Забезпечуємо, що поле не пусте
                if (string.IsNullOrEmpty(textBox.Text)) textBox.Text = "0";
        
                return double.TryParse(textBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out quantity);
            }

            // Хот-дог
            // !!! ЗМІНА: Додано перевірку на null для CheckBox перед його використанням
            if (HotDogCheckBox != null && HotDogCheckBox.IsChecked == true && TryGetQuantity(HotDogQuantityTextBox, out double q1))
            {
                total += PriceHotDog * q1;
            }

            // Гамбургер
            // !!! ЗМІНА: Додано перевірку на null для CheckBox
            if (HamburgerCheckBox != null && HamburgerCheckBox.IsChecked == true && TryGetQuantity(HamburgerQuantityTextBox, out double q2))
            {
                total += PriceHamburger * q2;
            }

            // Картопля-фрі
            // !!! ЗМІНА: Додано перевірку на null для CheckBox
            if (FriesCheckBox != null && FriesCheckBox.IsChecked == true && TryGetQuantity(FriesQuantityTextBox, out double q3))
            {
                total += PriceFries * q3;
            }

            // Кока-кола
            // !!! ЗМІНА: Додано перевірку на null для CheckBox
            if (CocaColaCheckBox != null && CocaColaCheckBox.IsChecked == true && TryGetQuantity(CocaColaQuantityTextBox, out double q4))
            {
                total += PriceCocaCola * q4;
            }

            // !!! ЗМІНА: Додано перевірку на null для CafeTotalTextBlock
            if (CafeTotalTextBlock != null)
            {
                CafeTotalTextBlock.Text = total.ToString("F2", CultureInfo.InvariantCulture);
            }
        }
        
        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            CalculateFuelTotal();
            CalculateCafeTotal();
            UpdateGrandTotal();
        }
        
        private void UpdateGrandTotal()
        {
            double fuelTotal = 0.0;
            double cafeTotal = 0.0;

            // 1. Безпечне отримання підсумків з проміжних текстових блоків
            // (Ми припускаємо, що FuelTotalTextBlock та CafeTotalTextBlock вже захищені 
            // або їхні контейнери, як і GrandTotalTextBlock, ще не ініціалізовані)

            if (FuelTotalTextBlock != null && !double.TryParse(FuelTotalTextBlock.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out fuelTotal))
            {
                fuelTotal = 0.0;
            }

            if (CafeTotalTextBlock != null && !double.TryParse(CafeTotalTextBlock.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out cafeTotal))
            {
                cafeTotal = 0.0;
            }

            double grandTotal = fuelTotal + cafeTotal;

            // 2. !!! ФІНАЛЬНЕ ВИПРАВЛЕННЯ !!!
            // Перевіряємо, чи ініціалізовано GrandTotalTextBlock перед встановленням тексту.
            if (GrandTotalTextBlock != null)
            {
                GrandTotalTextBlock.Text = grandTotal.ToString("F2", CultureInfo.InvariantCulture);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializePrices();    
    
            // 1. Динамічно приєднуємо обробник SelectionChanged. 
            // Це найважливіше виправлення, яке гарантує, що логіка спрацює, 
            // коли FuelComboBox буде повністю завантажений.
            FuelComboBox.SelectionChanged += Fuel_SelectionChanged;

            // 2. Викликаємо перший розрахунок для встановлення початкової ціни (A-95).
            Fuel_SelectionChanged(null, null);
    
            // Запобігаємо помилці, якщо AmountTextBox пустий на старті
            if (AmountTextBox.Text == string.Empty) AmountTextBox.Text = "0";
        }
    }
}


