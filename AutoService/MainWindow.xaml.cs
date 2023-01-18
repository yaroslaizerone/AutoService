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
    }
}
