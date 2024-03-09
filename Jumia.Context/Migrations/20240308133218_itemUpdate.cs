using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jumia.Context.Migrations
{
    /// <inheritdoc />
    public partial class itemUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_items_products_Id",
                table: "items");

            migrationBuilder.DropColumn(
                name: "String",
                table: "items");

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "items",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "ProductID",
                table: "items",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_items_ProductID",
                table: "items",
                column: "ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_items_products_ProductID",
                table: "items",
                column: "ProductID",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_items_products_ProductID",
                table: "items");

            migrationBuilder.DropIndex(
                name: "IX_items_ProductID",
                table: "items");

            migrationBuilder.DropColumn(
                name: "ProductID",
                table: "items");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "items",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "String",
                table: "items",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_items_products_Id",
                table: "items",
                column: "Id",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
