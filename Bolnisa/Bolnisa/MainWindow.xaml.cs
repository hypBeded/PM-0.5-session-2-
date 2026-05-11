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
using Bolnisa.Views;

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
            string role = (cmbRole.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                ShowError("Введите логин и пароль");
                return;
            }

            // Заглушка авторизации - в реальном проекте проверка через БД
            // Логин: admin любой пароль для входа
            if (login == "admin")
            {
                switch (role)
                {
                    case "Мед Регистратор":
                     var regWindow = new RegWindow();
                     regWindow.Show();
                     this.Close();
                        break;
                    case "Врач":
                     var doctorWindow = new DoctorWindow();
                     doctorWindow.Show();
                      this.Close();
                        break;
                    case "Мед Сестра":
                     var nurseWindow = new NurseWindow();
                      nurseWindow.Show();
                     this.Close();
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