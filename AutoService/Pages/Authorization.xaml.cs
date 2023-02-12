using AutoService.Entities;
using System;
using System.Drawing.Drawing2D;
using System.Drawing;
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
        DispatcherTimer TimerUser;
        TimeSpan TimeAfk;
        public Authorization()
        {
            InitializeComponent();
            textBlockCaptcha.Visibility = Visibility.Collapsed;//Скрываем поля
            textCaptch.Visibility = Visibility.Collapsed;      //Предназначенные для капчи
        }

        private void btnEnterGuest_Click(object sender, RoutedEventArgs e)
        {//Переход на страницу клиента для неавторизоанного пользователя
            NavigationService.Navigate(new Client(null));
        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            countUnsuccessful++;
            User user = new User();//Создаём объект пользователя
            string login = textLogin.Text.Trim();//Записываем в переменную логин
            string password = textPassword.Text.Trim(); //Записываем в перемненную пароль

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
                Аuthorization(user);
            }
            else if (userСount > 0 && textBlockCaptcha.Text == textCaptch.Text)
            {
                Аuthorization(user);
            }
            else
            {
                GenerataCapcha();
                DelLoginAndPassword();
                MessageBox.Show("Пароль или логин был неверно введён!");
            }
        }

        public void Аuthorization(User user)
        {
            MessageBox.Show($"Вы вошли под: {user.Role.RoleName.ToString()}");
            LoadForm(user.Role.RoleName.ToString(), user);
        }

        private void StopActive()
        {
            var converter = new System.Windows.Media.BrushConverter();
            textBlockCaptcha.Visibility = Visibility.Collapsed;
            textCaptch.Visibility = Visibility.Collapsed;
            TimeAfk = TimeSpan.FromSeconds(10);

            textLogin.BorderBrush = (System.Windows.Media.Brush)converter.ConvertFromString("#FFCC0605");
            textPassword.BorderBrush = (System.Windows.Media.Brush)converter.ConvertFromString("#FFCC0605");
            btnEnter.IsEnabled = false;
            textLogin.IsEnabled= false;
            textPassword.IsEnabled = false;
            tblPredup.Visibility = Visibility.Visible;

            TimerUser = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                tblTime.Text = TimeAfk.ToString("ss");
                if (TimeAfk == TimeSpan.Zero)
                {
                    TimerUser.Stop();
                    tblPredup.Visibility = Visibility.Hidden;
                    tblTime.Text = "";
                    
                    textLogin.BorderBrush = (System.Windows.Media.Brush)converter.ConvertFromString("#FF498C51");
                    textPassword.BorderBrush = (System.Windows.Media.Brush)converter.ConvertFromString("#FF498C51");
                    btnEnter.IsEnabled = true;
                    textLogin.IsEnabled = true;
                    textPassword.IsEnabled = true;

                    countUnsuccessful = 0;
                }
                TimeAfk = TimeAfk.Add(TimeSpan.FromSeconds(-1));
            }, System.Windows.Application.Current.Dispatcher);
            TimerUser.Start();
        }

        private void DelLoginAndPassword()
        {
            textLogin.Text = "";
            textPassword.Text = "";
            textCaptch.Text = "";
        }

        private string GenerataCapcha() //Генерация капчи
        {
            textCaptch.Visibility = Visibility.Visible;
            textBlockCaptcha.Visibility = Visibility.Visible;

            String Allowchar = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            Allowchar += "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,y,z";
            Allowchar += "1,2,3,4,5,6,7,8,9,0";
            char[] Separator = { ',' };
            String[] Symbols = Allowchar.Split(Separator);
            String Captha = "";
            string Temp = "";
            Random r = new Random();

            for (int i = 0; i < 7; i++)
            {
                Temp = Symbols[(r.Next(0, Symbols.Length))];
                Captha += Temp;
            }
            return textBlockCaptcha.Text = Captha;
        }

        private void LoadForm(string Role, User user)
        {
            switch (Role) //В зависимости от роли переходим на соответсвующую странцу
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
