using AutoService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using static AutoService.Entities.Product;

namespace AutoService.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrderPage.xaml
    /// </summary>
    public partial class OrderPage : Page
    {
        OrderedProduct orderedProduct = new OrderedProduct();
        List<Product> productList = new List<Product>();
        public OrderPage(List<Product> products, User user)
        {
            InitializeComponent();
            DataContext = this; // Привязываем контекст к данному классу
            productList = products; // Передача списка товаров в пустой лист
            lViewOrder.ItemsSource = productList; // Вывод товаров в ListView
            cmbPickupPouint.ItemsSource = AutoEntities.GetContext().PickupPoint.ToList();//Ввод пунктов выдачи в список

            if (user != null) // Проверка на наличие пользователя
                textUser.Text = user.UserSurname.ToString() + " " + user.UserName.ToString() + " " + user.UserPatronymic.ToString();
        } 
        public string Total
        {
            get
            {
                var total = productList.Sum(x => Convert.ToDouble(x.ProductCost) - Convert.ToDouble(x.ProductCost) * Convert.ToDouble(x.ProductDiscountAmount / 100.00));
                return total.ToString();
            }
        }
        
        public int CountPoroductOrdered
        {
            get
            {
                return 1;
            }
        }

        private void btnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {//TODO Исправить удаление товара из заказа
            if (MessageBox.Show("Вы уверены, что хотите удалить этот элемент?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                var selected = lViewOrder.SelectedItems.Cast<Product>().ToArray();
                foreach (var item in selected)
                {
                    productList.Remove(item);
                }
                lViewOrder.ItemsSource = null;
                lViewOrder.ItemsSource = productList;
                textUser.Text = "";
                ResultCost.Text = $" {Total.ToString()} рублей";
            }
        }

        private void btnOrderSave_Click(object sender, RoutedEventArgs e)
        {  //TODO Сделать проверку на наличие количества товара
            var productArticle = productList.Select(x => x.ProductArticleNumber).ToArray(); // Производим поиск товаров по артикулу, добавляя каждый отдельным элементом массива 
            Random random = new Random(); //Для случайного числа в коде получения
            var date = DateTime.Now; //Добавляем переменную, в которой хранится сегодняшняя дата
            if (productList.Any(x => x.ProductQuantityInStock < 3))
                date = date.AddDays(6);
            else                        //В зависимости от количества товара
                date = date.AddDays(3); //назначется время доставки
            
            if (cmbPickupPouint.SelectedItem == null)
            {   // Проверка на возможность добавления в бд без выбранного пункта выдачи
                MessageBox.Show("Выберите пункт выдачи!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            try
            {
                Order newOrder = new Order()
                {
                    OrderID = AutoEntities.GetContext().Order.OrderByDescending(x => x.OrderID).First().OrderID + 1,
                    IDOrderStatus = 1,
                    OrderDate = DateTime.Now,
                    OrderDeliveryDate = date,
                    OrderPickupPoint = cmbPickupPouint.SelectedIndex + 1,
                    ClientFullName = textUser.Text,
                    ReceiptCode = random.Next(100, 1000)
                };
                AutoEntities.GetContext().Order.Add(newOrder); //Передаём добавленные данные в таблицу Order
                //TODO Нужно сразу передовать CВЯЗАННЫЕ ORDER/ORFERPRODUCT/PRODUCT в следующую активность
                for (int i = 0; i < productArticle.Count(); i++)  // Счётчик, который будет добавлять записи до того как не закончатся артикулы
                {
                    OrderProduct newOrderProduct = new OrderProduct()
                    {
                        OrderID = newOrder.OrderID,
                        ProductArticleNumber = productArticle[i],
                        CountProduct = 1
                    };
                    AutoEntities.GetContext().OrderProduct.Add(newOrderProduct); //Передаём параметры для созранения в базу
                }
                AutoEntities.GetContext().SaveChanges();//Сохраняем записи в БД
                MessageBox.Show("Заказ Оформлен!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Navigate(new OrderTicketPage(newOrder, productList)); //Переходим на страницу талона заказа
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void TextCountProduct_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e) //проверрка на ввод чисел в поле количества
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
