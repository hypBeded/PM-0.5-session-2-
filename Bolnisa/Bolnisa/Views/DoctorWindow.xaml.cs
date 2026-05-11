using Bolnisa.Models;
using Bolnisa.Services;
using System.Windows;
using System.Windows.Controls;

namespace Bolnisa.Views
{
    /// <summary>
    /// Логика взаимодействия для DoctorWindow.xaml
    /// </summary>
        public partial class DoctorWindow : Window
        {
            private User _currentUser;
            private Doctor _currentDoctor;
            private List<Appointment> _appointments;

            public DoctorWindow(User user)
            {
                InitializeComponent();
                _currentUser = user;
                LoadDoctorInfo();
            }

            private void LoadDoctorInfo()
            {
                var doctors = DatabaseService.GetAllDoctors();
                _currentDoctor = doctors.FirstOrDefault(d => d.FullName == _currentUser.FullName);
                if (_currentDoctor == null && doctors.Any())
                    _currentDoctor = doctors.First();
            }

            private void btnLogout_Click(object sender, RoutedEventArgs e)
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }

            private void btnTodayAppointments_Click(object sender, RoutedEventArgs e)
            {
                ShowTodayAppointments();
            }

            private void btnEditPatientCard_Click(object sender, RoutedEventArgs e)
            {
                ShowEditPatientCard();
            }

            private void btnCompleteAppointment_Click(object sender, RoutedEventArgs e)
            {
                ShowCompleteAppointmentForm();
            }

            private void ShowTodayAppointments()
            {
                if (_currentDoctor == null) return;

                _appointments = DatabaseService.GetDoctorAppointments(_currentDoctor.Id);

                var grid = new DataGrid
                {
                    Margin = new Thickness(10),
                    AutoGenerateColumns = false,
                    IsReadOnly = true,
                    SelectionMode = DataGridSelectionMode.Single
                };

                grid.Columns.Add(new DataGridTextColumn { Header = "Номер записи", Binding = new System.Windows.Data.Binding("Id"), Width = 80 });
                grid.Columns.Add(new DataGridTextColumn { Header = "Дата записи", Binding = new System.Windows.Data.Binding("AppointmentDate"), Width = 100 });
                grid.Columns.Add(new DataGridTextColumn { Header = "Время записи", Binding = new System.Windows.Data.Binding("AppointmentTime"), Width = 80 });
                grid.Columns.Add(new DataGridTextColumn { Header = "ФИО Пациента", Binding = new System.Windows.Data.Binding("PatientName"), Width = 180 });
                grid.Columns.Add(new DataGridTextColumn { Header = "Статус", Binding = new System.Windows.Data.Binding("Status"), Width = 100 });

                grid.ItemsSource = _appointments;

                contentGrid.Children.Clear();
                contentGrid.Children.Add(grid);
            }

            private void ShowEditPatientCard()
            {
                var panel = new StackPanel { Margin = new Thickness(20) };

                panel.Children.Add(new TextBlock { Text = "Редактирование карты пациента", FontSize = 18, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 0, 20) });

                var cmbPatient = new ComboBox { Width = 250, DisplayMemberPath = "FullName" };
                var patients = DatabaseService.GetAllPatients();
                cmbPatient.ItemsSource = patients;
                cmbPatient.SelectedIndex = 0;
                panel.Children.Add(CreateFormRow("Выберите пациента:", cmbPatient));

                var txtHeight = new TextBox { Width = 200 };
                var txtWeight = new TextBox { Width = 200 };
                var txtHeartRate = new TextBox { Width = 200 };
                var txtCondition = new TextBox { Width = 200 };

                cmbPatient.SelectionChanged += (s, e) =>
                {
                    if (cmbPatient.SelectedItem is Patient patient)
                    {
                        txtHeight.Text = patient.Height?.ToString() ?? "";
                        txtWeight.Text = patient.Weight?.ToString() ?? "";
                        txtHeartRate.Text = patient.HeartRate?.ToString() ?? "";
                        txtCondition.Text = patient.GeneralCondition ?? "";
                    }
                };

                panel.Children.Add(CreateFormRow("Рост (см):", txtHeight));
                panel.Children.Add(CreateFormRow("Вес (кг):", txtWeight));
                panel.Children.Add(CreateFormRow("Сердцебиение (уд/мин):", txtHeartRate));
                panel.Children.Add(CreateFormRow("Общее состояние:", txtCondition));

                var saveBtn = new Button { Content = "Сохранить изменения", Width = 150, Height = 35, Margin = new Thickness(0, 20, 0, 0) };
                saveBtn.Click += (s, e) =>
                {
                    if (cmbPatient.SelectedItem is Patient patient)
                    {
                        patient.Height = string.IsNullOrEmpty(txtHeight.Text) ? null : (double?)double.Parse(txtHeight.Text);
                        patient.Weight = string.IsNullOrEmpty(txtWeight.Text) ? null : (double?)double.Parse(txtWeight.Text);
                        patient.HeartRate = string.IsNullOrEmpty(txtHeartRate.Text) ? null : (int?)int.Parse(txtHeartRate.Text);
                        patient.GeneralCondition = txtCondition.Text;
                        DatabaseService.UpdatePatient(patient);
                        MessageBox.Show("Данные пациента обновлены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                };
                panel.Children.Add(saveBtn);

                var scrollViewer = new ScrollViewer();
                scrollViewer.Content = panel;
                contentGrid.Children.Clear();
                contentGrid.Children.Add(scrollViewer);
            }

            private void ShowCompleteAppointmentForm()
            {
                var panel = new StackPanel { Margin = new Thickness(20) };

                panel.Children.Add(new TextBlock { Text = "Завершение приёма", FontSize = 18, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 0, 20) });

                _appointments = DatabaseService.GetDoctorAppointments(_currentDoctor?.Id ?? 0);
                var activeAppointments = _appointments.Where(a => a.Status != "Завершён").ToList();

                var cmbAppointment = new ComboBox { Width = 300 };
                cmbAppointment.DisplayMemberPath = "Id";
                cmbAppointment.SelectedValuePath = "Id";

                var items = new List<string>();
                foreach (var app in activeAppointments)
                {
                    items.Add($"№{app.Id} - {app.PatientName} - {app.AppointmentDate} {app.AppointmentTime}");
                }
                cmbAppointment.ItemsSource = items;

                if (activeAppointments.Any())
                    cmbAppointment.SelectedIndex = 0;

                panel.Children.Add(CreateFormRow("Выберите запись:", cmbAppointment));

                var txtDiagnosis = new TextBox { Width = 300, Height = 80, TextWrapping = TextWrapping.Wrap };
                panel.Children.Add(CreateFormRow("Диагноз:", txtDiagnosis));

                var completeBtn = new Button { Content = "Завершить приём", Width = 140, Height = 35, Margin = new Thickness(0, 20, 0, 0) };
                completeBtn.Click += (s, e) =>
                {
                    if (cmbAppointment.SelectedIndex >= 0 && activeAppointments.Any())
                    {
                        var selectedAppointment = activeAppointments[cmbAppointment.SelectedIndex];
                        DatabaseService.CompleteAppointmentAndAddToHistory(selectedAppointment, txtDiagnosis.Text);
                        MessageBox.Show("Приём завершён, диагноз сохранён", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        ShowTodayAppointments();
                    }
                };
                panel.Children.Add(completeBtn);

                contentGrid.Children.Clear();
                contentGrid.Children.Add(panel);
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
