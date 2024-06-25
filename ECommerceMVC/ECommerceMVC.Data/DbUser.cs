using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ECommerceMVC.Data;

public partial class DbUser : IdentityUser
{
    public string? DisplayName { get; set; }
    public string? Address { get; set; }

    public string? Avatar { get; set; }

    public string? Gender { get; set; }

    public virtual ICollection<DbAuctionRound> DbAuctionRounds { get; set; } = new List<DbAuctionRound>();

    public virtual ICollection<DbComment> DbComments { get; set; } = new List<DbComment>();

    public virtual ICollection<DbInvoice> DbInvoiceIdStaffNavigations { get; set; } = new List<DbInvoice>();

    public virtual ICollection<DbInvoice> DbInvoiceIdUserNavigations { get; set; } = new List<DbInvoice>();

    public virtual ICollection<DbOrder> DbOrders { get; set; } = new List<DbOrder>();

    public virtual ICollection<DbRating> DbRatings { get; set; } = new List<DbRating>();
}
