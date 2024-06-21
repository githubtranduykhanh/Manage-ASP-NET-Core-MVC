using System;
using System.Collections.Generic;

namespace ECommerceMVC.Data;

public partial class DbInvoice
{
    public int Id { get; set; }

    public double IntoMoney { get; set; }

    public string IdStaff { get; set; } = null!;

    public string IdUser { get; set; } = null!;

    public string NameStaff { get; set; } = null!;

    public string NameUser { get; set; } = null!;

    public virtual ICollection<DbInvoiceDetail> DbInvoiceDetails { get; set; } = new List<DbInvoiceDetail>();

    public virtual DbUser IdStaffNavigation { get; set; } = null!;

    public virtual DbUser IdUserNavigation { get; set; } = null!;
}
