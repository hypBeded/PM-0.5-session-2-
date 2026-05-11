using Bolnisa.Models;
using Bolnisa.Services;
using System.Windows;
using System.Windows.Controls;

namespace Bolnisa.Views
{
    /// <summary>
    /// Логика взаимодействия для RegWindow.xaml
    /// </summary>
    public partial class RegWindow : Window
    {
        private User _currentUser;
        private List<Patient> _patients;
        private List<Doctor> _doctors;

        public RegWindow(User user)
        {
            InitializeComponent();
            _currentUser = user;
            LoadData();
        }

        private void LoadData()
        {
            _patients = DatabaseService.GetAllPatients();
            _doctors = DatabaseService.GetAllDoctors();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void btnPatientsList_Click(object sender, RoutedEventArgs e)
        {
            ShowPatientsList();
        }

        private void btnTodayAppointments_Click(object sender, RoutedEventArgs e)
        {
            ShowTodayAppointments();
        }

        private void btnAddAppointment_Click(object sender, RoutedEventArgs e)
        {
            ShowAddAppointmentForm();
        }

        private void btnAddPatient_Click(object sender, RoutedEventArgs e)
        {
            ShowAddPatientForm();
        }

        private void btnDoctorsList_Click(object sender, RoutedEventArgs e)
        {
            ShowDoctorsList();
        }

        private void btnHistory_Click(object sender, RoutedEventArgs e)
        {
            ShowVisitHistory();
        }

        private void ShowPatientsList()
        {
            var grid = new DataGrid
            {
                Margin = new Thickness(10),
                AutoGenerateColumns = false,
                IsReadOnly = true,
                SelectionMode = DataGridSelectionMode.Single,
                CanUserAddRows = false
            };

            grid.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new System.Windows.Data.Binding("Id"), Width = 50 });
            grid.Columns.Add(new DataGridTextColumn { Header = "ФИО пациента", Binding = new System.Windows.Data.Binding("FullName"), Width = 150 });
            grid.Columns.Add(new DataGridTextColumn { Header = "Дата рождения", Binding = new System.Windows.Data.Binding("BirthDate"), Width = 100 });
            grid.Columns.Add(new DataGridTextColumn { Header = "Номер телефона", Binding = new System.Windows.Data.Binding("Phone"), Width = 120 });
            grid.Columns.Add(new DataGridTextColumn { Header = "Адрес проживания", Binding = new System.Windows.Data.Binding("Address"), Width = 150 });
            grid.Columns.Add(new DataGridTextColumn { Header = "Мед карта", Binding = new System.Windows.Data.Binding("MedicalCardNumber"), Width = 100 });

            grid.ItemsSource = _patients;

            var searchPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(10) };
            var txtSearch = new TextBox { Width = 200, Height = 30, Margin = new Thickness(0, 0, 10, 0), Tag = "Поиск" };
            txtSearch.TextChanged += (s, e) =>
            {
                var searchText = txtSearch.Text;
                if (string.IsNullOrEmpty(searchText))
                    grid.ItemsSource = _patients;
                else
                {
                    var filtered = _patients.Where(p => p.Id.ToString().Contains(searchText) ||
                                                       p.FullName.ToLower().Contains(searchText.ToLower())).ToList();
                    grid.ItemsSource = filtered;
                }
            };

            txtSearch.Text = "";
            txtSearch.SetValue(TextBox.TagProperty, "Введите ID или ФИО");
            searchPanel.Children.Add(txtSearch);

            var mainPanel = new StackPanel();
            mainPanel.Children.Add(searchPanel);
            mainPanel.Children.Add(grid);

