using Bolnisa.Database;
using Microsoft.Data.Sqlite;
using System;

namespace Bolnisa.Database
{
    public class DatabaseInitializer
    {
        public static void Initialize()
        {
            CreateTables();
            InsertTestData();
        }

        private static void CreateTables()
        {
            var createUsersTable = @"
                CREATE TABLE IF NOT EXISTS Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Login TEXT NOT NULL UNIQUE,
                    Password TEXT NOT NULL,
                    Role TEXT NOT NULL,
                    FullName TEXT NOT NULL
                )";

            var createPatientsTable = @"
                CREATE TABLE IF NOT EXISTS Patients (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    FullName TEXT NOT NULL,
                    BirthDate TEXT NOT NULL,
                    Phone TEXT NOT NULL,
                    Address TEXT NOT NULL,
                    MedicalCardNumber TEXT NOT NULL UNIQUE,
                    Height REAL,
                    Weight REAL,
                    HeartRate INTEGER,
                    GeneralCondition TEXT
                )";

            var createDoctorsTable = @"
                CREATE TABLE IF NOT EXISTS Doctors (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    FullName TEXT NOT NULL,
                    Position TEXT NOT NULL,
                    Specialty TEXT NOT NULL,
                    BirthDate TEXT NOT NULL,
                    Phone TEXT NOT NULL
                )";

            var createAppointmentsTable = @"
                CREATE TABLE IF NOT EXISTS Appointments (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    PatientId INTEGER NOT NULL,
                    DoctorId INTEGER NOT NULL,
                    AppointmentDate TEXT NOT NULL,
                    AppointmentTime TEXT NOT NULL,
                    Status TEXT NOT NULL,
                    Reason TEXT,
                    FOREIGN KEY(PatientId) REFERENCES Patients(Id),
                    FOREIGN KEY(DoctorId) REFERENCES Doctors(Id)
                )";

            var createVisitHistoryTable = @"
                CREATE TABLE IF NOT EXISTS VisitHistory (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    PatientId INTEGER NOT NULL,
                    DoctorId INTEGER NOT NULL,
                    VisitDate TEXT NOT NULL,
                    VisitTime TEXT NOT NULL,
                    Reason TEXT NOT NULL,
                    Diagnosis TEXT,
                    FOREIGN KEY(PatientId) REFERENCES Patients(Id),
                    FOREIGN KEY(DoctorId) REFERENCES Doctors(Id)
                )";

            DatabaseHelper.ExecuteNonQuery(createUsersTable);
            DatabaseHelper.ExecuteNonQuery(createPatientsTable);
            DatabaseHelper.ExecuteNonQuery(createDoctorsTable);
            DatabaseHelper.ExecuteNonQuery(createAppointmentsTable);
            DatabaseHelper.ExecuteNonQuery(createVisitHistoryTable);
        }

