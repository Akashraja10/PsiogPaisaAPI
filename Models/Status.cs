using System;
using System.Collections.Generic;

namespace PsiogPaisaAPI.Models;

public partial class Status
{
    public int StatusId { get; set; }

    public string Message { get; set; }

    public string Title { get; set; }

    public virtual ICollection<Group> Groups { get; } = new List<Group>();

    public virtual ICollection<Individual> Individuals { get; } = new List<Individual>();

    public virtual ICollection<Request> Requests { get; } = new List<Request>();

    public virtual ICollection<SelfWallet> SelfWallets { get; } = new List<SelfWallet>();

    public virtual ICollection<Statement> Statements { get; } = new List<Statement>();
}
