using AutoService.Pages;
using System.Windows;

namespace AutoService
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            frmMain.Navigate(new Authorization()); //Переход на страницу авторизации
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            frmMain.GoBack();
        }

        private void frmMain_ContentRendered(object sender, System.EventArgs e)
        {
            if (frmMain.CanGoBack)
            {
                btnBack.Visibility = Visibility.Visible;
            }
            else
            {
                btnBack.Visibility = Visibility.Hidden;
            }
        }
    }
}
