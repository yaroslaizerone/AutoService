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
    /// Логика взаимодействия для EditOrder.xaml
    /// </summary>
    public partial class EditOrder : Page
    {
        Order orderClient = new Order();

        public EditOrder(Order order)
        {
            InitializeComponent();
            
            orderClient = order;

            CBStatus.ItemsSource = AutoEntities.GetContext().StatusOrders.ToList();//Ввод видов состояния заказа списком
            CBPickUp.ItemsSource = AutoEntities.GetContext().PickupPoint.ToList();//Ввод пунктов выдачи в список

            CBStatus.SelectedIndex = orderClient.IDOrderStatus;
            CBPickUp.SelectedIndex = orderClient.OrderPickupPoint;

            DataContext = orderClient;
        }

        private void btnSaveProduct_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
