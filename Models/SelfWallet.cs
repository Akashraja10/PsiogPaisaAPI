using System;
using System.Collections.Generic;

namespace PsiogPaisaAPI.Models;

public partial class SelfWallet
{
    public int WalId { get; set; }

    public double? WalletAmount { get; set; }

    public int EmpId { get; set; }

    public int StatusId { get; set; }

    public virtual Employee Emp { get; set; }

    public virtual Status Status { get; set; }
}
