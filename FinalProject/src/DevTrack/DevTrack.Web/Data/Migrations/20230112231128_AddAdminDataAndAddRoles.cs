using DevTrack.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevTrack.Web.Data.Migrations
{
    public partial class AddAdminDataAndAddRoles : Migration
    {
        const string ADMIN_USER_ID = "C95008FF-A7F4-42FE-EC9A-08DAF4E08829";
        const string ADMIN_ROLE_ID = "344FB541-8D65-4F0C-7BA2-08DAF4E08E51";
        const string ADMIN_EMAIL = "devTrackAdmin@gmail.com";
        const string SECURITY_STAMP = "6GNKI6U7PDV6SBE7MJARDK7IJQNDLZBK";
        const string CONCURRENCY_STAMP = "40bd4d21-bac2-4d8d-a92c-5297b2e96719";
        const string ADMIN_NAME = "DevTrack Admin";
        const string TIME_ZONE = "(UTC+06:00) Bangladesh Standard Time";
        const string ROLE_NAME = "Admin";
        const string CONCURRENCY_STAMP_ROLE = "a713baa6-2a92-4ec2-86e1-882b9e071e70";


        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();

            var passwordHash = hasher.HashPassword(null, "*DevTrackAdmin123");

            migrationBuilder.InsertData(
               table: "AspNetUsers",
               columns: new[] 
               { 
                  "Id", "UserName", "NormalizedUserName", "Email", "NormalizedEmail", 
                   "EmailConfirmed", "PasswordHash", "SecurityStamp", "ConcurrencyStamp", "PhoneNumber",
                  "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEnd",
                  "LockoutEnabled", "AccessFailedCount", "Image", "Name",
                  "TimeZone"
               },
               values: new object[] 
                { 
                    ADMIN_USER_ID, ADMIN_EMAIL, ADMIN_EMAIL.ToUpper(), ADMIN_EMAIL,
                    ADMIN_EMAIL.ToUpper(), false, passwordHash, SECURITY_STAMP, CONCURRENCY_STAMP,
                    null, false, false, null, true, 0, null, ADMIN_NAME, TIME_ZONE
                }
            );

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[]
                {
                    "Id", "Name", "NormalizedName", "ConcurrencyStamp"
                },
                values: new object[]
                {
                   ADMIN_ROLE_ID, ROLE_NAME, ROLE_NAME.ToUpper(), CONCURRENCY_STAMP_ROLE
                }
            );

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[]
                {
                    "UserId", "RoleId"
                },
                values: new object[]
                {
                   ADMIN_USER_ID, ADMIN_ROLE_ID
                }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumn: "UserId",
                keyValue: ADMIN_USER_ID
            );

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: ADMIN_ROLE_ID
            );

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: ADMIN_USER_ID
            );
        }
    }
}
