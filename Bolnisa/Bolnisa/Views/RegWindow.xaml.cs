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
    /// Логика взаимодействия для RegWindow.xaml
    /// </summary>
        public partial class RegWindow : Window
        {
            public RegWindow()
            {
                InitializeComponent();
            }

            private void btnLogout_Click(object sender, RoutedEventArgs e)
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }

            private void btnPatientsList_Click(object sender, RoutedEventArgs e)
            {
                var grid = CreatePatientsGrid();
                contentGrid.Children.Clear();
                contentGrid.Children.Add(grid);
            }

            private void btnTodayAppointments_Click(object sender, RoutedEventArgs e)
            {
                var grid = CreateTodayAppointmentsGrid();
                contentGrid.Children.Clear();
                contentGrid.Children.Add(grid);
            }

            private void btnAddAppointment_Click(object sender, RoutedEventArgs e)
            {
                var panel = CreateAddAppointmentPanel();
                contentGrid.Children.Clear();
                contentGrid.Children.Add(panel);
            }

            private void btnAddPatient_Click(object sender, RoutedEventArgs e)
            {
                var panel = CreateAddPatientPanel();
                contentGrid.Children.Clear();
                contentGrid.Children.Add(panel);
            }

            private void btnDoctorsList_Click(object sender, RoutedEventArgs e)
            {
                var grid = CreateDoctorsGrid();
                contentGrid.Children.Clear();
                contentGrid.Children.Add(grid);
            }

            private void btnHistory_Click(object sender, RoutedEventArgs e)
            {
                var grid = CreateHistoryGrid();
                contentGrid.Children.Clear();
                contentGrid.Children.Add(grid);
            }

            private DataGrid CreatePatientsGrid()
            {
                var grid = new DataGrid
                {
                    Margin = new Thickness(10),
                    AutoGenerateColumns = false
                };

                grid.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new System.Windows.Data.Binding("Id"), Width = 50 });
                grid.Columns.Add(new DataGridTextColumn { Header = "ФИО пациента", Binding = new System.Windows.Data.Binding("FullName"), Width = 150 });
                grid.Columns.Add(new DataGridTextColumn { Header = "Дата рождения", Binding = new System.Windows.Data.Binding("BirthDate"), Width = 100 });
                grid.Columns.Add(new DataGridTextColumn { Header = "Номер телефона", Binding = new System.Windows.Data.Binding("Phone"), Width = 120 });
                grid.Columns.Add(new DataGridTextColumn { Header = "Адрес проживания", Binding = new System.Windows.Data.Binding("Address"), Width = 150 });
                grid.Columns.Add(new DataGridTextColumn { Header = "Мед карта", Binding = new System.Windows.Data.Binding("MedicalCard"), Width = 100 });

                // Заглушка данных
                var patients = new List<Patient>
            {
                new Patient { Id = 1, FullName = "Иванов Иван Иванович", BirthDate = "15.05.1980", Phone = "+7(999)123-45-67", Address = "ул. Ленина 10, кв.5", MedicalCard = "MC-001" },
                new Patient { Id = 2, FullName = "Петрова Мария Сергеевна", BirthDate = "22.11.1995", Phone = "+7(999)234-56-78", Address = "ул. Гагарина 25", MedicalCard = "MC-002" }
            };
                grid.ItemsSource = patients;

                // Панель поиска
                var searchPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(10) };
                searchPanel.Children.Add(new TextBox { Name = "txtSearch", Width = 200, Height = 30, Margin = new Thickness(0, 0, 10, 0), ToolTip = "ID или ФИО пациента" });
                var searchBtn = new Button { Content = "Найти", Width = 80, Height = 30 };
                searchBtn.Click += (s, e) =>
                {
                    // Логика поиска
                    MessageBox.Show("Поиск пациентов");
                };
                searchPanel.Children.Add(searchBtn);

                var mainPanel = new StackPanel();
                mainPanel.Children.Add(searchPanel);
                mainPanel.Children.Add(grid);

                return null; // Упрощённо - вернём только грид
            }

            private StackPanel CreateAddPatientPanel()
            {
                var panel = new StackPanel { Margin = new Thickness(20) };

                panel.Children.Add(new TextBlock { Text = "Добавление нового пациента", FontSize = 18, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 0, 20) });

                panel.Children.Add(CreateFormRow("ФИО пациента:", new TextBox { Name = "txtFullName", Width = 250 }));
                panel.Children.Add(CreateFormRow("Телефон пациента:", new TextBox { Name = "txtPhone", Width = 250 }));
                panel.Children.Add(CreateFormRow("Мед карта пациента:", new TextBox { Name = "txtMedicalCard", Width = 250 }));
                panel.Children.Add(CreateFormRow("День рождения:", new DatePicker { Name = "dpBirthDate", Width = 250 }));
                panel.Children.Add(CreateFormRow("Мед карта (№):", new TextBox { Name = "txtCardNumber", Width = 250 }));
                panel.Children.Add(CreateFormRow("Адрес проживания:", new TextBox { Name = "txtAddress", Width = 250 }));

                var saveBtn = new Button { Content = "Сохранить", Width = 120, Height = 35, Margin = new Thickness(0, 20, 0, 0) };
                saveBtn.Click += (s, e) => MessageBox.Show("Пациент сохранён");
                panel.Children.Add(saveBtn);

                return panel;
            }

            private StackPanel CreateAddAppointmentPanel()
            {
                var panel = new StackPanel { Margin = new Thickness(20) };

                panel.Children.Add(new TextBlock { Text = "Добавление записи на приём", FontSize = 18, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 0, 20) });

                var cmbDoctor = new ComboBox { Width = 250 };
                cmbDoctor.Items.Add("Иванов А.А. - Терапевт");
                cmbDoctor.Items.Add("Петров Б.Б. - Хирург");
                panel.Children.Add(CreateFormRow("ФИО Врача:", cmbDoctor));

                var cmbPatient = new ComboBox { Width = 250 };
                cmbPatient.Items.Add("Иванов И.И.");
                cmbPatient.Items.Add("Петрова М.С.");
                panel.Children.Add(CreateFormRow("ФИО пациента:", cmbPatient));

                panel.Children.Add(CreateFormRow("Дата записи:", new DatePicker { Name = "dpDate", Width = 250 }));
                panel.Children.Add(CreateFormRow("Время записи:", new TextBox { Name = "txtTime", Width = 250, Text = "10:00" }));

                var addBtn = new Button { Content = "Добавить", Width = 120, Height = 35, Margin = new Thickness(0, 20, 0, 0) };
                addBtn.Click += (s, e) => MessageBox.Show("Запись добавлена");
                panel.Children.Add(addBtn);

                return panel;
            }

            private StackPanel CreateFormRow(string label, FrameworkElement control)
            {
                var stack = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 5,0,0) };
                stack.Children.Add(new TextBlock { Text = label, Width = 120, VerticalAlignment = VerticalAlignment.Center });
                stack.Children.Add(control);
                return stack;
            }

            private DataGrid CreateDoctorsGrid()
            {
                var grid = new DataGrid
                {
                    Margin = new Thickness(10),
                    AutoGenerateColumns = false
                };

                grid.Columns.Add(new DataGridTextColumn { Header = "ФИО", Binding = new System.Windows.Data.Binding("FullName"), Width = 150 });
                grid.Columns.Add(new DataGridTextColumn { Header = "Должность", Binding = new System.Windows.Data.Binding("Position"), Width = 120 });
                grid.Columns.Add(new DataGridTextColumn { Header = "Специальность", Binding = new System.Windows.Data.Binding("Specialty"), Width = 120 });
                grid.Columns.Add(new DataGridTextColumn { Header = "Дата рождения", Binding = new System.Windows.Data.Binding("BirthDate"), Width = 100 });
                grid.Columns.Add(new DataGridTextColumn { Header = "Номер телефона", Binding = new System.Windows.Data.Binding("Phone"), Width = 120 });

                var doctors = new List<Doctor>
            {
                new Doctor { FullName = "Иванов А.А.", Position = "Врач", Specialty = "Терапевт", BirthDate = "10.05.1975", Phone = "+7(999)111-22-33" },
                new Doctor { FullName = "Петров Б.Б.", Position = "Врач", Specialty = "Хирург", BirthDate = "20.08.1980", Phone = "+7(999)444-55-66" }
            };
                grid.ItemsSource = doctors;

                return grid;
            }

            private DataGrid CreateHistoryGrid()
            {
                var grid = new DataGrid
                {
                    Margin = new Thickness(10),
                    AutoGenerateColumns = false
                };

                grid.Columns.Add(new DataGridTextColumn { Header = "ФИО пациента", Binding = new System.Windows.Data.Binding("PatientName"), Width = 150 });
                grid.Columns.Add(new DataGridTextColumn { Header = "ФИО врача", Binding = new System.Windows.Data.Binding("DoctorName"), Width = 150 });
                grid.Columns.Add(new DataGridTextColumn { Header = "Дата посещения", Binding = new System.Windows.Data.Binding("Date"), Width = 100 });
                grid.Columns.Add(new DataGridTextColumn { Header = "Время посещения", Binding = new System.Windows.Data.Binding("Time"), Width = 80 });
                grid.Columns.Add(new DataGridTextColumn { Header = "Причина посещения", Binding = new System.Windows.Data.Binding("Reason"), Width = 200 });

                return grid;
            }

            private DataGrid CreateTodayAppointmentsGrid()
            {
                var grid = new DataGrid
                {
                    Margin = new Thickness(10),
                    AutoGenerateColumns = false
                };

                grid.Columns.Add(new DataGridTextColumn { Header = "Время", Binding = new System.Windows.Data.Binding("Time"), Width = 80 });
                grid.Columns.Add(new DataGridTextColumn { Header = "ФИО пациента", Binding = new System.Windows.Data.Binding("PatientName"), Width = 150 });
                grid.Columns.Add(new DataGridTextColumn { Header = "ФИО врача", Binding = new System.Windows.Data.Binding("DoctorName"), Width = 150 });

                return grid;
            }
        }

        // Модели
        public class Patient
        {
            public int Id { get; set; }
            public string FullName { get; set; }
            public string BirthDate { get; set; }
            public string Phone { get; set; }
            public string Address { get; set; }
            public string MedicalCard { get; set; }
        }

        public class Doctor
        {
            public string FullName { get; set; }
            public string Position { get; set; }
            public string Specialty { get; set; }
            public string BirthDate { get; set; }
            public string Phone { get; set; }
        }
    }
