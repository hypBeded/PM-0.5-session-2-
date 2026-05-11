using Bolnisa.Views;
using Bolnisa.Services;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bolnisa
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text;
            string password = txtPassword.Password;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                ShowError("Введите логин и пароль");
                return;
            }

            var user = DatabaseService.GetUser(login, password);

            if (user != null)
            {
                switch (user.Role)
                {
                    case "Регистратор":
                        var regWindow = new RegWindow(user);
                        regWindow.Show();
                        this.Close();
                        break;
                    case "Врач":
                        var doctorWindow = new DoctorWindow(user);
                        doctorWindow.Show();
                        this.Close();
                        break;
                    case "Медсестра":
                        var nurseWindow = new NurseWindow(user);
                        nurseWindow.Show();
                        this.Close();
                        break;
                    default:
                        ShowError("Неизвестная роль пользователя");
                        break;
                }
            }
            else
            {
                ShowError("Неверный логин или пароль");
            }
        }

        private void ShowError(string message)
        {
            txtError.Text = message;
            txtError.Visibility = Visibility.Visible;
        }
    }
}