using Bolnisa.Database;
using Bolnisa.Models;
using Microsoft.Data.Sqlite;

namespace Bolnisa.Services
{
    public class DatabaseService
    {
        public static User GetUser(string login, string password)
        {
            var query = "SELECT Id, Login, Password, Role, FullName FROM Users WHERE Login = @login AND Password = @password";
            User user = null;

            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@login", login);
                    command.Parameters.AddWithValue("@password", password);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new User
                            {
                                Id = reader.GetInt32(0),
                                Login = reader.GetString(1),
                                Password = reader.GetString(2),
                                Role = reader.GetString(3),
                                FullName = reader.GetString(4)
                            };
                        }
                    }
                }
            }
            return user;
        }

        public static List<Patient> GetAllPatients()
        {
            var patients = new List<Patient>();
            var query = "SELECT Id, FullName, BirthDate, Phone, Address, MedicalCardNumber, Height, Weight, HeartRate, GeneralCondition FROM Patients";

            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                using (var command = new SqliteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        patients.Add(new Patient
                        {
                            Id = reader.GetInt32(0),
                            FullName = reader.GetString(1),
                            BirthDate = reader.GetString(2),
                            Phone = reader.GetString(3),
                            Address = reader.GetString(4),
                            MedicalCardNumber = reader.GetString(5),
                            Height = reader.IsDBNull(6) ? null : (double?)reader.GetDouble(6),
                            Weight = reader.IsDBNull(7) ? null : (double?)reader.GetDouble(7),
                            HeartRate = reader.IsDBNull(8) ? null : (int?)reader.GetInt32(8),
                            GeneralCondition = reader.IsDBNull(9) ? null : reader.GetString(9)
                        });
                    }
                }
            }
            return patients;
        }

        public static List<Patient> SearchPatients(string searchText)
        {
            var patients = new List<Patient>();
            var query = "SELECT Id, FullName, BirthDate, Phone, Address, MedicalCardNumber, Height, Weight, HeartRate, GeneralCondition FROM Patients WHERE Id LIKE @search OR FullName LIKE @search";

            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@search", $"%{searchText}%");
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            patients.Add(new Patient
                            {
                                Id = reader.GetInt32(0),
                                FullName = reader.GetString(1),
                                BirthDate = reader.GetString(2),
                                Phone = reader.GetString(3),
                                Address = reader.GetString(4),
                                MedicalCardNumber = reader.GetString(5),
                                Height = reader.IsDBNull(6) ? null : (double?)reader.GetDouble(6),
                                Weight = reader.IsDBNull(7) ? null : (double?)reader.GetDouble(7),
                                HeartRate = reader.IsDBNull(8) ? null : (int?)reader.GetInt32(8),
                                GeneralCondition = reader.IsDBNull(9) ? null : reader.GetString(9)
                            });
                        }
                    }
                }
            }
            return patients;
        }

        public static void AddPatient(Patient patient)
        {
            var query = @"
                INSERT INTO Patients (FullName, BirthDate, Phone, Address, MedicalCardNumber, Height, Weight, HeartRate, GeneralCondition) 
                VALUES (@fullName, @birthDate, @phone, @address, @medicalCard, @height, @weight, @heartRate, @condition)";

            DatabaseHelper.ExecuteNonQuery(query, cmd =>
            {
                cmd.Parameters.AddWithValue("@fullName", patient.FullName);
                cmd.Parameters.AddWithValue("@birthDate", patient.BirthDate);
                cmd.Parameters.AddWithValue("@phone", patient.Phone);
                cmd.Parameters.AddWithValue("@address", patient.Address);
                cmd.Parameters.AddWithValue("@medicalCard", patient.MedicalCardNumber);
                cmd.Parameters.AddWithValue("@height", patient.Height.HasValue ? (object)patient.Height.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@weight", patient.Weight.HasValue ? (object)patient.Weight.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@heartRate", patient.HeartRate.HasValue ? (object)patient.HeartRate.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@condition", patient.GeneralCondition ?? (object)DBNull.Value);
            });
        }

        public static void UpdatePatient(Patient patient)
        {
            var query = @"
                UPDATE Patients SET 
                    FullName = @fullName,
                    BirthDate = @birthDate,
                    Phone = @phone,
                    Address = @address,
                    MedicalCardNumber = @medicalCard,
                    Height = @height,
                    Weight = @weight,
                    HeartRate = @heartRate,
                    GeneralCondition = @condition
                WHERE Id = @id";

            DatabaseHelper.ExecuteNonQuery(query, cmd =>
            {
                cmd.Parameters.AddWithValue("@id", patient.Id);
                cmd.Parameters.AddWithValue("@fullName", patient.FullName);
                cmd.Parameters.AddWithValue("@birthDate", patient.BirthDate);
                cmd.Parameters.AddWithValue("@phone", patient.Phone);
                cmd.Parameters.AddWithValue("@address", patient.Address);
                cmd.Parameters.AddWithValue("@medicalCard", patient.MedicalCardNumber);
                cmd.Parameters.AddWithValue("@height", patient.Height.HasValue ? (object)patient.Height.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@weight", patient.Weight.HasValue ? (object)patient.Weight.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@heartRate", patient.HeartRate.HasValue ? (object)patient.HeartRate.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@condition", patient.GeneralCondition ?? (object)DBNull.Value);
            });
        }

       
        public static List<Doctor> GetAllDoctors()
        {
            var doctors = new List<Doctor>();
            var query = "SELECT Id, FullName, Position, Specialty, BirthDate, Phone FROM Doctors";

            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                using (var command = new SqliteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        doctors.Add(new Doctor
                        {
                            Id = reader.GetInt32(0),
                            FullName = reader.GetString(1),
                            Position = reader.GetString(2),
                            Specialty = reader.GetString(3),
                            BirthDate = reader.GetString(4),
                            Phone = reader.GetString(5)
                        });
                    }
                }
            }
            return doctors;
        }


        public static List<Appointment> GetTodayAppointments()
        {
            var appointments = new List<Appointment>();
            var today = DateTime.Now.ToString("dd.MM.yyyy");
            var query = @"
                SELECT a.Id, a.PatientId, p.FullName, a.DoctorId, d.FullName, a.AppointmentDate, a.AppointmentTime, a.Status, a.Reason 
                FROM Appointments a
                JOIN Patients p ON a.PatientId = p.Id
                JOIN Doctors d ON a.DoctorId = d.Id
                WHERE a.AppointmentDate = @today";

            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@today", today);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            appointments.Add(new Appointment
                            {
                                Id = reader.GetInt32(0),
                                PatientId = reader.GetInt32(1),
                                PatientName = reader.GetString(2),
                                DoctorId = reader.GetInt32(3),
                                DoctorName = reader.GetString(4),
                                AppointmentDate = reader.GetString(5),
                                AppointmentTime = reader.GetString(6),
                                Status = reader.GetString(7),
                                Reason = reader.IsDBNull(8) ? null : reader.GetString(8)
                            });
                        }
                    }
                }
            }
            return appointments;
        }

        public static List<Appointment> GetDoctorAppointments(int doctorId)
        {
            var appointments = new List<Appointment>();
            var query = @"
                SELECT a.Id, a.PatientId, p.FullName, a.DoctorId, d.FullName, a.AppointmentDate, a.AppointmentTime, a.Status, a.Reason 
                FROM Appointments a
                JOIN Patients p ON a.PatientId = p.Id
                JOIN Doctors d ON a.DoctorId = d.Id
                WHERE a.DoctorId = @doctorId AND a.AppointmentDate >= @today
                ORDER BY a.AppointmentDate, a.AppointmentTime";

            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@doctorId", doctorId);
                    command.Parameters.AddWithValue("@today", DateTime.Now.ToString("dd.MM.yyyy"));
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            appointments.Add(new Appointment
                            {
                                Id = reader.GetInt32(0),
                                PatientId = reader.GetInt32(1),
                                PatientName = reader.GetString(2),
                                DoctorId = reader.GetInt32(3),
                                DoctorName = reader.GetString(4),
                                AppointmentDate = reader.GetString(5),
                                AppointmentTime = reader.GetString(6),
                                Status = reader.GetString(7),
                                Reason = reader.IsDBNull(8) ? null : reader.GetString(8)
                            });
                        }
                    }
                }
            }
            return appointments;
        }

        public static void AddAppointment(Appointment appointment)
        {
            var query = @"
                INSERT INTO Appointments (PatientId, DoctorId, AppointmentDate, AppointmentTime, Status, Reason) 
                VALUES (@patientId, @doctorId, @date, @time, @status, @reason)";

            DatabaseHelper.ExecuteNonQuery(query, cmd =>
            {
                cmd.Parameters.AddWithValue("@patientId", appointment.PatientId);
                cmd.Parameters.AddWithValue("@doctorId", appointment.DoctorId);
                cmd.Parameters.AddWithValue("@date", appointment.AppointmentDate);
                cmd.Parameters.AddWithValue("@time", appointment.AppointmentTime);
                cmd.Parameters.AddWithValue("@status", appointment.Status ?? "Запланирован");
                cmd.Parameters.AddWithValue("@reason", appointment.Reason ?? (object)DBNull.Value);
            });
        }

        public static void UpdateAppointmentStatus(int appointmentId, string status)
        {
            var query = "UPDATE Appointments SET Status = @status WHERE Id = @id";

            DatabaseHelper.ExecuteNonQuery(query, cmd =>
            {
                cmd.Parameters.AddWithValue("@id", appointmentId);
                cmd.Parameters.AddWithValue("@status", status);
            });
        }

        public static void CompleteAppointmentAndAddToHistory(Appointment appointment, string diagnosis)
        {
            var historyQuery = @"
                INSERT INTO VisitHistory (PatientId, DoctorId, VisitDate, VisitTime, Reason, Diagnosis) 
                VALUES (@patientId, @doctorId, @date, @time, @reason, @diagnosis)";

            DatabaseHelper.ExecuteNonQuery(historyQuery, cmd =>
            {
                cmd.Parameters.AddWithValue("@patientId", appointment.PatientId);
                cmd.Parameters.AddWithValue("@doctorId", appointment.DoctorId);
                cmd.Parameters.AddWithValue("@date", appointment.AppointmentDate);
                cmd.Parameters.AddWithValue("@time", appointment.AppointmentTime);
                cmd.Parameters.AddWithValue("@reason", appointment.Reason ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@diagnosis", diagnosis ?? (object)DBNull.Value);
            });

            UpdateAppointmentStatus(appointment.Id, "Завершён");
        }

        public static List<VisitHistory> GetVisitHistory()
        {
            var history = new List<VisitHistory>();
            var query = @"
                SELECT h.Id, h.PatientId, p.FullName, h.DoctorId, d.FullName, h.VisitDate, h.VisitTime, h.Reason, h.Diagnosis
                FROM VisitHistory h
                JOIN Patients p ON h.PatientId = p.Id
                JOIN Doctors d ON h.DoctorId = d.Id
                ORDER BY h.VisitDate DESC, h.VisitTime DESC";

            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                using (var command = new SqliteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        history.Add(new VisitHistory
                        {
                            Id = reader.GetInt32(0),
                            PatientId = reader.GetInt32(1),
                            PatientName = reader.GetString(2),
                            DoctorId = reader.GetInt32(3),
                            DoctorName = reader.GetString(4),
                            VisitDate = reader.GetString(5),
                            VisitTime = reader.GetString(6),
                            Reason = reader.IsDBNull(7) ? null : reader.GetString(7),
                            Diagnosis = reader.IsDBNull(8) ? null : reader.GetString(8)
                        });
                    }
                }
            }
            return history;
        }
    }
}