using System;
using System.Collections.Generic;

namespace ECommerceMVC.Domain.Entities;

public partial class DbColor
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<DbProductColor> DbProductColors { get; set; } = new List<DbProductColor>();
}
