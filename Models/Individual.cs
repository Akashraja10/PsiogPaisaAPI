using System;
using System.Collections.Generic;

namespace PsiogPaisaAPI.Models;

public partial class Individual
{
    public int IndId { get; set; }

    public int SenderId { get; set; }

    public int RecieverId { get; set; }

    public int TypeId { get; set; }

    public double Amount { get; set; }

    public DateTime Time { get; set; }

    public int StatusId { get; set; }

    public virtual Employee Reciever { get; set; }

    public virtual Employee Sender { get; set; }

    public virtual ICollection<Statement> Statements { get; } = new List<Statement>();

    public virtual Status Status { get; set; }

    public virtual IndividualType Type { get; set; }
}
