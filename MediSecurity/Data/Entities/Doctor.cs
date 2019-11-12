using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediSecurity.Common.Models;

namespace MediSecurity.Data.Entities
{
    public class Doctor
    {
        public int Id { get; set; }
        public User User { get; set; }

        public ICollection<MedicalOrder> MedicalOrders { get; set; }

        internal DoctorResponse Select(Func<object, DoctorResponse> p)
        {
            throw new NotImplementedException();
        }
    }
}
