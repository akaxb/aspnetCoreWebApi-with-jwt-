using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace User.API.Migrations
{
    public partial class add_tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sysAppUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Avatar = table.Column<string>(maxLength: 256, nullable: true),
                    Company = table.Column<string>(maxLength: 128, nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sysAppUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbUserProperties",
                columns: table => new
                {
                    AppUserId = table.Column<int>(nullable: false),
                    Value = table.Column<string>(maxLength: 50, nullable: false),
                    Key = table.Column<string>(maxLength: 50, nullable: false),
                    Text = table.Column<string>(maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbUserProperties", x => new { x.AppUserId, x.Value, x.Key });
                    table.ForeignKey(
                        name: "FK_tbUserProperties_sysAppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "sysAppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbUserProperties");

            migrationBuilder.DropTable(
                name: "sysAppUsers");
        }
    }
}
