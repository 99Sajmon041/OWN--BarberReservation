using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarberReservation.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWHourstable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HairdresserWorkingHours_HairdresserId",
                table: "HairdresserWorkingHours");

            migrationBuilder.AddColumn<DateOnly>(
                name: "EffectiveFrom",
                table: "HairdresserWorkingHours",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.CreateIndex(
                name: "IX_HairdresserWorkingHours_HairdresserId_DayOfWeek_EffectiveFrom",
                table: "HairdresserWorkingHours",
                columns: new[] { "HairdresserId", "DayOfWeek", "EffectiveFrom" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HairdresserWorkingHours_HairdresserId_DayOfWeek_EffectiveFrom",
                table: "HairdresserWorkingHours");

            migrationBuilder.DropColumn(
                name: "EffectiveFrom",
                table: "HairdresserWorkingHours");

            migrationBuilder.CreateIndex(
                name: "IX_HairdresserWorkingHours_HairdresserId",
                table: "HairdresserWorkingHours",
                column: "HairdresserId");
        }
    }
}
