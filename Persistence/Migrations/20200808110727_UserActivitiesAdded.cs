using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class UserActivitiesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable("UserActivities",
                table => new
                {
                    AppUserId = table.Column<string>(nullable: false),
                    ActivityId = table.Column<Guid>(nullable: false),
                    DateJoined = table.Column<DateTime>(nullable: false),
                    IsHost = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserActivities", x => new {x.AppUserId, x.ActivityId});
                    table.ForeignKey("FK_UserActivities_Activities_ActivityId",
                        x => x.ActivityId,
                        "Activities",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey("FK_UserActivities_AspNetUsers_AppUserId",
                        x => x.AppUserId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex("IX_UserActivities_ActivityId",
                "UserActivities",
                "ActivityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("UserActivities");
        }
    }
}