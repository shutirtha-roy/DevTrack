using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevTrack.Web.Data.Migrations
{
    public partial class AddRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WebcamCaptures_ActivityId",
                table: "WebcamCaptures");

            migrationBuilder.DropIndex(
                name: "IX_ScreenCaptures_ActivityId",
                table: "ScreenCaptures");

            migrationBuilder.DropIndex(
                name: "IX_MouseActivities_ActivityId",
                table: "MouseActivities");

            migrationBuilder.DropIndex(
                name: "IX_KeyboardActivities_ActivityId",
                table: "KeyboardActivities");

            migrationBuilder.CreateIndex(
                name: "IX_WebcamCaptures_ActivityId",
                table: "WebcamCaptures",
                column: "ActivityId",
                unique: false);

            migrationBuilder.CreateIndex(
                name: "IX_ScreenCaptures_ActivityId",
                table: "ScreenCaptures",
                column: "ActivityId",
                unique: false);

            migrationBuilder.CreateIndex(
                name: "IX_MouseActivities_ActivityId",
                table: "MouseActivities",
                column: "ActivityId",
                unique: false);

            migrationBuilder.CreateIndex(
                name: "IX_KeyboardActivities_ActivityId",
                table: "KeyboardActivities",
                column: "ActivityId",
                unique: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WebcamCaptures_ActivityId",
                table: "WebcamCaptures");

            migrationBuilder.DropIndex(
                name: "IX_ScreenCaptures_ActivityId",
                table: "ScreenCaptures");

            migrationBuilder.DropIndex(
                name: "IX_MouseActivities_ActivityId",
                table: "MouseActivities");

            migrationBuilder.DropIndex(
                name: "IX_KeyboardActivities_ActivityId",
                table: "KeyboardActivities");

            migrationBuilder.CreateIndex(
                name: "IX_WebcamCaptures_ActivityId",
                table: "WebcamCaptures",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ScreenCaptures_ActivityId",
                table: "ScreenCaptures",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_MouseActivities_ActivityId",
                table: "MouseActivities",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_KeyboardActivities_ActivityId",
                table: "KeyboardActivities",
                column: "ActivityId");
        }
    }
}
