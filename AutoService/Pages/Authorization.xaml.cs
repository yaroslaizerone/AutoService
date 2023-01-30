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
                GenerataCapcha();
                DelLoginAndPassword();
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
        }

        private void GenerataCapcha() //Генерация капчи
        {
            textCaptch.Visibility = Visibility.Visible;
            textBlockCaptcha.Visibility = Visibility.Visible;

            String allowchar = "";
            allowchar = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            allowchar += "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,y,z";
            allowchar += "1,2,3,4,5,6,7,8,9,0";
            char[] a = { ',' };
            String[] ar = allowchar.Split(a);
            String pwd = "";
            string temp = "";
            Random r = new Random();

            for (int i = 0; i < 8; i++)
            {
                temp = ar[(r.Next(0, ar.Length))];
                pwd += temp;
            }
            textBlockCaptcha.Text = pwd;
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
