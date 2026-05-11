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
using System.Windows.Shapes;

namespace Bolnisa.Views
{
    /// <summary>
    /// Логика взаимодействия для DoctorWindow.xaml
    /// </summary>
    public partial class DoctorWindow : Window
    {
        public DoctorWindow()
        {
            InitializeComponent();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void btnTodayAppointments_Click(object sender, RoutedEventArgs e)
        {
            var grid = new DataGrid
            {
                Margin = new Thickness(10),
                AutoGenerateColumns = false
            };

            grid.Columns.Add(new DataGridTextColumn { Header = "Номер записи", Binding = new System.Windows.Data.Binding("Id"), Width = 80 });
            grid.Columns.Add(new DataGridTextColumn { Header = "Дата записи", Binding = new System.Windows.Data.Binding("Date"), Width = 100 });
            grid.Columns.Add(new DataGridTextColumn { Header = "Время записи", Binding = new System.Windows.Data.Binding("Time"), Width = 80 });
            grid.Columns.Add(new DataGridTextColumn { Header = "ФИО Пациента", Binding = new System.Windows.Data.Binding("PatientName"), Width = 180 });
            grid.Columns.Add(new DataGridTextColumn { Header = "ФИО Врача", Binding = new System.Windows.Data.Binding("DoctorName"), Width = 180 });

            var appointments = new List<Appointment>
            {
                new Appointment { Id = 1, Date = "11.05.2026", Time = "10:00", PatientName = "Иванов И.И.", DoctorName = "Иванов А.А." },
                new Appointment { Id = 2, Date = "11.05.2026", Time = "11:30", PatientName = "Петрова М.С.", DoctorName = "Иванов А.А." }
            };
            grid.ItemsSource = appointments;

            contentGrid.Children.Clear();
            contentGrid.Children.Add(grid);
        }

        private void btnEditPatientCard_Click(object sender, RoutedEventArgs e)
        {
            var panel = CreateEditPatientCardPanel();
            contentGrid.Children.Clear();
            contentGrid.Children.Add(panel);
        }

        private void btnCompleteAppointment_Click(object sender, RoutedEventArgs e)
        {
            var panel = CreateChangeStatusPanel();
            contentGrid.Children.Clear();
            contentGrid.Children.Add(panel);
        }

        private StackPanel CreateEditPatientCardPanel()
        {
            var panel = new StackPanel { Margin = new Thickness(20) };

            panel.Children.Add(new TextBlock { Text = "Редактирование карты пациента", FontSize = 18, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 0, 20) });

            var fields = new string[] { "ID пациента", "ФИО пациента", "Рост", "Мед карта", "Вес", "Дата рождения", "Сердцебиение", "Общее состояние", "Номер телефона", "Адрес проживания" };

            foreach (var field in fields)
            {
                var stack = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 5,0,0) };
                stack.Children.Add(new TextBlock { Text = field, Width = 150, VerticalAlignment = VerticalAlignment.Center });
                stack.Children.Add(new TextBox { Width = 200 });
                panel.Children.Add(stack);
            }

            var saveBtn = new Button { Content = "Изменить", Width = 120, Height = 35, Margin = new Thickness(0, 20, 0, 0) };
            saveBtn.Click += (s, e2) => MessageBox.Show("Данные пациента обновлены");
            panel.Children.Add(saveBtn);

            return panel;
        }

        private StackPanel CreateChangeStatusPanel()
        {
            var panel = new StackPanel { Margin = new Thickness(20) };

            panel.Children.Add(new TextBlock { Text = "Смена статуса приёма", FontSize = 18, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 0, 20) });

            var stackNum = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 5,0,0) };
            stackNum.Children.Add(new TextBlock { Text = "Номер записи", Width = 120, VerticalAlignment = VerticalAlignment.Center });
            stackNum.Children.Add(new TextBox { Name = "txtAppointmentId", Width = 150 });
            panel.Children.Add(stackNum);

            var stackStatus = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 5,0,0) };
            stackStatus.Children.Add(new TextBlock { Text = "Статус записи", Width = 120, VerticalAlignment = VerticalAlignment.Center });
            var cmbStatus = new ComboBox { Width = 150 };
            cmbStatus.Items.Add("Принят");
            cmbStatus.Items.Add("Завершён");
            cmbStatus.SelectedIndex = 0;
            stackStatus.Children.Add(cmbStatus);
            panel.Children.Add(stackStatus);

            var addBtn = new Button { Content = "Добавить", Width = 120, Height = 35, Margin = new Thickness(0, 20, 0, 0) };
            addBtn.Click += (s, e2) => MessageBox.Show("Статус приёма изменён");
            panel.Children.Add(addBtn);

            return panel;
        }
    }

    public class Appointment
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
    }
}
