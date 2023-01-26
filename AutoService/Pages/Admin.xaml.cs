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
    public partial class Admin : Page
    {
        User user = new User();
        public Admin(User currentUser)
        {
            InitializeComponent();

            var product = AutoserviceEntities.GetContext().Product.ToList(); // Обращаемся к таблице с товарами и получаем список
            LViewProduct.ItemsSource = product; // Передаём список в лист
            DataContext = this; //Используем данный класс как объект для привязки контекста

            textAllAmount.Text = product.Count.ToString(); //Передаём количество всех записей из таблицы

            user = currentUser; // передача значения в пустой объект

            UpdateData();
            Users();
        }
        private void Users()
        {
            if (user != null)
                textFullName.Text = user.UserSurname.ToString() + " " + user.UserName.ToString() + " " + user.UserPatronymic.ToString();
            else
                textFullName.Text = "Гость";
        }

        public string[] SortingList { get; set; } =
        {
            "Без сортировки",
            "Стоимость по возрастанию",
            "Стоимость по убыванию"
        };
        public string[] FilterList { get; set; } =
        {
            "Все диапозоны",
            "0%-9,99%",
            "10%-14,99%",
            "15% и более"
        };

        private void UpdateData()
        {
            var result = AutoserviceEntities.GetContext().Product.ToList(); //Принимаем данные из таблицы product в переменную

            if (cmbSorting.SelectedIndex == 1)// Реализация сортировки 
                result = result.OrderBy(x => x.ProductCost).ToList();
            if (cmbSorting.SelectedIndex == 2)
                result = result.OrderByDescending(x => x.ProductCost).ToList();

            if (cmbFilter.SelectedIndex == 1)//Реализация фильтрации
                result = result.Where(x => x.ProductDiscountAmount >= 0 && x.ProductDiscountAmount < 10).ToList();
            if (cmbFilter.SelectedIndex == 2)
                result = result.Where(x => x.ProductDiscountAmount >= 10 && x.ProductDiscountAmount < 15).ToList();
            if (cmbFilter.SelectedIndex == 3)
                result = result.Where(x => x.ProductDiscountAmount >= 15).ToList();

            result = result.Where(x => x.ProductName.ToLower().Contains(textSearch.Text.ToLower())).ToList();//Реализация поиска
            LViewProduct.ItemsSource = result; //Передаём результат в ListView

            textResultAmount.Text = result.Count.ToString();//Передём количество записей после применения поиска, сортировки, фильтрации
        }
        private void textSearch_SelectionChanged(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }

        private void cmbSorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }

        private void cmbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }

        private void btnAddNewProduct_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditProductPage(null)); // Если хотим добавить новый товар, то передаём пустое значение на следующую страницу
        }

        private void LViewProduct_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new AddEditProductPage(LViewProduct.SelectedItem as Product));// Если хотим перейти к редоктированию товара. то передаём на страницу данные об этом товаре
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(Visibility == Visibility.Visible)
            {
                AutoserviceEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                LViewProduct.ItemsSource = AutoserviceEntities.GetContext().Product.ToList();
            }
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnOrder_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
