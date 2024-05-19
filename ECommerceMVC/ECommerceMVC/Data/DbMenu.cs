using System;
using System.Collections.Generic;

namespace ECommerceMVC.Data;

public partial class DbMenu
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Link { get; set; } = null!;

    public int DisplayOrder { get; set; }

    public int? IdMenuParent { get; set; }
}
