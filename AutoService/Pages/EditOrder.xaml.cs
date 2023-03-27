using AutoService.Entities;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для EditOrder.xaml
    /// </summary>
    public partial class EditOrder : Page
    {
        Order orderClient = new Order();
        List<OrderProduct> listproduct = new List<OrderProduct>();

        public EditOrder(Order order, List<OrderProduct> listp)
        {
            InitializeComponent();
            
            orderClient = order;
            if (listp != null )
            {
                listproduct = listp;
            }

            CBStatus.ItemsSource = AutoEntities.GetContext().StatusOrders.ToList();//Ввод видов состояния заказа списком
            CBPickUp.ItemsSource = AutoEntities.GetContext().PickupPoint.ToList();//Ввод пунктов выдачи в список

            CBStatus.SelectedIndex = orderClient.IDOrderStatus - 1;
            CBPickUp.SelectedIndex = orderClient.OrderPickupPoint - 1;

            DataContext = orderClient;
        }

        private void btnSaveProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                orderClient.IDOrderStatus = CBStatus.SelectedIndex + 1;
                orderClient.OrderPickupPoint = CBPickUp.SelectedIndex + 1;
                orderClient.ClientFullName = ClientName.Text;
                orderClient.ReceiptCode = Int32.Parse(DropCode.Text);
                orderClient.OrderDate = DateOfish.SelectedDate.Value;
                orderClient.OrderDeliveryDate = DateDeliv.SelectedDate.Value;
                if (listproduct != null)
                {
                    foreach (var item in listproduct)
                    {
                        var check = AutoEntities.GetContext().OrderProduct.Where(x => x.Order == item.Order && x.Product == item.Product);
                        if (check == null)
                        {
                            AutoEntities.GetContext().OrderProduct.Add(item);
                        }
                    }
                }
                AutoEntities.GetContext().SaveChanges();
                MessageBox.Show("Запись была изменена");
            }
            catch
            {
                MessageBox.Show("Произошла ошибка");
            }
        }

        private void StackPanel_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
        private static readonly Regex _regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private void btnEditProductList_Click(object sender, RoutedEventArgs e)
        {
            listproduct = AutoEntities.GetContext().OrderProduct.Where(x => x.OrderID == orderClient.OrderID).ToList();
            NavigationService.Navigate(new EditProductOrderList(listproduct, orderClient));
        }
    }
}
