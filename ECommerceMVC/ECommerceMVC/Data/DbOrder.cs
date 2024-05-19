using System;
using System.Collections.Generic;

namespace ECommerceMVC.Data;

public partial class DbOrder
{
    public int Id { get; set; }

    public double TotalAmount { get; set; }

    public string Status { get; set; } = null!;

    public int IdUser { get; set; }

    public string NameUser { get; set; } = null!;

    public string EmailUser { get; set; } = null!;

    public string AddressUser { get; set; } = null!;

    public string PhoneUser { get; set; } = null!;

    public string PaymentType { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<DbOrderDetail> DbOrderDetails { get; set; } = new List<DbOrderDetail>();

    public virtual DbUser IdUserNavigation { get; set; } = null!;
}
