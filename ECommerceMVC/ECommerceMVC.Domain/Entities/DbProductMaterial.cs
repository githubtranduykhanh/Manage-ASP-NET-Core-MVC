using System;
using System.Collections.Generic;

namespace ECommerceMVC.Domain.Entities;

public partial class DbProductMaterial
{
    public int Id { get; set; }

    public int IdMaterial { get; set; }

    public int IdProduct { get; set; }

    public virtual DbMaterial IdMaterialNavigation { get; set; } = null!;

    public virtual DbProduct IdProductNavigation { get; set; } = null!;
}
