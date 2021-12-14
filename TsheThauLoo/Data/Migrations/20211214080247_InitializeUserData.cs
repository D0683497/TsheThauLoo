using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsheThauLoo.Data.Migrations
{
    public partial class InitializeUserData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "22mrI3gVYA8zWHTw0R0IHPLBy", "2ptmL3x5IWVQ4DRcV19AS0jfh", "Student", "STUDENT" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "eHEKw9koyc1UahKDJmN2kaYu4", "zgOFXPEuCBj0oYx7wcVOxPjuT", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "iVslkwN8bQq58oq6Xh7BUT8mE", "vm2O6QewMjb46zCjkzfS8WgYX", "Employee", "EMPLOYEE" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "YLWMXCM7aq6kUbrzBLNBV7edb", "9ZOtm2qcp9BU1V45CvjpzbwP0", "Staff", "STAFF" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "yu0ZAukYcdagLeUttlGA2dx0i", "BHdn3idxWd6CxVJbUiZdFizIG", "Alumnus", "ALUMNUS" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "Gender", "LockoutEnabled", "LockoutEnd", "Name", "NationalId", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "c2VwBZjw3ghfrbBqyCIAfxB5F", 0, null, "A27Or7TyZN2uY3jk2ExXpa25M", null, "admin@gmail.com", true, null, false, null, "管理員", null, "ADMIN@GMAIL.COM", "ADMIN", "AQAAAAEAACcQAAAAEHmKEp9iz9bQ+r1oN/P0akIeSJZmIixVjff2VGNcfgJzcQR96LQC62jjd7sONYaZ4g==", null, false, "JdEjK3uiYrv8RQGf4JrZfVP2Z", false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "eHEKw9koyc1UahKDJmN2kaYu4", "c2VwBZjw3ghfrbBqyCIAfxB5F" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "22mrI3gVYA8zWHTw0R0IHPLBy");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "iVslkwN8bQq58oq6Xh7BUT8mE");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "YLWMXCM7aq6kUbrzBLNBV7edb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "yu0ZAukYcdagLeUttlGA2dx0i");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "eHEKw9koyc1UahKDJmN2kaYu4", "c2VwBZjw3ghfrbBqyCIAfxB5F" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "eHEKw9koyc1UahKDJmN2kaYu4");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c2VwBZjw3ghfrbBqyCIAfxB5F");
        }
    }
}
