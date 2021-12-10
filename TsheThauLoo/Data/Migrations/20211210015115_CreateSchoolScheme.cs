using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsheThauLoo.Data.Migrations
{
    public partial class CreateSchoolScheme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Colleges",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 25, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colleges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 25, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    Degree = table.Column<int>(type: "INTEGER", nullable: false),
                    CollegeId = table.Column<string>(type: "TEXT", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_Colleges_CollegeId",
                        column: x => x.CollegeId,
                        principalTable: "Colleges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Colleges",
                columns: new[] { "Id", "Name" },
                values: new object[] { "0dI3qiJ4paEsZAffJarvrZtL3", "商學院" });

            migrationBuilder.InsertData(
                table: "Colleges",
                columns: new[] { "Id", "Name" },
                values: new object[] { "9xP6LTPTAcSWkRF6cx9Iq7cDv", "國際科技與管理學院" });

            migrationBuilder.InsertData(
                table: "Colleges",
                columns: new[] { "Id", "Name" },
                values: new object[] { "Bem4x4ONNKfMVLge333K97tpP", "人文社會學院" });

            migrationBuilder.InsertData(
                table: "Colleges",
                columns: new[] { "Id", "Name" },
                values: new object[] { "ggRSStSxsHsmiT3yvftunUmCm", "創能學院" });

            migrationBuilder.InsertData(
                table: "Colleges",
                columns: new[] { "Id", "Name" },
                values: new object[] { "if0gyRKzbTjqNb2BCn89dAVfA", "資訊電機學院" });

            migrationBuilder.InsertData(
                table: "Colleges",
                columns: new[] { "Id", "Name" },
                values: new object[] { "iQJHwPwg4VAFwjXDJPVH2wtE0", "金融學院" });

            migrationBuilder.InsertData(
                table: "Colleges",
                columns: new[] { "Id", "Name" },
                values: new object[] { "orgyTzqhLxdCWYZrjnfNMwE88", "工程與科學學院" });

            migrationBuilder.InsertData(
                table: "Colleges",
                columns: new[] { "Id", "Name" },
                values: new object[] { "QGqpVsNzjcusLw2lisuECLEaT", "建設學院" });

            migrationBuilder.InsertData(
                table: "Colleges",
                columns: new[] { "Id", "Name" },
                values: new object[] { "woYRzMjOYVT0LlwVEvqarGaoS", "建築專業學院" });

            migrationBuilder.InsertData(
                table: "Colleges",
                columns: new[] { "Id", "Name" },
                values: new object[] { "ZJQvZKxgdLSVgeAjnIBNkLUoD", "經營管理學院" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "", "QGqpVsNzjcusLw2lisuECLEaT", 0, "都市計畫與空間資訊學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "0IAQg7YRS9A2v1yDs3V6DS3fE", "9xP6LTPTAcSWkRF6cx9Iq7cDv", 0, "美國加州聖荷西州立大學工程雙學士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "0yn8nTHeTq33d7r8HGT972YKf", "0dI3qiJ4paEsZAffJarvrZtL3", 0, "經濟學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "117awZ9RQvBTaStkYBP3jk9qL", "QGqpVsNzjcusLw2lisuECLEaT", 1, "景觀與遊憩碩士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "12Hz7QGyAnqdqT7hMVpIX3ouB", "if0gyRKzbTjqNb2BCn89dAVfA", 2, "資訊工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "1aVlra0qikcAV2JRV7TMRp7iI", "orgyTzqhLxdCWYZrjnfNMwE88", 0, "工業工程與系統管理學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "1KMvherE36HxeETXr3xse9HZk", "0dI3qiJ4paEsZAffJarvrZtL3", 1, "企業管理學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "27kTZ7FpUueqEEXZYr5VFvjaS", "orgyTzqhLxdCWYZrjnfNMwE88", 1, "應用數學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "2Ao6OxweQUKatNTwLqL1SWUIx", "Bem4x4ONNKfMVLge333K97tpP", 1, "公共事務與社會創新碩士在職學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "2L99Mx5zbL0nVEVZ5SQwrdvPp", "if0gyRKzbTjqNb2BCn89dAVfA", 1, "自動控制工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "3qR46zhPrQyWVkMfXVWT5ikFs", "QGqpVsNzjcusLw2lisuECLEaT", 0, "水利工程與資源保育學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "4d9tHiUYHz5Xe7Wzepc6RShZ7", "ggRSStSxsHsmiT3yvftunUmCm", 0, "人工智慧技術與應用學士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "5BMNEb4vjpRMSBwQZP3zCDnpB", "if0gyRKzbTjqNb2BCn89dAVfA", 1, "視光科技碩士在職學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "5IhwNG2y1wlsaIfty7gyggsuL", "0dI3qiJ4paEsZAffJarvrZtL3", 1, "商學專業碩士在職學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "5kPtkwk26CsmYF92Yjm7eFS25", "if0gyRKzbTjqNb2BCn89dAVfA", 1, "資訊工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "5mecRMeCuQzDF9PrBjSUsCDtc", "if0gyRKzbTjqNb2BCn89dAVfA", 1, "資訊電機工程碩士在職專班" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "5zNvuPL5SJVEYi3a10zPEQO8p", "if0gyRKzbTjqNb2BCn89dAVfA", 1, "電機工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "6AEGePPNeOcgGUdevl7XD9v69", "if0gyRKzbTjqNb2BCn89dAVfA", 2, "智慧聯網產業博士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "6hqWk2IvTqFH56Ul0UCmptGmu", "0dI3qiJ4paEsZAffJarvrZtL3", 0, "行銷學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "7CoXdHz2bWFuTk7xE4jD3O626", "Bem4x4ONNKfMVLge333K97tpP", 0, "外國語文學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "7FSHfS7OyuHS5Dz0BoHkNvXSN", "orgyTzqhLxdCWYZrjnfNMwE88", 2, "化學工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "8CDofLpCA2z33Y4pXEKpPmCrf", "QGqpVsNzjcusLw2lisuECLEaT", 1, "都市計畫與空間資訊學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "8Qxn6SnBisKk0zL5ZaCNHzHNU", "orgyTzqhLxdCWYZrjnfNMwE88", 1, "環境工程與科學學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "90GsDYOXGVH4l3KVGxYol1W2o", "if0gyRKzbTjqNb2BCn89dAVfA", 0, "電子工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "9cQA7y6zMOXj4VrpoNbukepq3", "QGqpVsNzjcusLw2lisuECLEaT", 0, "運輸與物流學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "a6VsUoLnqqvOYVFOOYZXP9vW6", "9xP6LTPTAcSWkRF6cx9Iq7cDv", 0, "國際雙學士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "a8qq4y4du8qQFQ1E47zkSxRFj", "ZJQvZKxgdLSVgeAjnIBNkLUoD", 1, "經營管理碩士在職學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "AawwxbqguYo02vgUmruY8q1H5", "9xP6LTPTAcSWkRF6cx9Iq7cDv", 0, "美國加州聖荷西州立大學商學大數據分析雙學士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "aBP9gqZAzXx56nmIgKvFnDz3O", "woYRzMjOYVT0LlwVEvqarGaoS", 0, "創新設計學士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "AhS7F9gUmxfbQ45agBzV6ZqyK", "QGqpVsNzjcusLw2lisuECLEaT", 0, "土地管理學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "alfxjGB18v40cO9P1exRs9bch", "orgyTzqhLxdCWYZrjnfNMwE88", 2, "材料科學與工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "b7Qo0Z5zzhREULXvVgAsQbawy", "iQJHwPwg4VAFwjXDJPVH2wtE0", 2, "金融博士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "C3ps4hxYL8vu0hl6hpAcRYL7i", "QGqpVsNzjcusLw2lisuECLEaT", 1, "建設學院專案管理碩士在職專班" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "cbKctLMMlvmtYSLCDezoMWaDx", "QGqpVsNzjcusLw2lisuECLEaT", 2, "土木水利工程與建設規劃博士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "CsgftORomoxhN0uKAtYU7smKM", "0dI3qiJ4paEsZAffJarvrZtL3", 0, "會計學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "cvje3HNALJWaFcUJfhOv1BT7f", "0dI3qiJ4paEsZAffJarvrZtL3", 0, "企業管理學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "d0tSW85jXda65OgJG7QvJqA87", "0dI3qiJ4paEsZAffJarvrZtL3", 2, "商學博士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "de1Jnf9QD3gvMTPRwz4NxhVDw", "0dI3qiJ4paEsZAffJarvrZtL3", 0, "商學進修學士班" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "DEH5ixlYgCzxPcE1Npro3xXef", "0dI3qiJ4paEsZAffJarvrZtL3", 1, "合作經濟暨社會事業經營學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "Dh4oZEA9zkc3ACZaRM9VtdQBH", "Bem4x4ONNKfMVLge333K97tpP", 1, "文化與社會創新碩士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "DlcGMcK0dUdA5gRwdxmbM7oZG", "0dI3qiJ4paEsZAffJarvrZtL3", 1, "統計學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "Dm3w09uagkM33e8ZrKJjWyk1p", "woYRzMjOYVT0LlwVEvqarGaoS", 1, "創意設計碩士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "DMYlpsYuNEgYb49p4pQpqa89V", "0dI3qiJ4paEsZAffJarvrZtL3", 1, "財經法律研究所" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "dPxcwXdkxCCGd02FuI0mhswrG", "if0gyRKzbTjqNb2BCn89dAVfA", 1, "通訊工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "DqV7WwMlTC5QCzi5rCvIh6hIm", "0dI3qiJ4paEsZAffJarvrZtL3", 0, "國際經營與貿易學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "DZCrDRmTMETLmS41PoVLReTcT", "0dI3qiJ4paEsZAffJarvrZtL3", 0, "財稅學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "E21Gz0eq5OnXxHgLXGJsYIKmn", "if0gyRKzbTjqNb2BCn89dAVfA", 1, "資訊電機工程碩士在職學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "E4TLO6Rlk845z2oUdcY4Iv06m", "orgyTzqhLxdCWYZrjnfNMwE88", 0, "機械與電腦輔助工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "E5JhN19uEM9iT39gupupFtKqb", "orgyTzqhLxdCWYZrjnfNMwE88", 1, "電聲碩士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "Ef1eMd6LtaZP03ZpODKQnkPrr", "orgyTzqhLxdCWYZrjnfNMwE88", 0, "纖維與複合材料學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "ELz9CQQsBKPWDhP8dblVbBF6A", "orgyTzqhLxdCWYZrjnfNMwE88", 1, "光電科學與工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "fDaJWG0j4tZKJD7LZxf84ERNh", "Bem4x4ONNKfMVLge333K97tpP", 1, "中國文學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "fhbrQBPIlbOHqqDpVel1bofqj", "0dI3qiJ4paEsZAffJarvrZtL3", 1, "經濟學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "fHZZYLLRYQfq5zVpWYw2loUei", "orgyTzqhLxdCWYZrjnfNMwE88", 2, "環境工程與科學學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "Gl9ruZax8lgwKo8FrscM7G9wy", "0dI3qiJ4paEsZAffJarvrZtL3", 2, "經濟學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "hclqd3Hyx4ibaMgdNbj4eWqwu", "orgyTzqhLxdCWYZrjnfNMwE88", 2, "纖維與複合材料學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "HjMLEuFapCEXLd1M1TUxLd9sL", "if0gyRKzbTjqNb2BCn89dAVfA", 0, "資訊電機學院學士班" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "hpp8lWqxvHlO4UL5Jv8vIiyGV", "orgyTzqhLxdCWYZrjnfNMwE88", 0, "光電科學與工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "HZToLVKBIxKg1pcG4s8YqT3eo", "if0gyRKzbTjqNb2BCn89dAVfA", 0, "自動控制工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "i5Fj90mDet7rvSsNYDPY5YU24", "iQJHwPwg4VAFwjXDJPVH2wtE0", 0, "財務工程與精算學士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "IgGhCciiJgrkqhSlPwFAURR2A", "0dI3qiJ4paEsZAffJarvrZtL3", 2, "統計學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "IKoM8CjCMFhQIkxBq3VPnMMp9", "woYRzMjOYVT0LlwVEvqarGaoS", 0, "室內設計學士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "Iw4sxzEHJ2ohiitIGz8RAPpoA", "QGqpVsNzjcusLw2lisuECLEaT", 1, "西班牙薩拉戈薩大學物流供應鏈管理與創新創業雙碩士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "J3ppFIMFhj8EuCGUOOmCP0bL6", "QGqpVsNzjcusLw2lisuECLEaT", 1, "智慧城市碩士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "JdbXmSGoUw4piAV9Ytx2AHq6G", "0dI3qiJ4paEsZAffJarvrZtL3", 0, "統計學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "jipCkuWAE0Du3bpRCRx8bsh6a", "orgyTzqhLxdCWYZrjnfNMwE88", 0, "環境工程與科學學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "jKDRVp3ZfjFQsTIho96YUFmAj", "iQJHwPwg4VAFwjXDJPVH2wtE0", 1, "風險管理與保險學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "JkL8NFfUSTFriQGFfhftPkmpO", "0dI3qiJ4paEsZAffJarvrZtL3", 1, "會計學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "JlZcnZDK4kpGlRLUPr6fk9xzb", "0dI3qiJ4paEsZAffJarvrZtL3", 1, "財稅學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "leYi4G3rCzMLNDkQuA2F4DcH1", "0dI3qiJ4paEsZAffJarvrZtL3", 1, "國際經營與貿易學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "Lxx5OqarnVEwetnl8RCX4oAdy", "9xP6LTPTAcSWkRF6cx9Iq7cDv", 1, "國際經營管理碩士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "lziprhEYXoNTIaifDM1jg2aDl", "orgyTzqhLxdCWYZrjnfNMwE88", 0, "化學工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "M7uQ44lkDMXGUXTVIllFHIu0U", "orgyTzqhLxdCWYZrjnfNMwE88", 0, "精密系統設計學士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "mB10Nk6FneHFt04XuF84iJKc7", "9xP6LTPTAcSWkRF6cx9Iq7cDv", 0, "美國加州舊金山州立大學資訊工程雙學士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "mroQH0gDaaGHw3B31MDeWX2cO", "QGqpVsNzjcusLw2lisuECLEaT", 1, "水利工程與資源保育學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "mZItIEcOqGsf2D1CDB75ATNNb", "orgyTzqhLxdCWYZrjnfNMwE88", 1, "綠色能源科技碩士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "MzTRWWzVUx4We6TIkh4N6UxzH", "orgyTzqhLxdCWYZrjnfNMwE88", 1, "航太與系統工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "nONXyEz08bBBJdn66Cvq3oMIt", "if0gyRKzbTjqNb2BCn89dAVfA", 1, "電子工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "nzgPilWPhn1M0OWMU3dirxo0P", "woYRzMjOYVT0LlwVEvqarGaoS", 0, "建築專業學院學士班" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "o7HQY0mdOG2FCCMfPv3A0eiyU", "iQJHwPwg4VAFwjXDJPVH2wtE0", 1, "金融碩士在職專班" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "O9obZd6NxvP91r0V1HizlRmzx", "orgyTzqhLxdCWYZrjnfNMwE88", 1, "纖維與複合材料學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "OD1yUIm1w5q3rV5VKxKZY6aJg", "QGqpVsNzjcusLw2lisuECLEaT", 1, "運輸與物流學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "oJ9mWh0t2QzrKyVbqOKy6Uxla", "if0gyRKzbTjqNb2BCn89dAVfA", 1, "生醫資訊暨生醫工程碩士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "oQnr4w8CM9SqRcM75XIOJhfNc", "woYRzMjOYVT0LlwVEvqarGaoS", 1, "建築碩士在職學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "oW8SWY9EukV5Ywid5WrxTVHau", "if0gyRKzbTjqNb2BCn89dAVfA", 0, "資訊工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "OYM5tHafmDVssmZLQHtK5Sit2", "Bem4x4ONNKfMVLge333K97tpP", 2, "中國文學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "PAkm3KYC60qsMU9o9xuczaz65", "iQJHwPwg4VAFwjXDJPVH2wtE0", 1, "財務金融學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "pel3fUEJ3dUuTmhngqXuMARH7", "iQJHwPwg4VAFwjXDJPVH2wtE0", 1, "金融碩士在職學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "PZBs3lnNWZ1KJKiC91m09gRwG", "if0gyRKzbTjqNb2BCn89dAVfA", 1, "光電能源與視覺科技碩士在職專班" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "q3nlnIpsjsxXYpgNKlMttHXea", "0dI3qiJ4paEsZAffJarvrZtL3", 0, "國際企業管理全英語學士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "qDSwwlratoGaz84ii6LibEFs2", "0dI3qiJ4paEsZAffJarvrZtL3", 1, "科技管理碩士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "qlJ5WI3YfX90fA4NKrt3EKXzI", "QGqpVsNzjcusLw2lisuECLEaT", 1, "土木工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "qqt2F3NiHmRZC3JQ2swBtlkAU", "orgyTzqhLxdCWYZrjnfNMwE88", 0, "材料科學與工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "quIjrAqQ12NKLW2LL0DIDbMdS", "Bem4x4ONNKfMVLge333K97tpP", 1, "外國語文學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "QxgyniralF6EKpnroqXQdcCtS", "QGqpVsNzjcusLw2lisuECLEaT", 0, "土木工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "RLi7RVRW3IUQCOCLQQZIUrfIs", "if0gyRKzbTjqNb2BCn89dAVfA", 0, "通訊工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "rm3BkevoEpzLf1eMVqE5Yo0ce", "QGqpVsNzjcusLw2lisuECLEaT", 1, "土地管理學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "rxjYhZnZzLRGlbkWdLbRpGCgD", "if0gyRKzbTjqNb2BCn89dAVfA", 0, "電機工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "ShJZ82POiUdB5PCThigo9NHCJ", "orgyTzqhLxdCWYZrjnfNMwE88", 1, "數據科學碩士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "SJNlUG18aLT02IvfQgsXTuK2H", "0dI3qiJ4paEsZAffJarvrZtL3", 0, "商學進修學士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "t6iTp3drNHeKklGvntaCj4Y3x", "orgyTzqhLxdCWYZrjnfNMwE88", 0, "航太與系統工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "Td6VsS8gaAJJFiqqG9rYtb8Ka", "orgyTzqhLxdCWYZrjnfNMwE88", 1, "化學工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "TpmRpkDFfhrtlg14kuI55C29i", "woYRzMjOYVT0LlwVEvqarGaoS", 0, "室內設計進修學士班" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "UvKL9QK7ThPRzLH7V8Zrp1Chu", "iQJHwPwg4VAFwjXDJPVH2wtE0", 0, "風險管理與保險學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "V2mx0chIIFPm47C9QShaDcOse", "orgyTzqhLxdCWYZrjnfNMwE88", 0, "應用數學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "vaGAaenkbRlrHWXbU6GanCA2a", "ZJQvZKxgdLSVgeAjnIBNkLUoD", 1, "經營管理碩士在職專班" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "vmfuonsJ6lEsGrfg1zor4wgjT", "orgyTzqhLxdCWYZrjnfNMwE88", 1, "工業工程與系統管理學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "VTUjz812ZDfz7dcl6oNf8yHqJ", "Bem4x4ONNKfMVLge333K97tpP", 1, "公共事務與社會創新研究所" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "W7hjovei6RLK2AL7i1wZAFaDM", "orgyTzqhLxdCWYZrjnfNMwE88", 1, "機械與電腦輔助工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "wdVJDbdDmhz27blrxFLKIELqj", "Bem4x4ONNKfMVLge333K97tpP", 0, "中國文學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "wftcQpHOTzWoLLrA43mbVuSij", "orgyTzqhLxdCWYZrjnfNMwE88", 1, "材料科學與工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "WRxZ6INKBMk3Luk9pdlxWVbD8", "0dI3qiJ4paEsZAffJarvrZtL3", 1, "商學院商學專業碩士在職專班" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "WucZWrsuYrvozv4TWeWJW61IU", "QGqpVsNzjcusLw2lisuECLEaT", 1, "專案管理碩士在職學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "WWW1UII5bCGy6mZYliLWqTb3Y", "0dI3qiJ4paEsZAffJarvrZtL3", 1, "行銷學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "XqGWrO7VR16BZRlJBbOixhfAz", "Bem4x4ONNKfMVLge333K97tpP", 1, "歷史與文物研究所" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "YB0mkKPStKJuB1LIxf7YW1ZPf", "0dI3qiJ4paEsZAffJarvrZtL3", 0, "合作經濟暨社會事業經營學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "yRkkVDq2xMAOV6meUnmb1O4iV", "woYRzMjOYVT0LlwVEvqarGaoS", 0, "建築學士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "YX6zcrayVMdPrflcZBZVF58BR", "9xP6LTPTAcSWkRF6cx9Iq7cDv", 0, "美國普渡大學電機資訊雙學士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "yXJHEN90oBLQmE5rmnVgFP88G", "woYRzMjOYVT0LlwVEvqarGaoS", 1, "建築碩士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "z2GeW6JHLUxUyZp6ej88KdJ2z", "orgyTzqhLxdCWYZrjnfNMwE88", 1, "智能製造與工程管理碩士在職學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "Z3JpsBrwJF8BnPjrLnKyB1oHs", "Bem4x4ONNKfMVLge333K97tpP", 0, "人文社會學士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "z4sSzyyUCZGGcmj2hvL9y8XCf", "iQJHwPwg4VAFwjXDJPVH2wtE0", 0, "財務金融學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "ZGSzhoQ0SYpxasVopp9KRZha7", "9xP6LTPTAcSWkRF6cx9Iq7cDv", 0, "澳洲墨爾本皇家理工大學商學與創新雙學士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "ZIt17vZJhZYKuNu8uip8mpL4k", "orgyTzqhLxdCWYZrjnfNMwE88", 2, "機械與航空工程博士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "ZMbKwvFHC8uItgeEHlBXvEHKU", "orgyTzqhLxdCWYZrjnfNMwE88", 2, "工業工程與系統管理學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "ZOog2nEV9O1zrMv4DVRyQU55I", "if0gyRKzbTjqNb2BCn89dAVfA", 2, "電機與通訊工程博士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "zYc5qrrI7ZTyUdEY8mmOX2aN8", "QGqpVsNzjcusLw2lisuECLEaT", 1, "建設碩士在職學位學程" });

            migrationBuilder.CreateIndex(
                name: "IX_Departments_CollegeId",
                table: "Departments",
                column: "CollegeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Colleges");
        }
    }
}
