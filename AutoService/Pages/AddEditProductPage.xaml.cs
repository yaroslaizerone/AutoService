using AutoService.Entities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutoService.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditProductPage.xaml
    /// </summary>
    public partial class AddEditProductPage : Page
    {
        Product product = new Product();

        public AddEditProductPage(Product currentProduct)
        {
            InitializeComponent();

            if(currentProduct != null) // Если объект переданный с прошлой страницы не пустой, то добавляем его атрибуты в поля
            {
                product = currentProduct;

                btnDeleteProduct.Visibility = Visibility.Visible; // Показываем кнопку удаления
                textArticle.IsEnabled= false;// Запрещаем редактирование артикула
            }
            DataContext = product;
            cmbCategory.ItemsSource = CategoryList;
        }
        public string[] CategoryList =
        {
           "Аксессуары",
           "Автозапчасти",
           "Автосервис",
           "Съёмники подшипников",
           "Ручные инструменты",
           "Зарядные утройства"
        };

        private void btnEnterImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog GetImageDialog = new OpenFileDialog(); // Открытие диалогового окна

            GetImageDialog.Filter = "Файлы изображений: (*.png,*.jpg,*.jpeg)| *.png;*.jpg;*.jpeg"; // Фильтр типов файлов
            GetImageDialog.InitialDirectory = "F:\\ярослав\\коды\\AutoService\\AutoService\\Resources"; // Путь к папке ресурсов проекта
            if(GetImageDialog.ShowDialog() == true)
                product.ProductPhoto= GetImageDialog.SafeFileName;//Добавление наименования файла в БД
        }

        private void btnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show($"Вы действительно хотите удалить {product.ProductName}?","Внимание", MessageBoxButton.YesNo,MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    AutoserviceEntities.GetContext().Product.Remove(product);
                    AutoserviceEntities.GetContext().SaveChanges();
                    MessageBox.Show("Запись удалена!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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

            if(errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if(product.ProductArticleNumber == null)
                AutoserviceEntities.GetContext().Product.Add(product);// Добавление объекта в БД
            try
            {
                AutoserviceEntities.GetContext().SaveChanges();//Сохранение в БД
                MessageBox.Show("Информация сохранена!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); //  Вывод ошибки
            }
        }
    }
}
