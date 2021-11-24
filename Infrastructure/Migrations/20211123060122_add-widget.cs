using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class addwidget : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Widget_Dashboards_DashBoardId",
                table: "Widget");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Widget",
                table: "Widget");

            migrationBuilder.RenameTable(
                name: "Widget",
                newName: "Widgets");

            migrationBuilder.RenameIndex(
                name: "IX_Widget_DashBoardId",
                table: "Widgets",
                newName: "IX_Widgets_DashBoardId");

            migrationBuilder.AlterColumn<string>(
                name: "LayoutType",
                table: "Dashboards",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Widgets",
                table: "Widgets",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Widgets_Dashboards_DashBoardId",
                table: "Widgets",
                column: "DashBoardId",
                principalTable: "Dashboards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Widgets_Dashboards_DashBoardId",
                table: "Widgets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Widgets",
                table: "Widgets");

            migrationBuilder.RenameTable(
                name: "Widgets",
                newName: "Widget");

            migrationBuilder.RenameIndex(
                name: "IX_Widgets_DashBoardId",
                table: "Widget",
                newName: "IX_Widget_DashBoardId");

            migrationBuilder.AlterColumn<string>(
                name: "LayoutType",
                table: "Dashboards",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Widget",
                table: "Widget",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Widget_Dashboards_DashBoardId",
                table: "Widget",
                column: "DashBoardId",
                principalTable: "Dashboards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
