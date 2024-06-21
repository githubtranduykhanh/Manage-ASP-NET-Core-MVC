using System;
using System.Collections.Generic;

namespace ECommerceMVC.Data;

public partial class DbProductSize
{
    public int Id { get; set; }

    public int IdSize { get; set; }

    public int IdProduct { get; set; }

    public virtual DbProduct IdProductNavigation { get; set; } = null!;

    public virtual DbSize IdSizeNavigation { get; set; } = null!;
}
