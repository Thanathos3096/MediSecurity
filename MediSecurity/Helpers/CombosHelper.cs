using MediSecurity.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediSecurity.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _dataContext;

        public CombosHelper(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IEnumerable<SelectListItem> GetCombosHospitaltype()
        {
            var list = _dataContext.HospitalTypes.Select(hp => new SelectListItem
            {
                Text = hp.Name,
                Value = $"{hp.Id}"

            })
                .OrderBy(hp => hp.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a hospital Type...)",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboPatients()
        {
            var list = _dataContext.Patients.Include(l => l.User).Select(p => new SelectListItem
            {
                Text = p.User.FullNameWithDocument,
                Value = p.Id.ToString()
            }).OrderBy(p => p.Text).ToList();

            return list;
        }

        public IEnumerable<SelectListItem> GetComboRoles()
        {
            var list = new List<SelectListItem>
            {
                new SelectListItem { Value = "0", Text = "(Select a role...)" },
                new SelectListItem { Value = "1", Text = "Patient" }
            };

            return list;
        }
    }
}
