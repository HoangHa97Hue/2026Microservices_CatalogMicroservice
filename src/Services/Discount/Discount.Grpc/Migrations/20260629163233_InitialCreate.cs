using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Discount.Grpc.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Discount",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProductId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProductName = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discount", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Discount",
                columns: new[] { "Id", "Amount", "Description", "ProductId", "ProductName" },
                values: new object[,]
                {
                    { new Guid("6f1c0f77-8f45-4a84-9fc7-1e7a6bb7e101"), 150m, "IPhone1 Discount", new Guid("3d13c704-e34e-441b-bd29-6037bda6ae67"), "Iphone 1" },
                    { new Guid("8a4d2c91-1b7e-4f56-a3c8-9d0e2f7b6a42"), 78m, "Samsung1 Discount", new Guid("12ff1742-8097-4f95-a2ba-9289605a09f3"), "Samung Galaxy 1" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Discount");
        }
    }
}
