using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyBasketWebApi.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameColumnExpireOnInRefreshTokenTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpirOn",
                table: "RefreshTokens",
                newName: "ExpiresOn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpiresOn",
                table: "RefreshTokens",
                newName: "ExpirOn");
        }
    }
}
