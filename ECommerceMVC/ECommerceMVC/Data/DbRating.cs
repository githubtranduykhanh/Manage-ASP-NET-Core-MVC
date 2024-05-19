using System;
using System.Collections.Generic;

namespace ECommerceMVC.Data;

public partial class DbRating
{
    public int Id { get; set; }

    public int IdProduct { get; set; }

    public int IdUser { get; set; }

    public int? Star { get; set; }

    public virtual DbProduct IdProductNavigation { get; set; } = null!;

    public virtual DbUser IdUserNavigation { get; set; } = null!;
}
