using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrevFinance.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOAuth2IdentitySupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalProvider",
                table: "users",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExternalSubject",
                table: "users",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_ExternalProvider_ExternalSubject",
                table: "users",
                columns: new[] { "ExternalProvider", "ExternalSubject" },
                unique: true,
                filter: "\"ExternalProvider\" IS NOT NULL AND \"ExternalSubject\" IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_users_ExternalProvider_ExternalSubject",
                table: "users");

            migrationBuilder.DropColumn(
                name: "ExternalProvider",
                table: "users");

            migrationBuilder.DropColumn(
                name: "ExternalSubject",
                table: "users");
        }
    }
}
