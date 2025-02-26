using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Hotel.Migrations
{
    /// <inheritdoc />
    public partial class Hotel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ROOMNUMBER = table.Column<string>(type: "text", nullable: false),
                    TYPE = table.Column<int>(type: "integer", nullable: false),
                    PRICEPERNIGHT = table.Column<double>(type: "double precision", nullable: false),
                    CAPACITY = table.Column<int>(type: "integer", nullable: false),
                    STATUS = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EMAIL = table.Column<string>(type: "text", nullable: false),
                    NAME = table.Column<string>(type: "text", nullable: false),
                    PASSWORD = table.Column<string>(type: "text", nullable: false),
                    ROLE = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    USERID = table.Column<int>(type: "integer", nullable: false),
                    STARTDATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ENDDATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TOTALPRICE = table.Column<double>(type: "double precision", nullable: false),
                    STATUS = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Reservations_Users_USERID",
                        column: x => x.USERID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReservationRooms",
                columns: table => new
                {
                    ReservationID = table.Column<int>(type: "integer", nullable: false),
                    RoomID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationRooms", x => new { x.ReservationID, x.RoomID });
                    table.ForeignKey(
                        name: "FK_ReservationRooms_Reservations_ReservationID",
                        column: x => x.ReservationID,
                        principalTable: "Reservations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservationRooms_Rooms_RoomID",
                        column: x => x.RoomID,
                        principalTable: "Rooms",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservationRooms_RoomID",
                table: "ReservationRooms",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_USERID",
                table: "Reservations",
                column: "USERID");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_ROOMNUMBER",
                table: "Rooms",
                column: "ROOMNUMBER",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_EMAIL",
                table: "Users",
                column: "EMAIL",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservationRooms");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
