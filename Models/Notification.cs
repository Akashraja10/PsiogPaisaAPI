using System;
using System.Collections.Generic;

namespace PsiogPaisaAPI.Models;

public partial class Notification
{
    public int NotificationId { get; set; }

    public string Content { get; set; }

    public int EmpId { get; set; }

    public int NotificationTypeId { get; set; }

    public virtual Employee Emp { get; set; }

    public virtual NotificationType NotificationType { get; set; }
}
