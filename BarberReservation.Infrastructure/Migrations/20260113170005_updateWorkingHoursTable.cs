using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarberReservation.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateWorkingHoursTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HairdresserServices_HairdresserId",
                table: "HairdresserServices");

            migrationBuilder.AddColumn<bool>(
                name: "IsWorkingDay",
                table: "HairdresserWorkingHours",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_HairdresserServices_HairdresserId_ServiceId",
                table: "HairdresserServices",
                columns: new[] { "HairdresserId", "ServiceId" },
                unique: true,
                filter: "[IsActive] = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HairdresserServices_HairdresserId_ServiceId",
                table: "HairdresserServices");

            migrationBuilder.DropColumn(
                name: "IsWorkingDay",
                table: "HairdresserWorkingHours");

            migrationBuilder.CreateIndex(
                name: "IX_HairdresserServices_HairdresserId",
                table: "HairdresserServices",
                column: "HairdresserId");
        }
    }
}
