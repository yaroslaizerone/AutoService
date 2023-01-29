using AutoService.Entities;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace AutoService.Pages
{
    public partial class Authorization : Page
    {
        private int countUnsuccessful = 0; //Количество неверных попыток входа
        DispatcherTimer _timer;
        TimeSpan _time;
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
            countUnsuccessful++;
            string login = textLogin.Text.Trim();//Записываем в переменную логин
            string password = textPassword.Text.Trim(); //Записываем в перемненную пароль

            User user = new User();//Создаём объект пользователя

            user = AutoEntities.GetContext().User.Where(x => x.UserLogin == login && x.UserPassword == password).FirstOrDefault(); // Параметр, хранящий в себе данные найденного пользователя
            int userСount = AutoEntities.GetContext().User.Where(x => x.UserLogin == login && x.UserPassword == password).Count(); // Поиск количества пользователей с данными параметрами
            //Проверка, на количество попыток успешного входа
            if (userСount == 0 && textBlockCaptcha.Text != textCaptch.Text && countUnsuccessful == 4)
            {
                DelLoginAndPassword();
                StopActive();
            }
            else if (userСount > 0 && countUnsuccessful <= 1) 
            {
                MessageBox.Show($"Вы вошли под: {user.Role.RoleName.ToString()}");
                LoadForm(user.Role.RoleName.ToString(), user);
            }
            else if (userСount > 0 && textBlockCaptcha.Text == textCaptch.Text)
            {
                MessageBox.Show($"Вы вошли под: {user.Role.RoleName.ToString()}");
                LoadForm(user.Role.RoleName.ToString(), user);
            }
            else
            {
                DelLoginAndPassword();
                GenerataCapcha();
                userСount = 0;
                MessageBox.Show("Пароль или логин был неверно введён!");
            }
        }

        private void StopActive()
        {
            textBlockCaptcha.Visibility = Visibility.Hidden;
            textCaptch.Visibility = Visibility.Hidden;
            _time = TimeSpan.FromSeconds(10);
            btnEnter.IsEnabled = false;
            tblPredup.Visibility = Visibility.Visible;
            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                tblTime.Text = _time.ToString("ss");
                if (_time == TimeSpan.Zero)
                {
                    _timer.Stop();
                    tblPredup.Visibility = Visibility.Hidden;
                    tblTime.Text = "";
                    var converter = new System.Windows.Media.BrushConverter();
                    textLogin.BorderBrush = (System.Windows.Media.Brush)converter.ConvertFromString("#FF498C51");
                    textPassword.BorderBrush = (System.Windows.Media.Brush)converter.ConvertFromString("#FF498C51");
                    btnEnter.IsEnabled = true;
                    countUnsuccessful = 0;
                }
                _time = _time.Add(TimeSpan.FromSeconds(-1));
            }, System.Windows.Application.Current.Dispatcher);
            _timer.Start();
        }

        private void DelLoginAndPassword()
        {
            textLogin.Text = "";
            textPassword.Text = "";
            textCaptch.Text = "";
        }

        private void GenerataCapcha()
        {
            textCaptch.Visibility = Visibility.Visible;
            textBlockCaptcha.Visibility = Visibility.Visible;

            Random random = new Random();
            int randomNum = random.Next(0, 3);
            // Каждому числу присвоено своё значение капчи
            switch (randomNum)
            {
                case 1:
                    textBlockCaptcha.Text = "ju2sT8Cbs";
                    break;
                case 2:
                    textBlockCaptcha.Text = "iNuK2cl";
                    break;
                case 3:
                    textBlockCaptcha.Text = "uOozGk95";
                    break;
            }
        }

        private void LoadForm(string _role, User user)
        {
            switch (_role) //В зависимости от роли переходим на соответсвующую странцу
            {
                case "Клиент":
                    NavigationService.Navigate(new Client(user));
                    break;
                case "Менеджер":
                    NavigationService.Navigate(new Client(user));
                    break;
                case "Администратор":
                    NavigationService.Navigate(new Admin(user));
                    break;
            }
        }
    }
}
