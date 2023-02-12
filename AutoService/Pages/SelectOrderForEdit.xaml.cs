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
    /// Логика взаимодействия для SelectOrderForEdit.xaml
    /// </summary>
    public partial class SelectOrderForEdit : Page
    {
        public SelectOrderForEdit()
        {
            InitializeComponent();
            var OrderDB = AutoEntities.GetContext().Order.ToList(); // Обращаемся к таблице с товарами и получаем список
            LViewOrders.ItemsSource = OrderDB; // Передаём список в лист
            DataContext = this; //Используем данный класс как объект для привязки контекста

            textAllAmount.Text = OrderDB.Count.ToString(); //Передаём количество всех записей из таблицы
            UpdateData();
        }

        private void btnAddOrder_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditOrder(LViewOrders.SelectedItem as Order));
        }

        private void textSearch_SelectionChanged(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }

        private void cmbSorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }

        private void UpdateData() //Метод обновления содержимого ListView
        {
            var result = AutoEntities.GetContext().Order.ToList(); //Принимаем данные из таблицы product в переменную

            if (cmbSorting.SelectedIndex == 1)// Реализация сортировки 
                result = result.OrderBy(x => x.OrderDate).ToList();
            if (cmbSorting.SelectedIndex == 2)
                result = result.OrderByDescending(x => x.OrderDate).ToList();

            result = result.Where(x => x.ClientFullName.ToLower().Contains(textSearch.Text.ToLower())).ToList();//Реализация поиска
            LViewOrders.ItemsSource = result; //Передаём результат в ListView

            textResultAmount.Text = result.Count.ToString();//Передём количество записей после применения поиска, сортировки, фильтрации
        }

        public string[] SortingList { get; set; } =
        {
            "Без сортировки",
            "Дата создания по возрастанию",
            "Дата создания по убыванию"
        };
    }
}
