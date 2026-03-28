using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrevFinance.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddInstallmentPlanAndTransactionStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InstallmentCount",
                table: "transactions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InstallmentNumber",
                table: "transactions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InstallmentPlanId",
                table: "transactions",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "transactions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "installment_plans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    InstallmentCount = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Frequency = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_installment_plans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_installment_plans_accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_installment_plans_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_transactions_InstallmentPlanId",
                table: "transactions",
                column: "InstallmentPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_installment_plans_AccountId",
                table: "installment_plans",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_installment_plans_UserId_StartDate",
                table: "installment_plans",
                columns: new[] { "UserId", "StartDate" });

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_installment_plans_InstallmentPlanId",
                table: "transactions",
                column: "InstallmentPlanId",
                principalTable: "installment_plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transactions_installment_plans_InstallmentPlanId",
                table: "transactions");

            migrationBuilder.DropTable(
                name: "installment_plans");

            migrationBuilder.DropIndex(
                name: "IX_transactions_InstallmentPlanId",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "InstallmentCount",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "InstallmentNumber",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "InstallmentPlanId",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "transactions");
        }
    }
}
