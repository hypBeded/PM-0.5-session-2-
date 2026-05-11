using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnisa.Models
{
    public class VisitHistory
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string VisitDate { get; set; }
        public string VisitTime { get; set; }
        public string Reason { get; set; }
        public string Diagnosis { get; set; }
    }
}
