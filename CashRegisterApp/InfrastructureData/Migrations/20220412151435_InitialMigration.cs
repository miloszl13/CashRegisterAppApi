using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace InfrastructureData.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bills",
                columns: table => new
                {
                    Bill_number = table.Column<string>(type: "text", nullable: false),
                    Total_cost = table.Column<int>(type: "integer", nullable: true),
                    Credit_card = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bills", x => x.Bill_number);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Product_Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Cost = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Product_Id);
                });

            migrationBuilder.CreateTable(
                name: "BillProducts",
                columns: table => new
                {
                    Bill_number = table.Column<string>(type: "text", nullable: false),
                    Product_id = table.Column<int>(type: "integer", nullable: false),
                    Product_quantity = table.Column<int>(type: "integer", nullable: false),
                    Products_cost = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillProducts", x => new { x.Bill_number, x.Product_id });
                    table.ForeignKey(
                        name: "FK_BillProducts_Bills_Bill_number",
                        column: x => x.Bill_number,
                        principalTable: "Bills",
                        principalColumn: "Bill_number",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BillProducts_Products_Product_id",
                        column: x => x.Product_id,
                        principalTable: "Products",
                        principalColumn: "Product_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BillProducts_Product_id",
                table: "BillProducts",
                column: "Product_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillProducts");

            migrationBuilder.DropTable(
                name: "Bills");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
