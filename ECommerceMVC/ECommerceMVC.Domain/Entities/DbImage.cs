using System;
using System.Collections.Generic;

namespace ECommerceMVC.Domain.Entities;

public partial class DbImage
{
    public int Id { get; set; }

    public string Url { get; set; } = null!;

    public string? IdPublic { get; set; }

    public virtual ICollection<DbProductImage> DbProductImages { get; set; } = new List<DbProductImage>();
}
