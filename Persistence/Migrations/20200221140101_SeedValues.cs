using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class SeedValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                "Values",
                new[] {"Id", "Name"},
                new object[] {1, "Value1"});

            migrationBuilder.InsertData(
                "Values",
                new[] {"Id", "Name"},
                new object[] {2, "Value2"});

            migrationBuilder.InsertData(
                "Values",
                new[] {"Id", "Name"},
                new object[] {3, "Value3"});
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                "Values",
                "Id",
                1);

            migrationBuilder.DeleteData(
                "Values",
                "Id",
                2);

            migrationBuilder.DeleteData(
                "Values",
                "Id",
                3);
        }
    }
}