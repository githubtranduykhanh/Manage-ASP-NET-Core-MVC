using System;
using System.Collections.Generic;

namespace ECommerceMVC.Data;

public partial class DbSize
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<DbProductSize> DbProductSizes { get; set; } = new List<DbProductSize>();
}
