using System;
using System.Collections.Generic;

namespace PsiogPaisaAPI.Models;

public partial class IndividualType
{
    public int TypeId { get; set; }

    public string Typename { get; set; }

    public virtual ICollection<Individual> Individuals { get; } = new List<Individual>();
}
