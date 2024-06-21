using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceMVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "db_Advertising_Group",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    location = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    display_order = table.Column<int>(type: "int", nullable: false),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__db_Adver__3213E83FB9810C73", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "db_Category",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    id_category_parent = table.Column<int>(type: "int", nullable: true),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    display_order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__db_Categ__3213E83F81D04C20", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "db_Color",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__db_Color__3213E83F88252288", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "db_Group",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    id_group_parent = table.Column<int>(type: "int", nullable: true),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    display_order = table.Column<int>(type: "int", nullable: false),
                    display_quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__db_Group__3213E83F60BDADE2", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "db_Image",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__db_Image__3213E83F067CAA6F", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "db_Material",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__db_Mater__3213E83FBE99EB8A", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "db_Menu",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    display_order = table.Column<int>(type: "int", nullable: false),
                    id_menu_parent = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__db_Menu__3213E83F55CB1853", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "db_New_Category",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    display_order = table.Column<int>(type: "int", nullable: false),
                    id_new_parent = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__db_New_C__3213E83F94361708", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "db_Size",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__db_Size__3213E83F714311FC", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "db_User",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    gender = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__db_User__3213E83F589D6F1E", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "db_Product",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    id_category = table.Column<int>(type: "int", nullable: false),
                    id_group = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<double>(type: "float", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    view = table.Column<int>(type: "int", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    removedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__db_Produ__3213E83F649DB81A", x => x.id);
                    table.ForeignKey(
                        name: "db_Product_fk2",
                        column: x => x.id_category,
                        principalTable: "db_Category",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "db_Product_fk3",
                        column: x => x.id_group,
                        principalTable: "db_Group",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "db_New",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    describe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    view = table.Column<int>(type: "int", nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    id_new_category = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__db_New__3213E83F63CC78E1", x => x.id);
                    table.ForeignKey(
                        name: "db_New_fk7",
                        column: x => x.id_new_category,
                        principalTable: "db_New_Category",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "db_Invoice",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    into_money = table.Column<double>(type: "float", nullable: false),
                    id_staff = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    id_user = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    name_staff = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    name_user = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__db_Invoi__3213E83F3439CF29", x => x.id);
                    table.ForeignKey(
                        name: "db_Invoice_fk2",
                        column: x => x.id_staff,
                        principalTable: "db_User",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "db_Invoice_fk3",
                        column: x => x.id_user,
                        principalTable: "db_User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "db_Order",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    total_amount = table.Column<double>(type: "float", nullable: false),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    id_user = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    name_user = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    email_user = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address_user = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phone_user = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    paymentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__db_Order__3213E83FC55D56BD", x => x.id);
                    table.ForeignKey(
                        name: "db_Order_fk3",
                        column: x => x.id_user,
                        principalTable: "db_User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_db_User_UserId",
                        column: x => x.UserId,
                        principalTable: "db_User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_db_User_UserId",
                        column: x => x.UserId,
                        principalTable: "db_User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_db_User_UserId",
                        column: x => x.UserId,
                        principalTable: "db_User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_db_User_UserId",
                        column: x => x.UserId,
                        principalTable: "db_User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "db_Auction",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    createdAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    removedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    id_product = table.Column<int>(type: "int", nullable: false),
                    starting_price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__db_Aucti__3213E83FCCC7C676", x => x.id);
                    table.ForeignKey(
                        name: "db_Auction_fk3",
                        column: x => x.id_product,
                        principalTable: "db_Product",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "db_Comment",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_product = table.Column<int>(type: "int", nullable: false),
                    id_user = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__db_Comme__3213E83FC9C7D131", x => x.id);
                    table.ForeignKey(
                        name: "db_Comment_fk1",
                        column: x => x.id_product,
                        principalTable: "db_Product",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "db_Comment_fk2",
                        column: x => x.id_user,
                        principalTable: "db_User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "db_Product_Color",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_color = table.Column<int>(type: "int", nullable: false),
                    id_product = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__db_Produ__3213E83F1668A9EA", x => x.id);
                    table.ForeignKey(
                        name: "db_Product_Color_fk1",
                        column: x => x.id_color,
                        principalTable: "db_Color",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "db_Product_Color_fk2",
                        column: x => x.id_product,
                        principalTable: "db_Product",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "db_Product_Image",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_image = table.Column<int>(type: "int", nullable: false),
                    id_product = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__db_Produ__3213E83F2F4719FC", x => x.id);
                    table.ForeignKey(
                        name: "db_Product_Image_fk1",
                        column: x => x.id_image,
                        principalTable: "db_Image",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "db_Product_Image_fk2",
                        column: x => x.id_product,
                        principalTable: "db_Product",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "db_Product_Material",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_material = table.Column<int>(type: "int", nullable: false),
                    id_product = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__db_Produ__3213E83FAA796C7A", x => x.id);
                    table.ForeignKey(
                        name: "db_Product_Material_fk1",
                        column: x => x.id_material,
                        principalTable: "db_Material",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "db_Product_Material_fk2",
                        column: x => x.id_product,
                        principalTable: "db_Product",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "db_Product_Size",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_size = table.Column<int>(type: "int", nullable: false),
                    id_product = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__db_Produ__3213E83FCB8DD80D", x => x.id);
                    table.ForeignKey(
                        name: "db_Product_Size_fk1",
                        column: x => x.id_size,
                        principalTable: "db_Size",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "db_Product_Size_fk2",
                        column: x => x.id_product,
                        principalTable: "db_Product",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "db_Rating",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_product = table.Column<int>(type: "int", nullable: false),
                    id_user = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    star = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__db_Ratin__3213E83FA223E37A", x => x.id);
                    table.ForeignKey(
                        name: "db_Rating_fk1",
                        column: x => x.id_product,
                        principalTable: "db_Product",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "db_Rating_fk2",
                        column: x => x.id_user,
                        principalTable: "db_User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "db_Invoice_Detail",
                columns: table => new
                {
                    id_invoice = table.Column<int>(type: "int", nullable: false),
                    id_product = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    unit_price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__db_Invoi__76F2AD7781ABE8D2", x => new { x.id_invoice, x.id_product });
                    table.ForeignKey(
                        name: "db_Invoice_Detail_fk0",
                        column: x => x.id_invoice,
                        principalTable: "db_Invoice",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "db_Invoice_Detail_fk1",
                        column: x => x.id_product,
                        principalTable: "db_Product",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "db_Order_Details",
                columns: table => new
                {
                    id_product = table.Column<int>(type: "int", nullable: false),
                    id_order = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    unit_price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__db_Order__57EC50BC88258AE2", x => new { x.id_product, x.id_order });
                    table.ForeignKey(
                        name: "db_Order_Details_fk0",
                        column: x => x.id_product,
                        principalTable: "db_Product",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "db_Order_Details_fk1",
                        column: x => x.id_order,
                        principalTable: "db_Order",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "db_Auction_Round",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    createdAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    price_given = table.Column<double>(type: "float", nullable: false),
                    id_user = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    id_auction = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__db_Aucti__3213E83F8CAE048F", x => x.id);
                    table.ForeignKey(
                        name: "db_Auction_Round_fk3",
                        column: x => x.id_user,
                        principalTable: "db_User",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "db_Auction_Round_fk4",
                        column: x => x.id_auction,
                        principalTable: "db_Auction",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ__db_Adver__3213E83EF258E9F4",
                table: "db_Advertising_Group",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_db_Auction_id_product",
                table: "db_Auction",
                column: "id_product");

            migrationBuilder.CreateIndex(
                name: "UQ__db_Aucti__3213E83E4C02198B",
                table: "db_Auction",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_db_Auction_Round_id_auction",
                table: "db_Auction_Round",
                column: "id_auction");

            migrationBuilder.CreateIndex(
                name: "IX_db_Auction_Round_id_user",
                table: "db_Auction_Round",
                column: "id_user");

            migrationBuilder.CreateIndex(
                name: "UQ__db_Aucti__3213E83E0F3DEF2D",
                table: "db_Auction_Round",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__db_Categ__3213E83E90881A63",
                table: "db_Category",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__db_Color__3213E83E5FAC0072",
                table: "db_Color",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_db_Comment_id_product",
                table: "db_Comment",
                column: "id_product");

            migrationBuilder.CreateIndex(
                name: "IX_db_Comment_id_user",
                table: "db_Comment",
                column: "id_user");

            migrationBuilder.CreateIndex(
                name: "UQ__db_Comme__3213E83E83DCDE4C",
                table: "db_Comment",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__db_Group__3213E83E7D3A97F3",
                table: "db_Group",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__db_Image__3213E83ECE5D1F75",
                table: "db_Image",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_db_Invoice_id_staff",
                table: "db_Invoice",
                column: "id_staff");

            migrationBuilder.CreateIndex(
                name: "IX_db_Invoice_id_user",
                table: "db_Invoice",
                column: "id_user");

            migrationBuilder.CreateIndex(
                name: "UQ__db_Invoi__3213E83E26389EE4",
                table: "db_Invoice",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_db_Invoice_Detail_id_product",
                table: "db_Invoice_Detail",
                column: "id_product");

            migrationBuilder.CreateIndex(
                name: "UQ__db_Mater__3213E83EA760629B",
                table: "db_Material",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__db_Menu__3213E83E0D98ACEE",
                table: "db_Menu",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_db_New_id_new_category",
                table: "db_New",
                column: "id_new_category");

            migrationBuilder.CreateIndex(
                name: "UQ__db_New__3213E83E3BC217F6",
                table: "db_New",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__db_New_C__3213E83E05841274",
                table: "db_New_Category",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_db_Order_id_user",
                table: "db_Order",
                column: "id_user");

            migrationBuilder.CreateIndex(
                name: "UQ__db_Order__3213E83E10DFBEA2",
                table: "db_Order",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_db_Order_Details_id_order",
                table: "db_Order_Details",
                column: "id_order");

            migrationBuilder.CreateIndex(
                name: "IX_db_Product_id_category",
                table: "db_Product",
                column: "id_category");

            migrationBuilder.CreateIndex(
                name: "IX_db_Product_id_group",
                table: "db_Product",
                column: "id_group");

            migrationBuilder.CreateIndex(
                name: "UQ__db_Produ__3213E83E14B9A373",
                table: "db_Product",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_db_Product_Color_id_color",
                table: "db_Product_Color",
                column: "id_color");

            migrationBuilder.CreateIndex(
                name: "IX_db_Product_Color_id_product",
                table: "db_Product_Color",
                column: "id_product");

            migrationBuilder.CreateIndex(
                name: "UQ__db_Produ__3213E83E94BC206C",
                table: "db_Product_Color",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_db_Product_Image_id_image",
                table: "db_Product_Image",
                column: "id_image");

            migrationBuilder.CreateIndex(
                name: "IX_db_Product_Image_id_product",
                table: "db_Product_Image",
                column: "id_product");

            migrationBuilder.CreateIndex(
                name: "UQ__db_Produ__3213E83E3D75A638",
                table: "db_Product_Image",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_db_Product_Material_id_material",
                table: "db_Product_Material",
                column: "id_material");

            migrationBuilder.CreateIndex(
                name: "IX_db_Product_Material_id_product",
                table: "db_Product_Material",
                column: "id_product");

            migrationBuilder.CreateIndex(
                name: "UQ__db_Produ__3213E83E77FE73E8",
                table: "db_Product_Material",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_db_Product_Size_id_product",
                table: "db_Product_Size",
                column: "id_product");

            migrationBuilder.CreateIndex(
                name: "IX_db_Product_Size_id_size",
                table: "db_Product_Size",
                column: "id_size");

            migrationBuilder.CreateIndex(
                name: "UQ__db_Produ__3213E83E983B254E",
                table: "db_Product_Size",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_db_Rating_id_product",
                table: "db_Rating",
                column: "id_product");

            migrationBuilder.CreateIndex(
                name: "IX_db_Rating_id_user",
                table: "db_Rating",
                column: "id_user");

            migrationBuilder.CreateIndex(
                name: "UQ__db_Ratin__3213E83EB725E587",
                table: "db_Rating",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__db_Size__3213E83ED72EA46E",
                table: "db_Size",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "db_User",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UQ__db_User__3213E83E883C510A",
                table: "db_User",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "db_User",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "db_Advertising_Group");

            migrationBuilder.DropTable(
                name: "db_Auction_Round");

            migrationBuilder.DropTable(
                name: "db_Comment");

            migrationBuilder.DropTable(
                name: "db_Invoice_Detail");

            migrationBuilder.DropTable(
                name: "db_Menu");

            migrationBuilder.DropTable(
                name: "db_New");

            migrationBuilder.DropTable(
                name: "db_Order_Details");

            migrationBuilder.DropTable(
                name: "db_Product_Color");

            migrationBuilder.DropTable(
                name: "db_Product_Image");

            migrationBuilder.DropTable(
                name: "db_Product_Material");

            migrationBuilder.DropTable(
                name: "db_Product_Size");

            migrationBuilder.DropTable(
                name: "db_Rating");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "db_Auction");

            migrationBuilder.DropTable(
                name: "db_Invoice");

            migrationBuilder.DropTable(
                name: "db_New_Category");

            migrationBuilder.DropTable(
                name: "db_Order");

            migrationBuilder.DropTable(
                name: "db_Color");

            migrationBuilder.DropTable(
                name: "db_Image");

            migrationBuilder.DropTable(
                name: "db_Material");

            migrationBuilder.DropTable(
                name: "db_Size");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "db_Product");

            migrationBuilder.DropTable(
                name: "db_User");

            migrationBuilder.DropTable(
                name: "db_Category");

            migrationBuilder.DropTable(
                name: "db_Group");
        }
    }
}
