using System;
using System.Collections.Generic;

namespace ECommerceMVC.Data;

public partial class DbRole
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<DbUser> DbUsers { get; set; } = new List<DbUser>();
}
