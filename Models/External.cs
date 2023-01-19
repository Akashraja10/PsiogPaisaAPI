using System;
using System.Collections.Generic;

namespace PsiogPaisaAPI.Models;

public partial class External
{
    public int ExtId { get; set; }

    public double Amount { get; set; }

    public string Content { get; set; }

    public int TypeId { get; set; }

    public int EmpId { get; set; }

    public virtual Employee Emp { get; set; }

    public virtual ICollection<Statement> Statements { get; } = new List<Statement>();

    public virtual ExternalType Type { get; set; }
}
