using System;
using System.Collections.Generic;

namespace ECommerceMVC.Data;

public partial class DbGroup
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? IdGroupParent { get; set; }

    public string Image { get; set; } = null!;

    public int DisplayOrder { get; set; }

    public int DisplayQuantity { get; set; }

    public virtual ICollection<DbProduct> DbProducts { get; set; } = new List<DbProduct>();
}
