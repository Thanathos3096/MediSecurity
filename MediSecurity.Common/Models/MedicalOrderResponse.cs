using System;
using System.Collections.Generic;
using System.Text;

namespace MediSecurity.Common.Models
{
    public class MedicalOrderResponse
    {
        public int Id { get; set; }

        public string Remarks { get; set; }
        public decimal Price { get; set; }

        public DateTime StartDate { get; set; }

        public DoctorResponse Doctor { get; set; }
    }

}