        private static void InsertTestData()
        {
            // Проверяем, есть ли уже данные
            var count = DatabaseHelper.ExecuteScalar<int>("SELECT COUNT(*) FROM Users");
            if (count > 0) return;

            // Добавляем пользователей
            var insertUser = @"
                INSERT INTO Users (Login, Password, Role, FullName) 
                VALUES (@login, @password, @role, @fullName)";

            DatabaseHelper.ExecuteNonQuery(insertUser, cmd =>
            {
                cmd.Parameters.AddWithValue("@login", "admin");
                cmd.Parameters.AddWithValue("@password", "admin");
                cmd.Parameters.AddWithValue("@role", "Регистратор");
                cmd.Parameters.AddWithValue("@fullName", "Администратор Системы");
            });

            DatabaseHelper.ExecuteNonQuery(insertUser, cmd =>
            {
                cmd.Parameters.AddWithValue("@login", "doctor");
                cmd.Parameters.AddWithValue("@password", "doctor");
                cmd.Parameters.AddWithValue("@role", "Врач");
                cmd.Parameters.AddWithValue("@fullName", "Иванов Антон Алексеевич");
            });

            DatabaseHelper.ExecuteNonQuery(insertUser, cmd =>
            {
                cmd.Parameters.AddWithValue("@login", "nurse");
                cmd.Parameters.AddWithValue("@password", "nurse");
                cmd.Parameters.AddWithValue("@role", "Медсестра");
                cmd.Parameters.AddWithValue("@fullName", "Петрова Елена Сергеевна");
            });

            // Добавляем врачей
            var insertDoctor = @"
                INSERT INTO Doctors (FullName, Position, Specialty, BirthDate, Phone) 
                VALUES (@fullName, @position, @specialty, @birthDate, @phone)";

            DatabaseHelper.ExecuteNonQuery(insertDoctor, cmd =>
            {
                cmd.Parameters.AddWithValue("@fullName", "Иванов Антон Алексеевич");
                cmd.Parameters.AddWithValue("@position", "Врач");
                cmd.Parameters.AddWithValue("@specialty", "Терапевт");
                cmd.Parameters.AddWithValue("@birthDate", "15.03.1975");
                cmd.Parameters.AddWithValue("@phone", "+7(999)111-22-33");
            });

            DatabaseHelper.ExecuteNonQuery(insertDoctor, cmd =>
            {
                cmd.Parameters.AddWithValue("@fullName", "Петров Борис Борисович");
                cmd.Parameters.AddWithValue("@position", "Врач");
                cmd.Parameters.AddWithValue("@specialty", "Хирург");
                cmd.Parameters.AddWithValue("@birthDate", "22.08.1980");
                cmd.Parameters.AddWithValue("@phone", "+7(999)444-55-66");
            });

            // Добавляем пациентов
            var insertPatient = @"
                INSERT INTO Patients (FullName, BirthDate, Phone, Address, MedicalCardNumber, Height, Weight, HeartRate, GeneralCondition) 
                VALUES (@fullName, @birthDate, @phone, @address, @medicalCard, @height, @weight, @heartRate, @condition)";

            DatabaseHelper.ExecuteNonQuery(insertPatient, cmd =>
            {
                cmd.Parameters.AddWithValue("@fullName", "Иванов Иван Иванович");
                cmd.Parameters.AddWithValue("@birthDate", "15.05.1980");
                cmd.Parameters.AddWithValue("@phone", "+7(999)123-45-67");
                cmd.Parameters.AddWithValue("@address", "ул. Ленина 10, кв.5");
                cmd.Parameters.AddWithValue("@medicalCard", "MC-001");
                cmd.Parameters.AddWithValue("@height", 178.5);
                cmd.Parameters.AddWithValue("@weight", 82.3);
                cmd.Parameters.AddWithValue("@heartRate", 72);
                cmd.Parameters.AddWithValue("@condition", "Удовлетворительное");
            });

            DatabaseHelper.ExecuteNonQuery(insertPatient, cmd =>
            {
                cmd.Parameters.AddWithValue("@fullName", "Петрова Мария Сергеевна");
                cmd.Parameters.AddWithValue("@birthDate", "22.11.1995");
                cmd.Parameters.AddWithValue("@phone", "+7(999)234-56-78");
                cmd.Parameters.AddWithValue("@address", "ул. Гагарина 25");
                cmd.Parameters.AddWithValue("@medicalCard", "MC-002");
                cmd.Parameters.AddWithValue("@height", 165.0);
                cmd.Parameters.AddWithValue("@weight", 58.5);
                cmd.Parameters.AddWithValue("@heartRate", 75);
                cmd.Parameters.AddWithValue("@condition", "Хорошее");
            });

            // Добавляем записи на приём
            var insertAppointment = @"
                INSERT INTO Appointments (PatientId, DoctorId, AppointmentDate, AppointmentTime, Status, Reason) 
                VALUES (@patientId, @doctorId, @date, @time, @status, @reason)";

            DatabaseHelper.ExecuteNonQuery(insertAppointment, cmd =>
            {
                cmd.Parameters.AddWithValue("@patientId", 1);
                cmd.Parameters.AddWithValue("@doctorId", 1);
                cmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("dd.MM.yyyy"));
                cmd.Parameters.AddWithValue("@time", "10:00");
                cmd.Parameters.AddWithValue("@status", "Запланирован");
                cmd.Parameters.AddWithValue("@reason", "Профилактический осмотр");
            });

            DatabaseHelper.ExecuteNonQuery(insertAppointment, cmd =>
            {
                cmd.Parameters.AddWithValue("@patientId", 2);
                cmd.Parameters.AddWithValue("@doctorId", 2);
                cmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("dd.MM.yyyy"));
                cmd.Parameters.AddWithValue("@time", "11:30");
                cmd.Parameters.AddWithValue("@status", "Запланирован");
                cmd.Parameters.AddWithValue("@reason", "Консультация");
            });
        }
    }
}