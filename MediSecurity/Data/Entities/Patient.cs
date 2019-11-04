using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediSecurity.Data.Entities
{
    public class Patient
    {
        public int Id { get; set; }

        public User User { get; set; }

        public ICollection<MedicalOrder> MedicalOrders { get; set; }
    }
}
