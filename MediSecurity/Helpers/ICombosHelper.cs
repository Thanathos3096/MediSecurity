using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MediSecurity.Helpers
{
    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetCombosHospitaltype();

        IEnumerable<SelectListItem> GetComboPatients();

        IEnumerable<SelectListItem> GetComboRoles();

    }
}