using System;
using System.Collections.Generic;

namespace ECommerceMVC.Data;

public partial class DbAuctionRound
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public double PriceGiven { get; set; }

    public int IdUser { get; set; }

    public int IdAuction { get; set; }

    public virtual DbAuction IdAuctionNavigation { get; set; } = null!;

    public virtual DbUser IdUserNavigation { get; set; } = null!;
}
