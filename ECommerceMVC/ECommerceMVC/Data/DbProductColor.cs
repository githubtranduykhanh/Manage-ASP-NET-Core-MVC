using System;
using System.Collections.Generic;

namespace ECommerceMVC.Data;

public partial class DbProductColor
{
    public int Id { get; set; }

    public int IdColor { get; set; }

    public int IdProduct { get; set; }

    public virtual DbColor IdColorNavigation { get; set; } = null!;

    public virtual DbProduct IdProductNavigation { get; set; } = null!;
}
