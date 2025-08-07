using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace F.Fireworks.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_TenantName_To_AuditLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TenantName",
                table: "AuditLogs",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantName",
                table: "AuditLogs");
        }
    }
}
