using System;
using System.Collections.Generic;

namespace PsiogPaisaAPI.Models;

public partial class ExternalType
{
    public int TypeId { get; set; }

    public string Typename { get; set; }

    public virtual ICollection<External> Externals { get; } = new List<External>();
}
