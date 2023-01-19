using System;
using System.Collections.Generic;

namespace PsiogPaisaAPI.Models;

public partial class Group
{
   

    public int GroupId { get; set; }

    public int ReqId { get; set; }

    public int ContributorId { get; set; }

    public double Amount { get; set; }

    public int StatusId { get; set; }

    public int TypeId { get; set; }

    public virtual Employee Contributor { get; set; }

    public virtual ICollection<LendBack> LendBacks { get; } = new List<LendBack>();

    public virtual Request Req { get; set; }

    public virtual ICollection<Statement> Statements { get; } = new List<Statement>();

    public virtual Status Status { get; set; }

    public virtual PayType Type { get; set; }
  
}
