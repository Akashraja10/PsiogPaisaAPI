using System;
using System.Collections.Generic;

namespace PsiogPaisaAPI.Models;

public partial class LendBack
{
    public int LendId { get; set; }

    public double PaybackAmount { get; set; }

    public int GroupId { get; set; }

    public int TypeId { get; set; }

    public virtual Group Group { get; set; }

    public virtual PayType Type { get; set; }
}
