using System;
using System.Collections.Generic;

namespace ECommerceMVC.Data;

public partial class DbInvoiceDetail
{
    public int IdInvoice { get; set; }

    public int IdProduct { get; set; }

    public int Quantity { get; set; }

    public double UnitPrice { get; set; }

    public virtual DbInvoice IdInvoiceNavigation { get; set; } = null!;

    public virtual DbProduct IdProductNavigation { get; set; } = null!;
}
