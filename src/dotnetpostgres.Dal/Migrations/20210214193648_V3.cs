using Microsoft.EntityFrameworkCore.Migrations;

namespace dotnetpostgres.Dal.Migrations
{
    public partial class V3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsConstant",
                table: "Parameters",
                newName: "IsSystem");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsSystem",
                table: "Parameters",
                newName: "IsConstant");
        }
    }
}
