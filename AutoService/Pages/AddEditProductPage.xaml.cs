using AutoService.Entities;
using Microsoft.Win32;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace AutoService.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditProductPage.xaml
    /// </summary>
    public partial class AddEditProductPage : Page
    {
        Product product = new Product();
        Product product1 = new Product();

        public AddEditProductPage(Product currentProduct)
        {
            InitializeComponent();

            cmbCategory.ItemsSource = AutoEntities.GetContext().ProductCategors.ToList();
            cmbUnit.ItemsSource = AutoEntities.GetContext().Units.ToList();
            cmbSupplier.ItemsSource = AutoEntities.GetContext().Suppliers.ToList();
            cmbManufacture.ItemsSource = AutoEntities.GetContext().Manufacture.ToList();

            if (currentProduct != null) // Если объект переданный с прошлой страницы не пустой, то добавляем его атрибуты в поля
            {

                product = currentProduct;
                cmbCategory.SelectedIndex = product.IDProductCategory - 1;
                cmbUnit.SelectedIndex = product.IDUnit - 1;
                cmbSupplier.SelectedIndex = product.IDSupplier - 1;
                cmbManufacture.SelectedIndex = product.IDManufacture - 1;
                btnDeleteProduct.Visibility = Visibility.Visible; // Показываем кнопку удаления
                textArticle.IsEnabled = false;// Запрещаем редактирование артикула
            }
            else if (currentProduct == null)
            {
                product1 = null;
            }
            DataContext = product;
        }

        private void btnEnterImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog GetImageDialog = new OpenFileDialog(); // Открытие диалогового окна

            GetImageDialog.Filter = "Файлы изображений: (*.png,*.jpg,*.jpeg)| *.png;*.jpg;*.jpeg"; // Фильтр типов файлов
            GetImageDialog.InitialDirectory = "F:\\ярослав\\коды\\AutoService\\AutoService\\Resources"; // Путь к папке ресурсов проекта
            if (GetImageDialog.ShowDialog() == true)
                product.ProductPhoto = GetImageDialog.SafeFileName;//Добавление наименования файла в БД
                DataContext = null;
                DataContext = product;   
        }

        private void btnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Вы действительно хотите удалить {product.ProductName}?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    AutoEntities.GetContext().Product.Remove(product);
                    AutoEntities.GetContext().SaveChanges();
                    MessageBox.Show("Запись удалена!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                NavigationService.GoBack();
            }
        }

        private void btnSaveProduct_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (product.ProductCost < 0)
                errors.AppendLine("Стоимость не может быть отрицательной!");
            if (product.MinCount < 0)
                errors.AppendLine("Минимальное количество не может быть отрицательным!");
            if (product.ProductDiscountAmount > product.MaxDiscountAmount)
                errors.AppendLine("Действующая скидка товара не может быть больше максимальной скидки!");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            product.ProductArticleNumber = textArticle.Text;
            product.ProductName = textTitle.Text;
            product.IDProductCategory = cmbCategory.SelectedIndex + 1;
            product.ProductQuantityInStock = Int32.Parse(textCountInStock.Text);
            product.IDUnit = cmbUnit.SelectedIndex + 1;
            if (textCountInPack.Text != "")
            {
                product.CountPack = Int32.Parse(textCountInPack.Text);
            }
            if (textMinCount.Text != "")
            {
                product.MinCount = Int32.Parse(textMinCount.Text);
            }
            product.IDSupplier = cmbSupplier.SelectedIndex + 1;
            product.IDManufacture = cmbManufacture.SelectedIndex + 1;
            product.MaxDiscountAmount = Int32.Parse(textMaxDiscount.Text);
            product.ProductDiscountAmount = Byte.Parse(textDiscount.Text);
            product.ProductCost = decimal.Parse(textCost.Text, CultureInfo.InvariantCulture);
            product.ProductDescription = textDescription.Text;

            if (product1 == null)
                AutoEntities.GetContext().Product.Add(product);// Добавление объекта в БД
            try
            {
                AutoEntities.GetContext().SaveChanges();//Сохранение в БД
                MessageBox.Show("Информация сохранена!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); //  Вывод ошибки
            }
        }

        private void textMaxDiscount_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void textCountInStock_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void textCountInPack_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void textMinCount_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void textDiscount_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void textCost_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
        private static readonly Regex _regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }
    }
}