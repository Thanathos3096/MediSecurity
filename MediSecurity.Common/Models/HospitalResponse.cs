using System.Collections.Generic;

namespace MediSecurity.Common.Models
{
    public class HospitalResponse
    {
        public int Id { get; set; }

        public string Neighborhood { get; set; }

        public string Address { get; set; }


        public int Stratum { get; set; }

        public bool HasParkingLot { get; set; }

        public bool IsAvailable { get; set; }

        public string Remarks { get; set; }

        public string PropertyType { get; set; }

        public ICollection<HospitalImageResponse> HospitalImages { get; set; }

        public ICollection<MedicalOrderResponse> OrderMedical { get; set; }

    }
}
