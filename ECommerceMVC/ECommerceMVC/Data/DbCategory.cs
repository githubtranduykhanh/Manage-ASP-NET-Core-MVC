using System;
using System.Collections.Generic;

namespace ECommerceMVC.Data;

public partial class DbCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? IdCategoryParent { get; set; }

    public string Image { get; set; } = null!;

    public int DisplayOrder { get; set; }

    public virtual ICollection<DbProduct> DbProducts { get; set; } = new List<DbProduct>();
}
