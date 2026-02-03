using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TreeWeb.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TW_Directory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    ParentId = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("TW_Directory_PK", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TW_Directory_TW_Directory_ParentId",
                        column: x => x.ParentId,
                        principalTable: "TW_Directory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TW_User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "User")
                },
                constraints: table =>
                {
                    table.PrimaryKey("TW_User_PK", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TW_Directory_ParentId",
                table: "TW_Directory",
                column: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TW_Directory");

            migrationBuilder.DropTable(
                name: "TW_User");
        }
    }
}
