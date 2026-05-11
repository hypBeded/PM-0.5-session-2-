using Bolnisa.Models;
using Bolnisa.Services;
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
        private User _currentUser;

        public NurseWindow(User user)
        {
            InitializeComponent();
            _currentUser = user;
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void btnAppointments_Click(object sender, RoutedEventArgs e)
        {
            ShowAppointments();
        }

        private void btnEditPatient_Click(object sender, RoutedEventArgs e)
        {
            ShowEditPatient();
        }

        private void ShowAppointments()
        {
            var appointments = DatabaseService.GetTodayAppointments();

            var grid = new DataGrid
            {
                Margin = new Thickness(10),
                AutoGenerateColumns = false,
                IsReadOnly = true
            };

            grid.Columns.Add(new DataGridTextColumn { Header = "ФИО пациента", Binding = new System.Windows.Data.Binding("PatientName"), Width = 180 });
            grid.Columns.Add(new DataGridTextColumn { Header = "Дата записи", Binding = new System.Windows.Data.Binding("AppointmentDate"), Width = 100 });
            grid.Columns.Add(new DataGridTextColumn { Header = "Время записи", Binding = new System.Windows.Data.Binding("AppointmentTime"), Width = 80 });
            grid.Columns.Add(new DataGridTextColumn { Header = "ФИО Врача", Binding = new System.Windows.Data.Binding("DoctorName"), Width = 180 });
            grid.Columns.Add(new DataGridTextColumn { Header = "Статус", Binding = new System.Windows.Data.Binding("Status"), Width = 100 });

            grid.ItemsSource = appointments;

            contentGrid.Children.Clear();
            contentGrid.Children.Add(grid);
        }

        private void ShowEditPatient()
        {
            var scrollViewer = new ScrollViewer();
            var panel = new StackPanel { Margin = new Thickness(20) };

            panel.Children.Add(new TextBlock { Text = "Редактирование данных пациента", FontSize = 18, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 0, 20) });

            var cmbPatient = new ComboBox { Width = 250, DisplayMemberPath = "FullName" };
            var patients = DatabaseService.GetAllPatients();
            cmbPatient.ItemsSource = patients;
            cmbPatient.SelectedIndex = 0;
            panel.Children.Add(CreateFormRow("Выберите пациента:", cmbPatient));

            var txtPhone = new TextBox { Width = 250 };
            var txtAddress = new TextBox { Width = 250 };
            var txtHeight = new TextBox { Width = 250 };
            var txtWeight = new TextBox { Width = 250 };
            var txtHeartRate = new TextBox { Width = 250 };
            var txtCondition = new TextBox { Width = 250 };

            cmbPatient.SelectionChanged += (s, e) =>
            {
                if (cmbPatient.SelectedItem is Patient patient)
                {
                    txtPhone.Text = patient.Phone;
                    txtAddress.Text = patient.Address;
                    txtHeight.Text = patient.Height?.ToString() ?? "";
                    txtWeight.Text = patient.Weight?.ToString() ?? "";
                    txtHeartRate.Text = patient.HeartRate?.ToString() ?? "";
                    txtCondition.Text = patient.GeneralCondition ?? "";
                }
            };

            panel.Children.Add(CreateFormRow("Телефон:", txtPhone));
            panel.Children.Add(CreateFormRow("Адрес проживания:", txtAddress));
            panel.Children.Add(CreateFormRow("Рост (см):", txtHeight));
            panel.Children.Add(CreateFormRow("Вес (кг):", txtWeight));
            panel.Children.Add(CreateFormRow("Сердцебиение (уд/мин):", txtHeartRate));
            panel.Children.Add(CreateFormRow("Общее состояние:", txtCondition));

            var saveBtn = new Button { Content = "Сохранить изменения", Width = 150, Height = 35, Margin = new Thickness(0, 20, 0, 0) };
            saveBtn.Click += (s, e) =>
            {
                if (cmbPatient.SelectedItem is Patient patient)
                {
                    patient.Phone = txtPhone.Text;
                    patient.Address = txtAddress.Text;
                    patient.Height = string.IsNullOrEmpty(txtHeight.Text) ? null : (double?)double.Parse(txtHeight.Text);
                    patient.Weight = string.IsNullOrEmpty(txtWeight.Text) ? null : (double?)double.Parse(txtWeight.Text);
                    patient.HeartRate = string.IsNullOrEmpty(txtHeartRate.Text) ? null : (int?)int.Parse(txtHeartRate.Text);
                    patient.GeneralCondition = txtCondition.Text;
                    DatabaseService.UpdatePatient(patient);
                    MessageBox.Show("Данные пациента обновлены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            };
            panel.Children.Add(saveBtn);

            scrollViewer.Content = panel;
            contentGrid.Children.Clear();
            contentGrid.Children.Add(scrollViewer);
        }

        private StackPanel CreateFormRow(string label, FrameworkElement control)
        {
            var stack = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 5,0,0) };
            stack.Children.Add(new TextBlock { Text = label, Width = 130, VerticalAlignment = VerticalAlignment.Center });
            stack.Children.Add(control);
            return stack;
        }
    }
}
