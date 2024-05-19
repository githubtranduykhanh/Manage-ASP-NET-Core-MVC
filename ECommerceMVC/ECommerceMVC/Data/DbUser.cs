using System;
using System.Collections.Generic;

namespace ECommerceMVC.Data;

public partial class DbUser
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int IdRole { get; set; }

    public string Sex { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string LoginType { get; set; } = null!;

    public string? RefreshToken { get; set; }

    public string? Avatar { get; set; }

    public string? SecurityQuestion { get; set; }

    public bool Status { get; set; }

    public virtual ICollection<DbAuctionRound> DbAuctionRounds { get; set; } = new List<DbAuctionRound>();

    public virtual ICollection<DbComment> DbComments { get; set; } = new List<DbComment>();

    public virtual ICollection<DbInvoice> DbInvoiceIdStaffNavigations { get; set; } = new List<DbInvoice>();

    public virtual ICollection<DbInvoice> DbInvoiceIdUserNavigations { get; set; } = new List<DbInvoice>();

    public virtual ICollection<DbOrder> DbOrders { get; set; } = new List<DbOrder>();

    public virtual ICollection<DbRating> DbRatings { get; set; } = new List<DbRating>();

    public virtual DbRole IdRoleNavigation { get; set; } = null!;
}
