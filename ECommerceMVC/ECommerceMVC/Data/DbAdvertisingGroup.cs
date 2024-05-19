using System;
using System.Collections.Generic;

namespace ECommerceMVC.Data;

public partial class DbAdvertisingGroup
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Location { get; set; } = null!;

    public int DisplayOrder { get; set; }

    public string Image { get; set; } = null!;
}
