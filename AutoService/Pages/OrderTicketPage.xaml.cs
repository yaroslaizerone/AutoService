using AutoService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace AutoService.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrderTicketPage.xaml
    /// </summary>
    public partial class OrderTicketPage : Page
    {
        List<OrderProduct> productList = new List<OrderProduct>();
        public OrderTicketPage(Order currentOrder, List<OrderProduct> products)
        {
            InitializeComponent();
            productList = products; // Передаём в пустой лист данные с прошлой страницы
            DataContext = currentOrder;//Привязываем контекст к классу

            textPickupPoint.Text = currentOrder.PickupPoint.Addres; //Выводим адрес пункта выдачи
            var result = "";

            foreach (var pl in productList)
                result += (result == "" ? "" : ", ") + pl.Product.ProductName.ToString(); //Выводим названия товаров, оформленых в заказе
            textProductList.Text = result.ToString();

            var total = productList.Sum(x => Convert.ToDouble(x.Product.ProductCost) - Convert.ToDouble(x.Product.ProductCost) * Convert.ToDouble(x.Product.ProductDiscountAmount) / 100.00);
            textCost.Text = total.ToString() + " рублей"; //Выводим итоговую стоимость заказа
        }

        private void btnSaveDocument_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog pd = new PrintDialog();
            if (pd.ShowDialog() == true)
            {
                IDocumentPaginatorSource idp = flowDocument;
                pd.PrintDocument(idp.DocumentPaginator, Title);
            }
        }
    }
}
