using System;
using System.Collections.Generic;

namespace ECommerceMVC.Data;

public partial class DbNew
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Image { get; set; } = null!;

    public string Describe { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public int View { get; set; }

    public string Content { get; set; } = null!;

    public int IdNewCategory { get; set; }

    public virtual DbNewCategory IdNewCategoryNavigation { get; set; } = null!;
}