            contentGrid.Children.Clear();
            contentGrid.Children.Add(mainPanel);
        }

        private void ShowTodayAppointments()
        {
            var appointments = DatabaseService.GetTodayAppointments();

            var grid = new DataGrid
            {
                Margin = new Thickness(10),
                AutoGenerateColumns = false,
                IsReadOnly = true
            };

            grid.Columns.Add(new DataGridTextColumn { Header = "Время", Binding = new System.Windows.Data.Binding("AppointmentTime"), Width = 80 });
            grid.Columns.Add(new DataGridTextColumn { Header = "ФИО пациента", Binding = new System.Windows.Data.Binding("PatientName"), Width = 180 });
            grid.Columns.Add(new DataGridTextColumn { Header = "ФИО врача", Binding = new System.Windows.Data.Binding("DoctorName"), Width = 180 });
            grid.Columns.Add(new DataGridTextColumn { Header = "Статус", Binding = new System.Windows.Data.Binding("Status"), Width = 100 });

            grid.ItemsSource = appointments;

            contentGrid.Children.Clear();
            contentGrid.Children.Add(grid);
        }

        private void ShowAddAppointmentForm()
        {
            var panel = new StackPanel { Margin = new Thickness(20) };

            panel.Children.Add(new TextBlock { Text = "Добавление записи на приём", FontSize = 18, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 0, 20) });

            var cmbDoctor = new ComboBox { Width = 250, DisplayMemberPath = "FullName" };
            cmbDoctor.ItemsSource = _doctors;
            cmbDoctor.SelectedIndex = 0;
            panel.Children.Add(CreateFormRow("ФИО Врача:", cmbDoctor));

            var cmbPatient = new ComboBox { Width = 250, DisplayMemberPath = "FullName" };
            cmbPatient.ItemsSource = _patients;
            cmbPatient.SelectedIndex = 0;
            panel.Children.Add(CreateFormRow("ФИО пациента:", cmbPatient));

            var dpDate = new DatePicker { Width = 250, SelectedDate = DateTime.Now };
            panel.Children.Add(CreateFormRow("Дата записи:", dpDate));

            var txtTime = new TextBox { Width = 250, Text = "10:00" };
            panel.Children.Add(CreateFormRow("Время записи:", txtTime));

            var txtReason = new TextBox { Width = 250, Height = 60, TextWrapping = TextWrapping.Wrap };
            panel.Children.Add(CreateFormRow("Причина обращения:", txtReason));

            var addBtn = new Button { Content = "Добавить", Width = 120, Height = 35, Margin = new Thickness(0, 20, 0, 0) };
            addBtn.Click += (s, e) =>
            {
                if (cmbDoctor.SelectedItem is Doctor doctor && cmbPatient.SelectedItem is Patient patient)
                {
                    var appointment = new Appointment
                    {
                        PatientId = patient.Id,
                        DoctorId = doctor.Id,
                        AppointmentDate = dpDate.SelectedDate?.ToString("dd.MM.yyyy") ?? DateTime.Now.ToString("dd.MM.yyyy"),
                        AppointmentTime = txtTime.Text,
                        Status = "Запланирован",
                        Reason = txtReason.Text
                    };
                    DatabaseService.AddAppointment(appointment);
                    MessageBox.Show("Запись успешно добавлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    ShowTodayAppointments();
                }
            };
            panel.Children.Add(addBtn);

            contentGrid.Children.Clear();
            contentGrid.Children.Add(panel);
        }

        private void ShowAddPatientForm()
        {
            var scrollViewer = new ScrollViewer();
            var panel = new StackPanel { Margin = new Thickness(20) };

            panel.Children.Add(new TextBlock { Text = "Добавление нового пациента", FontSize = 18, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 0, 20) });

            var txtFullName = new TextBox { Width = 300 };
            var txtPhone = new TextBox { Width = 300 };
            var txtMedicalCard = new TextBox { Width = 300 };
            var dpBirthDate = new DatePicker { Width = 300, SelectedDate = DateTime.Now.AddYears(-30) };
            var txtAddress = new TextBox { Width = 300 };
            var txtHeight = new TextBox { Width = 300 };
            var txtWeight = new TextBox { Width = 300 };
            var txtHeartRate = new TextBox { Width = 300 };
            var txtCondition = new TextBox { Width = 300 };

            panel.Children.Add(CreateFormRow("ФИО пациента:*", txtFullName));
            panel.Children.Add(CreateFormRow("Телефон пациента:*", txtPhone));
            panel.Children.Add(CreateFormRow("Мед карта №:*", txtMedicalCard));
            panel.Children.Add(CreateFormRow("Дата рождения:*", dpBirthDate));
            panel.Children.Add(CreateFormRow("Адрес проживания:*", txtAddress));
            panel.Children.Add(CreateFormRow("Рост (см):", txtHeight));
            panel.Children.Add(CreateFormRow("Вес (кг):", txtWeight));
            panel.Children.Add(CreateFormRow("Сердцебиение (уд/мин):", txtHeartRate));
            panel.Children.Add(CreateFormRow("Общее состояние:", txtCondition));

            var saveBtn = new Button { Content = "Сохранить", Width = 120, Height = 35, Margin = new Thickness(0, 20, 0, 0) };
            saveBtn.Click += (s, e) =>
            {
                if (string.IsNullOrEmpty(txtFullName.Text) || string.IsNullOrEmpty(txtPhone.Text) ||
                    string.IsNullOrEmpty(txtMedicalCard.Text) || string.IsNullOrEmpty(txtAddress.Text))
                {
                    MessageBox.Show("Заполните обязательные поля (*)", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var patient = new Patient
                {
                    FullName = txtFullName.Text,
                    Phone = txtPhone.Text,
                    MedicalCardNumber = txtMedicalCard.Text,
                    BirthDate = dpBirthDate.SelectedDate?.ToString("dd.MM.yyyy") ?? "",
                    Address = txtAddress.Text,
                    Height = string.IsNullOrEmpty(txtHeight.Text) ? null : (double?)double.Parse(txtHeight.Text),
                    Weight = string.IsNullOrEmpty(txtWeight.Text) ? null : (double?)double.Parse(txtWeight.Text),
                    HeartRate = string.IsNullOrEmpty(txtHeartRate.Text) ? null : (int?)int.Parse(txtHeartRate.Text),
                    GeneralCondition = txtCondition.Text
                };

                DatabaseService.AddPatient(patient);
                MessageBox.Show("Пациент успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadData();
                ShowPatientsList();
            };
            panel.Children.Add(saveBtn);

            scrollViewer.Content = panel;
            contentGrid.Children.Clear();
            contentGrid.Children.Add(scrollViewer);
        }

        private void ShowDoctorsList()
        {
            var grid = new DataGrid
            {
                Margin = new Thickness(10),
                AutoGenerateColumns = false,
                IsReadOnly = true
            };

            grid.Columns.Add(new DataGridTextColumn { Header = "ФИО", Binding = new System.Windows.Data.Binding("FullName"), Width = 150 });
            grid.Columns.Add(new DataGridTextColumn { Header = "Должность", Binding = new System.Windows.Data.Binding("Position"), Width = 100 });
            grid.Columns.Add(new DataGridTextColumn { Header = "Специальность", Binding = new System.Windows.Data.Binding("Specialty"), Width = 120 });
            grid.Columns.Add(new DataGridTextColumn { Header = "Дата рождения", Binding = new System.Windows.Data.Binding("BirthDate"), Width = 100 });
            grid.Columns.Add(new DataGridTextColumn { Header = "Номер телефона", Binding = new System.Windows.Data.Binding("Phone"), Width = 120 });

            grid.ItemsSource = _doctors;

            contentGrid.Children.Clear();
            contentGrid.Children.Add(grid);
        }

        private void ShowVisitHistory()
        {
            var history = DatabaseService.GetVisitHistory();

            var grid = new DataGrid
            {
                Margin = new Thickness(10),
                AutoGenerateColumns = false,
                IsReadOnly = true
            };

            grid.Columns.Add(new DataGridTextColumn { Header = "ФИО пациента", Binding = new System.Windows.Data.Binding("PatientName"), Width = 150 });
            grid.Columns.Add(new DataGridTextColumn { Header = "ФИО врача", Binding = new System.Windows.Data.Binding("DoctorName"), Width = 150 });
            grid.Columns.Add(new DataGridTextColumn { Header = "Дата посещения", Binding = new System.Windows.Data.Binding("VisitDate"), Width = 100 });
            grid.Columns.Add(new DataGridTextColumn { Header = "Время посещения", Binding = new System.Windows.Data.Binding("VisitTime"), Width = 80 });
            grid.Columns.Add(new DataGridTextColumn { Header = "Причина посещения", Binding = new System.Windows.Data.Binding("Reason"), Width = 150 });
            grid.Columns.Add(new DataGridTextColumn { Header = "Диагноз", Binding = new System.Windows.Data.Binding("Diagnosis"), Width = 150 });

            grid.ItemsSource = history;

            contentGrid.Children.Clear();
            contentGrid.Children.Add(grid);
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
