using System;
using System.Collections.Generic;

namespace PsiogPaisaAPI.Models;

public partial class NotificationType
{
    public int NotificationTypeId { get; set; }

    public string Description { get; set; }

    public virtual ICollection<Notification> Notifications { get; } = new List<Notification>();
}
