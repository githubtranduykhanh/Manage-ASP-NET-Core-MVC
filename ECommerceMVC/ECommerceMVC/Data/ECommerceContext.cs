using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ECommerceMVC.Data;

public partial class ECommerceContext : DbContext
{
    public ECommerceContext()
    {
    }

    public ECommerceContext(DbContextOptions<ECommerceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DbAdvertisingGroup> DbAdvertisingGroups { get; set; }

    public virtual DbSet<DbAuction> DbAuctions { get; set; }

    public virtual DbSet<DbAuctionRound> DbAuctionRounds { get; set; }

    public virtual DbSet<DbCategory> DbCategories { get; set; }

    public virtual DbSet<DbColor> DbColors { get; set; }

    public virtual DbSet<DbComment> DbComments { get; set; }

    public virtual DbSet<DbGroup> DbGroups { get; set; }

    public virtual DbSet<DbImage> DbImages { get; set; }

    public virtual DbSet<DbInvoice> DbInvoices { get; set; }

    public virtual DbSet<DbInvoiceDetail> DbInvoiceDetails { get; set; }

    public virtual DbSet<DbMaterial> DbMaterials { get; set; }

    public virtual DbSet<DbMenu> DbMenus { get; set; }

    public virtual DbSet<DbNew> DbNews { get; set; }

    public virtual DbSet<DbNewCategory> DbNewCategories { get; set; }

    public virtual DbSet<DbOrder> DbOrders { get; set; }

    public virtual DbSet<DbOrderDetail> DbOrderDetails { get; set; }

    public virtual DbSet<DbProduct> DbProducts { get; set; }

    public virtual DbSet<DbProductColor> DbProductColors { get; set; }

    public virtual DbSet<DbProductImage> DbProductImages { get; set; }

    public virtual DbSet<DbProductMaterial> DbProductMaterials { get; set; }

    public virtual DbSet<DbProductSize> DbProductSizes { get; set; }

    public virtual DbSet<DbRating> DbRatings { get; set; }

    public virtual DbSet<DbRole> DbRoles { get; set; }

    public virtual DbSet<DbSize> DbSizes { get; set; }

    public virtual DbSet<DbUser> DbUsers { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning /*To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.*/
//        => optionsBuilder.UseSqlServer("Data Source=LAPTOP-POOHLKIL\\SQLEXPRESS;Initial Catalog=E_Commerce;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DbAdvertisingGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__db_Adver__3213E83FB9810C73");

            entity.ToTable("db_Advertising_Group");

            entity.HasIndex(e => e.Id, "UQ__db_Adver__3213E83EF258E9F4").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.Location)
                .HasMaxLength(50)
                .HasColumnName("location");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<DbAuction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__db_Aucti__3213E83FCCC7C676");

            entity.ToTable("db_Auction");

            entity.HasIndex(e => e.Id, "UQ__db_Aucti__3213E83E4C02198B").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.IdProduct).HasColumnName("id_product");
            entity.Property(e => e.RemovedAt)
                .HasColumnType("datetime")
                .HasColumnName("removedAt");
            entity.Property(e => e.StartingPrice).HasColumnName("starting_price");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.DbAuctions)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("db_Auction_fk3");
        });

        modelBuilder.Entity<DbAuctionRound>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__db_Aucti__3213E83F8CAE048F");

            entity.ToTable("db_Auction_Round");

            entity.HasIndex(e => e.Id, "UQ__db_Aucti__3213E83E0F3DEF2D").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.IdAuction).HasColumnName("id_auction");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.PriceGiven).HasColumnName("price_given");

            entity.HasOne(d => d.IdAuctionNavigation).WithMany(p => p.DbAuctionRounds)
                .HasForeignKey(d => d.IdAuction)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("db_Auction_Round_fk4");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.DbAuctionRounds)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("db_Auction_Round_fk3");
        });

        modelBuilder.Entity<DbCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__db_Categ__3213E83F81D04C20");

            entity.ToTable("db_Category");

            entity.HasIndex(e => e.Id, "UQ__db_Categ__3213E83E90881A63").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IdCategoryParent).HasColumnName("id_category_parent");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<DbColor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__db_Color__3213E83F88252288");

            entity.ToTable("db_Color");

            entity.HasIndex(e => e.Id, "UQ__db_Color__3213E83E5FAC0072").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<DbComment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__db_Comme__3213E83FC9C7D131");

            entity.ToTable("db_Comment");

            entity.HasIndex(e => e.Id, "UQ__db_Comme__3213E83E83DCDE4C").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IdProduct).HasColumnName("id_product");
            entity.Property(e => e.IdUser).HasColumnName("id_user");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.DbComments)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("db_Comment_fk1");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.DbComments)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("db_Comment_fk2");
        });

        modelBuilder.Entity<DbGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__db_Group__3213E83F60BDADE2");

            entity.ToTable("db_Group");

            entity.HasIndex(e => e.Id, "UQ__db_Group__3213E83E7D3A97F3").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.DisplayQuantity).HasColumnName("display_quantity");
            entity.Property(e => e.IdGroupParent).HasColumnName("id_group_parent");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<DbImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__db_Image__3213E83F067CAA6F");

            entity.ToTable("db_Image");

            entity.HasIndex(e => e.Id, "UQ__db_Image__3213E83ECE5D1F75").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Url).HasColumnName("url");
        });

        modelBuilder.Entity<DbInvoice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__db_Invoi__3213E83F3439CF29");

            entity.ToTable("db_Invoice");

            entity.HasIndex(e => e.Id, "UQ__db_Invoi__3213E83E26389EE4").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdStaff).HasColumnName("id_staff");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.IntoMoney).HasColumnName("into_money");
            entity.Property(e => e.NameStaff)
                .HasMaxLength(50)
                .HasColumnName("name_staff");
            entity.Property(e => e.NameUser)
                .HasMaxLength(50)
                .HasColumnName("name_user");

            entity.HasOne(d => d.IdStaffNavigation).WithMany(p => p.DbInvoiceIdStaffNavigations)
                .HasForeignKey(d => d.IdStaff)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("db_Invoice_fk2");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.DbInvoiceIdUserNavigations)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("db_Invoice_fk3");
        });

        modelBuilder.Entity<DbInvoiceDetail>(entity =>
        {
            entity.HasKey(e => new { e.IdInvoice, e.IdProduct }).HasName("PK__db_Invoi__76F2AD7781ABE8D2");

            entity.ToTable("db_Invoice_Detail");

            entity.Property(e => e.IdInvoice).HasColumnName("id_invoice");
            entity.Property(e => e.IdProduct).HasColumnName("id_product");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.UnitPrice).HasColumnName("unit_price");

            entity.HasOne(d => d.IdInvoiceNavigation).WithMany(p => p.DbInvoiceDetails)
                .HasForeignKey(d => d.IdInvoice)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("db_Invoice_Detail_fk0");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.DbInvoiceDetails)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("db_Invoice_Detail_fk1");
        });

        modelBuilder.Entity<DbMaterial>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__db_Mater__3213E83FBE99EB8A");

            entity.ToTable("db_Material");

            entity.HasIndex(e => e.Id, "UQ__db_Mater__3213E83EA760629B").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<DbMenu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__db_Menu__3213E83F55CB1853");

            entity.ToTable("db_Menu");

            entity.HasIndex(e => e.Id, "UQ__db_Menu__3213E83E0D98ACEE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IdMenuParent).HasColumnName("id_menu_parent");
            entity.Property(e => e.Link).HasColumnName("link");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<DbNew>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__db_New__3213E83F63CC78E1");

            entity.ToTable("db_New");

            entity.HasIndex(e => e.Id, "UQ__db_New__3213E83E3BC217F6").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Describe).HasColumnName("describe");
            entity.Property(e => e.IdNewCategory).HasColumnName("id_new_category");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.View).HasColumnName("view");

            entity.HasOne(d => d.IdNewCategoryNavigation).WithMany(p => p.DbNews)
                .HasForeignKey(d => d.IdNewCategory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("db_New_fk7");
        });

        modelBuilder.Entity<DbNewCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__db_New_C__3213E83F94361708");

            entity.ToTable("db_New_Category");

            entity.HasIndex(e => e.Id, "UQ__db_New_C__3213E83E05841274").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IdNewParent).HasColumnName("id_new_parent");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<DbOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__db_Order__3213E83FC55D56BD");

            entity.ToTable("db_Order");

            entity.HasIndex(e => e.Id, "UQ__db_Order__3213E83E10DFBEA2").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddressUser).HasColumnName("address_user");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.EmailUser).HasColumnName("email_user");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.NameUser)
                .HasMaxLength(50)
                .HasColumnName("name_user");
            entity.Property(e => e.PaymentType)
                .HasMaxLength(50)
                .HasColumnName("paymentType");
            entity.Property(e => e.PhoneUser)
                .HasMaxLength(15)
                .HasColumnName("phone_user");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.TotalAmount).HasColumnName("total_amount");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.DbOrders)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("db_Order_fk3");
        });

        modelBuilder.Entity<DbOrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.IdProduct, e.IdOrder }).HasName("PK__db_Order__57EC50BC88258AE2");

            entity.ToTable("db_Order_Details");

            entity.Property(e => e.IdProduct).HasColumnName("id_product");
            entity.Property(e => e.IdOrder).HasColumnName("id_order");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.UnitPrice).HasColumnName("unit_price");

            entity.HasOne(d => d.IdOrderNavigation).WithMany(p => p.DbOrderDetails)
                .HasForeignKey(d => d.IdOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("db_Order_Details_fk1");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.DbOrderDetails)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("db_Order_Details_fk0");
        });

        modelBuilder.Entity<DbProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__db_Produ__3213E83F649DB81A");

            entity.ToTable("db_Product");

            entity.HasIndex(e => e.Id, "UQ__db_Produ__3213E83E14B9A373").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IdCategory).HasColumnName("id_category");
            entity.Property(e => e.IdGroup).HasColumnName("id_group");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.RemovedAt)
                .HasColumnType("datetime")
                .HasColumnName("removedAt");
            entity.Property(e => e.View).HasColumnName("view");

            entity.HasOne(d => d.IdCategoryNavigation).WithMany(p => p.DbProducts)
                .HasForeignKey(d => d.IdCategory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("db_Product_fk2");

            entity.HasOne(d => d.IdGroupNavigation).WithMany(p => p.DbProducts)
                .HasForeignKey(d => d.IdGroup)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("db_Product_fk3");
        });

        modelBuilder.Entity<DbProductColor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__db_Produ__3213E83F1668A9EA");

            entity.ToTable("db_Product_Color");

            entity.HasIndex(e => e.Id, "UQ__db_Produ__3213E83E94BC206C").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdColor).HasColumnName("id_color");
            entity.Property(e => e.IdProduct).HasColumnName("id_product");

            entity.HasOne(d => d.IdColorNavigation).WithMany(p => p.DbProductColors)
                .HasForeignKey(d => d.IdColor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("db_Product_Color_fk1");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.DbProductColors)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("db_Product_Color_fk2");
        });

        modelBuilder.Entity<DbProductImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__db_Produ__3213E83F2F4719FC");

            entity.ToTable("db_Product_Image");

            entity.HasIndex(e => e.Id, "UQ__db_Produ__3213E83E3D75A638").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdImage).HasColumnName("id_image");
            entity.Property(e => e.IdProduct).HasColumnName("id_product");

            entity.HasOne(d => d.IdImageNavigation).WithMany(p => p.DbProductImages)
                .HasForeignKey(d => d.IdImage)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("db_Product_Image_fk1");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.DbProductImages)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("db_Product_Image_fk2");
        });

        modelBuilder.Entity<DbProductMaterial>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__db_Produ__3213E83FAA796C7A");

            entity.ToTable("db_Product_Material");

            entity.HasIndex(e => e.Id, "UQ__db_Produ__3213E83E77FE73E8").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdMaterial).HasColumnName("id_material");
            entity.Property(e => e.IdProduct).HasColumnName("id_product");

            entity.HasOne(d => d.IdMaterialNavigation).WithMany(p => p.DbProductMaterials)
                .HasForeignKey(d => d.IdMaterial)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("db_Product_Material_fk1");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.DbProductMaterials)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("db_Product_Material_fk2");
        });

        modelBuilder.Entity<DbProductSize>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__db_Produ__3213E83FCB8DD80D");

            entity.ToTable("db_Product_Size");

            entity.HasIndex(e => e.Id, "UQ__db_Produ__3213E83E983B254E").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdProduct).HasColumnName("id_product");
            entity.Property(e => e.IdSize).HasColumnName("id_size");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.DbProductSizes)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("db_Product_Size_fk2");

            entity.HasOne(d => d.IdSizeNavigation).WithMany(p => p.DbProductSizes)
                .HasForeignKey(d => d.IdSize)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("db_Product_Size_fk1");
        });

        modelBuilder.Entity<DbRating>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__db_Ratin__3213E83FA223E37A");

            entity.ToTable("db_Rating");

            entity.HasIndex(e => e.Id, "UQ__db_Ratin__3213E83EB725E587").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdProduct).HasColumnName("id_product");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Star).HasColumnName("star");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.DbRatings)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("db_Rating_fk1");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.DbRatings)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("db_Rating_fk2");
        });

        modelBuilder.Entity<DbRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__db_Role__3213E83F650D386B");

            entity.ToTable("db_Role");

            entity.HasIndex(e => e.Id, "UQ__db_Role__3213E83E80A9DED2").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<DbSize>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__db_Size__3213E83F714311FC");

            entity.ToTable("db_Size");

            entity.HasIndex(e => e.Id, "UQ__db_Size__3213E83ED72EA46E").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<DbUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__db_User__3213E83F589D6F1E");

            entity.ToTable("db_User");

            entity.HasIndex(e => e.Id, "UQ__db_User__3213E83E883C510A").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__db_User__AB6E616405FC6C0C").IsUnique();

            entity.HasIndex(e => e.Phone, "UQ__db_User__B43B145F0EBE3480").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasColumnName("address");
            entity.Property(e => e.Avatar).HasColumnName("avatar");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.IdRole).HasColumnName("idRole");
            entity.Property(e => e.LoginType)
                .HasMaxLength(50)
                .HasColumnName("loginType");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasDefaultValue("User Name")
                .HasColumnName("name");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.RefreshToken).HasColumnName("refreshToken");
            entity.Property(e => e.SecurityQuestion).HasColumnName("securityQuestion");
            entity.Property(e => e.Sex)
                .HasMaxLength(10)
                .HasColumnName("sex");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.DbUsers)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("db_User_fk6");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
