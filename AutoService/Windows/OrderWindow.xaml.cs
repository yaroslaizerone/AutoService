using AutoService.Entities;
using AutoService.Pages;
using System.Collections.Generic;
using System.Windows;


namespace AutoService.Windows
{
    /// <summary>
    /// Логика взаимодействия для OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        public OrderWindow(List<Product> products, User user)
        {
            InitializeComponent();
            frmOrder.Navigate(new OrderPage(products, user));
        }
    }
}
