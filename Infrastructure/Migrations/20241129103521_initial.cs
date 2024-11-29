using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    account_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    ic_number = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    mobile_number = table.Column<string>(type: "nvarchar(15)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    mobile_number_confirmed = table.Column<bool>(type: "bit", nullable: false),
                    email_confirmed = table.Column<bool>(type: "bit", nullable: false),
                    pin_hash = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    PinSetup = table.Column<bool>(type: "bit", nullable: false),
                    biometric_enabled = table.Column<bool>(type: "bit", nullable: false),
                    TermAccepted = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.account_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_ic_number",
                table: "Accounts",
                column: "ic_number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
