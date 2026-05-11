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
    /// Логика взаимодействия для NurseWindow.xaml
    /// </summary>
    public partial class NurseWindow : Window
    {
        public NurseWindow()
        {
            InitializeComponent();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void btnAppointments_Click(object sender, RoutedEventArgs e)
        {
            var grid = new DataGrid
            {
                Margin = new Thickness(10),
                AutoGenerateColumns = false
            };

            grid.Columns.Add(new DataGridTextColumn { Header = "ФИО пациента", Binding = new System.Windows.Data.Binding("PatientName"), Width = 150 });
            grid.Columns.Add(new DataGridTextColumn { Header = "Дата записи", Binding = new System.Windows.Data.Binding("Date"), Width = 100 });
            grid.Columns.Add(new DataGridTextColumn { Header = "Время записи", Binding = new System.Windows.Data.Binding("Time"), Width = 80 });
            grid.Columns.Add(new DataGridTextColumn { Header = "ФИО Врача", Binding = new System.Windows.Data.Binding("DoctorName"), Width = 150 });
            grid.Columns.Add(new DataGridTextColumn { Header = "Специальность врача", Binding = new System.Windows.Data.Binding("Specialty"), Width = 120 });

            var appointments = new List<NurseAppointment>
            {
                new NurseAppointment { PatientName = "Иванов И.И.", Date = "11.05.2026", Time = "10:00", DoctorName = "Иванов А.А.", Specialty = "Терапевт" },
                new NurseAppointment { PatientName = "Петрова М.С.", Date = "11.05.2026", Time = "11:30", DoctorName = "Петров Б.Б.", Specialty = "Хирург" }
            };
            grid.ItemsSource = appointments;

            contentGrid.Children.Clear();
            contentGrid.Children.Add(grid);
        }

        private void btnEditPatient_Click(object sender, RoutedEventArgs e)
        {
            var panel = new StackPanel { Margin = new Thickness(20) };

            panel.Children.Add(new TextBlock { Text = "Редактирование данных пациента", FontSize = 18, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 0, 20) });

            var fields = new string[] { "ФИО пациента", "Телефон", "Мед карта", "День рождения", "Адрес проживания", "Рост", "Вес" };

            foreach (var field in fields)
            {
                var stack = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 5,0,0) };
                stack.Children.Add(new TextBlock { Text = field, Width = 130, VerticalAlignment = VerticalAlignment.Center });
                stack.Children.Add(new TextBox { Width = 200 });
                panel.Children.Add(stack);
            }

            var saveBtn = new Button { Content = "Сохранить изменения", Width = 140, Height = 35, Margin = new Thickness(0, 20, 0, 0) };
            saveBtn.Click += (s, e2) => MessageBox.Show("Данные пациента обновлены");
            panel.Children.Add(saveBtn);

            contentGrid.Children.Clear();
            contentGrid.Children.Add(panel);
        }
    }

    public class NurseAppointment
    {
        public string PatientName { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string DoctorName { get; set; }
        public string Specialty { get; set; }
    }
}
