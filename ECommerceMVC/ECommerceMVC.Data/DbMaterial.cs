using System;
using System.Collections.Generic;

namespace ECommerceMVC.Data;

public partial class DbMaterial
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<DbProductMaterial> DbProductMaterials { get; set; } = new List<DbProductMaterial>();
}
