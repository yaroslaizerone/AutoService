﻿using AutoService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
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
    /// Логика взаимодействия для EditProductOrderList.xaml
    /// </summary>
    public partial class EditProductOrderList : Page
    {
        List<OrderProduct> listprod = new List<OrderProduct>();
        Order clientOrder = new Order();
        List<OrderProduct> Deletelistprod = new List<OrderProduct>();
        public EditProductOrderList(List<OrderProduct> listproduct, Order orderClient)
        {
            InitializeComponent();
            clientOrder = orderClient;
            listprod = listproduct;
            var product = AutoEntities.GetContext().Product.ToList(); // Обращаемся к таблице с товарами и получаем список
            LViewProduct.ItemsSource = product; // Передаём список в лист
            lViewOrder.ItemsSource = listprod; // Передаём список заказаных товаров
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            OrderProduct SelectItem = new OrderProduct
            {
                Product = LViewProduct.SelectedItem as Product,
                CountProduct = 1,
                Order = clientOrder
            };
            int checkproduct = 0;
            foreach (var item in listprod)
            {
                if(item.Product.ProductArticleNumber == SelectItem.Product.ProductArticleNumber)
                {
                    checkproduct++;
                }
            }
            if (listprod.Count() >= 0 && checkproduct==0) // Проверка на наличие повторения в списке
            {
                listprod.Add(SelectItem);
                AutoEntities.GetContext().OrderProduct.Add(SelectItem);
            }
            else
            {
                MessageBox.Show("Такой товар уже есть в вашей карзине!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            lViewOrder.ItemsSource = null;
            lViewOrder.ItemsSource = listprod;
        }

        private void btnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить этот элемент?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                var selected = lViewOrder.SelectedItems.Cast<OrderProduct>().ToArray();
                foreach (var item in selected)
                {
                    Deletelistprod.Add(item);
                    listprod.Remove(item);
                }

                lViewOrder.ItemsSource = null;
                lViewOrder.ItemsSource = listprod;
            }
        }

        private void MinusProduct_Click(object sender, RoutedEventArgs e)
        {
            var selected = lViewOrder.SelectedItems.Cast<OrderProduct>().ToArray();
            foreach (OrderProduct item in selected)
            {
                if (item.CountProduct >= 0)
                {
                    item.CountProduct--;
                    lViewOrder.ItemsSource = null;
                    lViewOrder.ItemsSource = listprod;
                }
                else
                    MessageBox.Show("Нельзя заказать отрицательное количество товара");
            }
        }

        private void PlusProduct_Click(object sender, RoutedEventArgs e)
        {
            var selected = lViewOrder.SelectedItems.Cast<OrderProduct>().ToArray();
            foreach (OrderProduct item in selected)
            {
                item.CountProduct = item.CountProduct + 1;
                lViewOrder.ItemsSource = null;
                lViewOrder.ItemsSource = listprod;
            }
        }

        private void SaveChengeProductList_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Состав товаров заказа был изменён!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            foreach (var item in Deletelistprod)
            {
                if (AutoEntities.GetContext().OrderProduct.Any(x=>x.Product.ProductArticleNumber == item.Product.ProductArticleNumber && x.Order.OrderID == item.Order.OrderID))
                {
                    AutoEntities.GetContext().OrderProduct.Remove(item);
                }
            }
            NavigationService.Navigate(new EditOrder(clientOrder,listprod)); //Переходим на страницу редактирования заказа
        }

        private static readonly Regex _regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private void TextCountProduct_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
    }
}
