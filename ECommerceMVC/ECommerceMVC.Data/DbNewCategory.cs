using System;
using System.Collections.Generic;

namespace ECommerceMVC.Data;

public partial class DbNewCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Image { get; set; } = null!;

    public int DisplayOrder { get; set; }

    public int? IdNewParent { get; set; }

    public virtual ICollection<DbNew> DbNews { get; set; } = new List<DbNew>();
}
