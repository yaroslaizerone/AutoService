using AutoService.Entities;
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
    /// Логика взаимодействия для OrderTicketPage.xaml
    /// </summary>
    public partial class OrderTicketPage : Page
    {
        List<Product> productList = new List<Product>();
        public OrderTicketPage(Order currentOrder, List<Product> products)
        {
            InitializeComponent();
            productList = products; // Передаём в пустой лист данные с прошлой страницы
            DataContext = currentOrder;//Привязываем контекст к классу

            textPickupPoint.Text = currentOrder.PickupPoint.Addres; //Выводим адрес пункта выдачи
            var result = "";

            foreach (var pl in productList)
                result += (result == "" ? "" : ", ") + pl.ProductName.ToString(); //Выводим названия товаров, оформленых в заказе
            textProductList.Text = result.ToString();

            var total = productList.Sum(x => Convert.ToDouble(x.ProductCost) - Convert.ToDouble(x.ProductCost) * Convert.ToDouble(x.ProductDiscountAmount) / 100.00);
            textCost.Text = total.ToString() + " рублей"; //Выводим итоговую стоимость заказа

            var discountSum = productList.Sum(x => x.ProductDiscountAmount);
            textDiscountSum.Text = discountSum.ToString() + "%"; //Выводим сумму скидки заказа
        }

        private void btnSaveDocument_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog pd = new PrintDialog();
            if(pd.ShowDialog() == true)
            {
                IDocumentPaginatorSource idp = flowDocument;
                pd.PrintDocument(idp.DocumentPaginator, Title);
            }
        }
    }
}
