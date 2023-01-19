using System;
using System.Collections.Generic;

namespace PsiogPaisaAPI.Models;

public partial class PayType
{
    public int TypeId { get; set; }

    public string Typename { get; set; }

    public virtual ICollection<Group> Groups { get; } = new List<Group>();

    public virtual ICollection<LendBack> LendBacks { get; } = new List<LendBack>();
}
