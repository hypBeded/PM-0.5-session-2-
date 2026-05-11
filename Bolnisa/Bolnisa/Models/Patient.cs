using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnisa.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string BirthDate { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string MedicalCardNumber { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        public int? HeartRate { get; set; }
        public string GeneralCondition { get; set; }
    }
}
