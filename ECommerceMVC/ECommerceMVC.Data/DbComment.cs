using System;
using System.Collections.Generic;

namespace ECommerceMVC.Data;

public partial class DbComment
{
    public int Id { get; set; }

    public int IdProduct { get; set; }

    public string IdUser { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual DbProduct IdProductNavigation { get; set; } = null!;

    public virtual DbUser IdUserNavigation { get; set; } = null!;
}
