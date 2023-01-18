using AutoService.Entities;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Data.Entity;

namespace AutoService.Pages
{
    public partial class Authorization : Page
    {
        public Authorization()
        {
            InitializeComponent();
            textBlockCaptcha.Visibility = Visibility.Hidden;//Скрываем поля
            textCaptch.Visibility = Visibility.Hidden;      //Предназначенные для капчи
        }

        private void btnEnterGuest_Click(object sender, RoutedEventArgs e)
        {//Переход на страницу клиента для неавторизоанного пользователя
            NavigationService.Navigate(new Client());
        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            string login = textLogin.Text.Trim();//Записываем в переменную логин
            string password = textPassword.Text.Trim(); //Записываем в перемненную пароль

            User user = new User();//создаём объект пользователя

            user = AutoServiceEntities.GetContext().User.Where(p => p.UserLogin == login && p.UserPassword = password.FirstOrDefault());


        }
    }
}
