using System;
using System.Collections.Generic;

namespace PsiogPaisaAPI.Models;

public partial class Statement
{
    public int TransId { get; set; }

    public string Typename { get; set; }

    public int IndId { get; set; }

    public int ExtId { get; set; }

    public int GroupId { get; set; }

    public int StatusId { get; set; }

    public double Amount { get; set; }

    public string CdOrDb { get; set; }

    public virtual External Ext { get; set; }

    public virtual Group Group { get; set; }

    public virtual Individual Ind { get; set; }

    public virtual Status Status { get; set; }
}
