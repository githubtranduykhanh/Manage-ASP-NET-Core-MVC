using System;
using System.Collections.Generic;

namespace ECommerceMVC.Data;

public partial class DbOrderDetail
{
    public int IdProduct { get; set; }

    public int IdOrder { get; set; }

    public int Quantity { get; set; }

    public double UnitPrice { get; set; }

    public virtual DbOrder IdOrderNavigation { get; set; } = null!;

    public virtual DbProduct IdProductNavigation { get; set; } = null!;
}
