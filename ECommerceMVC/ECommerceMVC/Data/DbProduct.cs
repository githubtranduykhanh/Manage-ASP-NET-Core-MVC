using System;
using System.Collections.Generic;

namespace ECommerceMVC.Data;

public partial class DbProduct
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int IdCategory { get; set; }

    public int IdGroup { get; set; }

    public int Quantity { get; set; }

    public double Price { get; set; }

    public string? Description { get; set; }

    public int View { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime RemovedAt { get; set; }

    public virtual ICollection<DbAuction> DbAuctions { get; set; } = new List<DbAuction>();

    public virtual ICollection<DbComment> DbComments { get; set; } = new List<DbComment>();

    public virtual ICollection<DbInvoiceDetail> DbInvoiceDetails { get; set; } = new List<DbInvoiceDetail>();

    public virtual ICollection<DbOrderDetail> DbOrderDetails { get; set; } = new List<DbOrderDetail>();

    public virtual ICollection<DbProductColor> DbProductColors { get; set; } = new List<DbProductColor>();

    public virtual ICollection<DbProductImage> DbProductImages { get; set; } = new List<DbProductImage>();

    public virtual ICollection<DbProductMaterial> DbProductMaterials { get; set; } = new List<DbProductMaterial>();

    public virtual ICollection<DbProductSize> DbProductSizes { get; set; } = new List<DbProductSize>();

    public virtual ICollection<DbRating> DbRatings { get; set; } = new List<DbRating>();

    public virtual DbCategory IdCategoryNavigation { get; set; } = null!;

    public virtual DbGroup IdGroupNavigation { get; set; } = null!;
}
