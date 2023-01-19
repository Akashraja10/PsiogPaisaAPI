using System;
using System.Collections.Generic;

namespace PsiogPaisaAPI.Models;

public partial class Request
{
    public int ReqId { get; set; }

    public int EmpId { get; set; }

    public string Purpose { get; set; }

    public double QuotedAmount { get; set; }

    public double RecievedAmount { get; set; }

    public int StatusId { get; set; }

    public virtual Employee Emp { get; set; }

    public virtual ICollection<Group> Groups { get; } = new List<Group>();

    public virtual Status Status { get; set; }
}
