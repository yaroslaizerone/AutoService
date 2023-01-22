using AutoService.Entities;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace AutoService.Pages
{
    public partial class Authorization : Page
    {
        private int countUnsuccessful = 0; //Количество неверных попыток входа
        public Authorization()
        {
            InitializeComponent();
            textBlockCaptcha.Visibility = Visibility.Hidden;//Скрываем поля
            textCaptch.Visibility = Visibility.Hidden;      //Предназначенные для капчи
        }

        private void btnEnterGuest_Click(object sender, RoutedEventArgs e)
        {//Переход на страницу клиента для неавторизоанного пользователя
            NavigationService.Navigate(new Client(null));
        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            string login = textLogin.Text.Trim();//Записываем в переменную логин
            string password = textPassword.Text.Trim(); //Записываем в перемненную пароль

            User user = new User();//Создаём объект пользователя

            user = AutoEntities.GetContext().User.Where(x => x.UserLogin == login && x.UserPassword == password).FirstOrDefault(); // Параметр, хранящий в себе данные найденного пользователя
            int userСount = AutoEntities.GetContext().User.Where(x => x.UserLogin == login && x.UserPassword == password).Count(); // Поиск количества пользователей с данными параметрами
            
            if (countUnsuccessful < 1) //Проверка, на схождение логина и пароля
            {
                if(userСount > 0)
                {
                    MessageBox.Show($"Вы вошли под: {user.Role.RoleName.ToString()}");
                    LoadForm(user.Role.RoleName.ToString(),user);
                }
                else
                {
                    MessageBox.Show("Пароль или логин был неверно введён!");
                    countUnsuccessful++;
                    if (countUnsuccessful == 1) // Если произошёл неправильный ввод
                        GenerataCapcha();       // Генерируется капча
                }
            }
            else
            {
                if (userСount > 0 && textBlockCaptcha.Text == textCaptch.Text)
                {
                    MessageBox.Show($"Вы вошли под: {user.Role.RoleName.ToString()}");
                    LoadForm(user.Role.RoleName.ToString(), user);
                }
                else { 
                    MessageBox.Show("Введите данные заново!");
                }
            }
        }
        private void GenerataCapcha()
        {
            textCaptch.Visibility = Visibility.Visible;
            textBlockCaptcha.Visibility = Visibility.Visible;

            Random random= new Random();
            int randomNum = random.Next(0, 3);
            // Каждому числу присвоено своё значение капчи
            switch (randomNum)
            {
                case 1:
                    textBlockCaptcha.Text = "ju2sT8Cbs";
                    textCaptch.TextDecorations = TextDecorations.Strikethrough;
                    break;
                case 2:
                    textBlockCaptcha.Text = "iNuK2cl";
                    textCaptch.TextDecorations = TextDecorations.Strikethrough;
                    break;
                case 3:
                    textBlockCaptcha.Text = "uOozGk95";
                    textCaptch.TextDecorations = TextDecorations.Strikethrough;
                    break;
            }
        }

        private void LoadForm (string _role, User user)
        {   
            switch(_role) //В зависимости от роли переходим на соответсвующую странцу
            {
                case "Клиент":
                    NavigationService.Navigate(new Client(user));
                    break;
            }
        }
    }
}
