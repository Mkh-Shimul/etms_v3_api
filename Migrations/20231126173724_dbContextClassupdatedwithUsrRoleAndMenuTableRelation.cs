using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ETMS_API.Migrations
{
    /// <inheritdoc />
    public partial class dbContextClassupdatedwithUsrRoleAndMenuTableRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuUserRole_Menus_MenusId",
                table: "MenuUserRole");

            migrationBuilder.DropForeignKey(
                name: "FK_MenuUserRole_UserRoles_UserRolesId",
                table: "MenuUserRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MenuUserRole",
                table: "MenuUserRole");

            migrationBuilder.RenameTable(
                name: "MenuUserRole",
                newName: "MenuPermission");

            migrationBuilder.RenameIndex(
                name: "IX_MenuUserRole_UserRolesId",
                table: "MenuPermission",
                newName: "IX_MenuPermission_UserRolesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MenuPermission",
                table: "MenuPermission",
                columns: new[] { "MenusId", "UserRolesId" });

            migrationBuilder.AddForeignKey(
                name: "FK_MenuPermission_Menus_MenusId",
                table: "MenuPermission",
                column: "MenusId",
                principalTable: "Menus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MenuPermission_UserRoles_UserRolesId",
                table: "MenuPermission",
                column: "UserRolesId",
                principalTable: "UserRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuPermission_Menus_MenusId",
                table: "MenuPermission");

            migrationBuilder.DropForeignKey(
                name: "FK_MenuPermission_UserRoles_UserRolesId",
                table: "MenuPermission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MenuPermission",
                table: "MenuPermission");

            migrationBuilder.RenameTable(
                name: "MenuPermission",
                newName: "MenuUserRole");

            migrationBuilder.RenameIndex(
                name: "IX_MenuPermission_UserRolesId",
                table: "MenuUserRole",
                newName: "IX_MenuUserRole_UserRolesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MenuUserRole",
                table: "MenuUserRole",
                columns: new[] { "MenusId", "UserRolesId" });

            migrationBuilder.AddForeignKey(
                name: "FK_MenuUserRole_Menus_MenusId",
                table: "MenuUserRole",
                column: "MenusId",
                principalTable: "Menus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MenuUserRole_UserRoles_UserRolesId",
                table: "MenuUserRole",
                column: "UserRolesId",
                principalTable: "UserRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
