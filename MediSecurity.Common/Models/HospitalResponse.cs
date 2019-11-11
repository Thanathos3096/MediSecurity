using System;
using System.Collections.Generic;
using System.Text;

namespace MediSecurity.Common.Models
{
    public class HospitalResponse
    {
        public int Id { get; set; }

        public string Neighborhood { get; set; }

        public string Address { get; set; }
        public string Remarks { get; set; }
        public int Stratum { get; set; }
        public bool HasParkingLot { get; set; }
    }
}
