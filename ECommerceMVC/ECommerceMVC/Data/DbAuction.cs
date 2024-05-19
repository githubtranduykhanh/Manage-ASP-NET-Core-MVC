using System;
using System.Collections.Generic;

namespace ECommerceMVC.Data;

public partial class DbAuction
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime RemovedAt { get; set; }

    public int IdProduct { get; set; }

    public double StartingPrice { get; set; }

    public virtual ICollection<DbAuctionRound> DbAuctionRounds { get; set; } = new List<DbAuctionRound>();

    public virtual DbProduct IdProductNavigation { get; set; } = null!;
}
