using System;
using System.Collections.Generic;

namespace PsiogPaisaAPI.Models;

public partial class Employee
{
    internal string Token;

    public int EmpId { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public string EmpFname { get; set; }

    public string EmpLname { get; set; }

    public string Email { get; set; }

    public int Age { get; set; }

    public string Gender { get; set; }

    public virtual ICollection<External> Externals { get; } = new List<External>();

    public virtual ICollection<Group> Groups { get; } = new List<Group>();

    public virtual ICollection<Individual> IndividualRecievers { get; } = new List<Individual>();

    public virtual ICollection<Individual> IndividualSenders { get; } = new List<Individual>();

    public virtual ICollection<MasterAccount> MasterAccounts { get; } = new List<MasterAccount>();

    public virtual ICollection<Notification> Notifications { get; } = new List<Notification>();

    public virtual ICollection<Request> Requests { get; } = new List<Request>();

    public virtual ICollection<SelfWallet> SelfWallets { get; } = new List<SelfWallet>();
}
