using System;
using System.Collections.Generic;

namespace PsiogPaisaAPI.Models;

public partial class MasterAccount
{
    public int MasterId { get; set; }

    public int EmployeeEmpId { get; set; }

    public double TotalAmount { get; set; }

    public virtual Employee EmployeeEmp { get; set; }
}
