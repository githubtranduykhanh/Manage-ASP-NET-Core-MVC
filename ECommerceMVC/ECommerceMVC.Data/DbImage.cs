using System;
using System.Collections.Generic;

namespace ECommerceMVC.Data;

public partial class DbImage
{
    public int Id { get; set; }

    public string Url { get; set; } = null!;

    public virtual ICollection<DbProductImage> DbProductImages { get; set; } = new List<DbProductImage>();
}
