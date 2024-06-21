using System;
using System.Collections.Generic;

namespace ECommerceMVC.Data;

public partial class DbProductImage
{
    public int Id { get; set; }

    public int IdImage { get; set; }

    public int IdProduct { get; set; }

    public virtual DbImage IdImageNavigation { get; set; } = null!;

    public virtual DbProduct IdProductNavigation { get; set; } = null!;
}
