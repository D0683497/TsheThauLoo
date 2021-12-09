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
                    Id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
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
                    Id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    Degree = table.Column<int>(type: "INTEGER", nullable: false),
                    CollegeId = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false)
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
                values: new object[] { "13c57893-16db-478f-9b65-264ccb4898e6", "商學院" });

            migrationBuilder.InsertData(
                table: "Colleges",
                columns: new[] { "Id", "Name" },
                values: new object[] { "1ce0231c-283c-4636-979b-ba143fa42b62", "金融學院" });

            migrationBuilder.InsertData(
                table: "Colleges",
                columns: new[] { "Id", "Name" },
                values: new object[] { "227f18d6-d050-4147-aedd-ba775a71a292", "國際科技與管理學院" });

            migrationBuilder.InsertData(
                table: "Colleges",
                columns: new[] { "Id", "Name" },
                values: new object[] { "3041f226-9230-4756-935d-8e0fc1ed112d", "創能學院" });

            migrationBuilder.InsertData(
                table: "Colleges",
                columns: new[] { "Id", "Name" },
                values: new object[] { "3e22a96f-c909-4c79-a903-8c0fef6c789a", "工程與科學學院" });

            migrationBuilder.InsertData(
                table: "Colleges",
                columns: new[] { "Id", "Name" },
                values: new object[] { "8f0bd676-a08b-43e1-a4bb-4f32dbf8174f", "建築專業學院" });

            migrationBuilder.InsertData(
                table: "Colleges",
                columns: new[] { "Id", "Name" },
                values: new object[] { "a89c9f64-6562-44b9-b744-fc1050c575e5", "經營管理學院" });

            migrationBuilder.InsertData(
                table: "Colleges",
                columns: new[] { "Id", "Name" },
                values: new object[] { "d8458f4f-6a6a-4bad-97ee-6d1ce9aaf93a", "人文社會學院" });

            migrationBuilder.InsertData(
                table: "Colleges",
                columns: new[] { "Id", "Name" },
                values: new object[] { "f07974ca-3ef4-4166-a5ea-60b19980c05e", "建設學院" });

            migrationBuilder.InsertData(
                table: "Colleges",
                columns: new[] { "Id", "Name" },
                values: new object[] { "fa044fea-06d3-4f0e-a354-f335d761e2cb", "資訊電機學院" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "03492289-e244-4aa9-90ff-58190616ab12", "8f0bd676-a08b-43e1-a4bb-4f32dbf8174f", 1, "創意設計碩士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "037122b4-04e7-4666-a9f4-4cbaf3b1790a", "13c57893-16db-478f-9b65-264ccb4898e6", 1, "財稅學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "05d8034e-7a32-4244-af88-a8e82885c002", "fa044fea-06d3-4f0e-a354-f335d761e2cb", 1, "生醫資訊暨生醫工程碩士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "0753a654-47eb-417d-ace0-9e8ebd6a814c", "3e22a96f-c909-4c79-a903-8c0fef6c789a", 1, "智能製造與工程管理碩士在職學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "08392375-344e-472d-a4e1-e0dce298f3b5", "13c57893-16db-478f-9b65-264ccb4898e6", 1, "科技管理碩士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "08dd0723-8c09-4b5c-b4e4-86d48b8bb144", "1ce0231c-283c-4636-979b-ba143fa42b62", 2, "金融博士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "0a05fc3d-08a2-42e0-aeda-1efac68e94b5", "3e22a96f-c909-4c79-a903-8c0fef6c789a", 2, "機械與航空工程博士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "0aec387a-093b-4dc0-a9b8-21a684c94d95", "3e22a96f-c909-4c79-a903-8c0fef6c789a", 2, "環境工程與科學學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "0b4046c5-ff28-4b77-bd47-262b7def018d", "227f18d6-d050-4147-aedd-ba775a71a292", 0, "美國加州聖荷西州立大學工程雙學士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "0b7b6c54-59c8-4eb7-a47d-477572bf464c", "8f0bd676-a08b-43e1-a4bb-4f32dbf8174f", 0, "室內設計學士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "0db86b82-81ca-42ed-bc5f-a18ebbab6924", "fa044fea-06d3-4f0e-a354-f335d761e2cb", 2, "資訊工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "0ebdf6ea-88f1-499f-88ae-f6d3646d92af", "f07974ca-3ef4-4166-a5ea-60b19980c05e", 1, "專案管理碩士在職學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "119e1c67-f803-4dc3-959b-a0673220c71c", "3e22a96f-c909-4c79-a903-8c0fef6c789a", 1, "機械與電腦輔助工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "14f08a09-0f6d-4a8c-aa7a-31999d32ab94", "13c57893-16db-478f-9b65-264ccb4898e6", 0, "經濟學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "1d50af56-b8e1-4aef-b19f-b6954e3223c5", "13c57893-16db-478f-9b65-264ccb4898e6", 0, "行銷學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "1faab6e3-45e9-4f2a-b9c6-d515a80ee74e", "13c57893-16db-478f-9b65-264ccb4898e6", 0, "統計學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "21879ce1-3894-4d02-97d3-b222a21b439c", "227f18d6-d050-4147-aedd-ba775a71a292", 1, "國際經營管理碩士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "236e7e9d-dc6c-441a-96b1-7f89a42293e5", "d8458f4f-6a6a-4bad-97ee-6d1ce9aaf93a", 1, "歷史與文物研究所" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "25c0ee9a-c2fe-480b-9526-61f01f68d527", "3041f226-9230-4756-935d-8e0fc1ed112d", 0, "人工智慧技術與應用學士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "266c7fd6-474e-43dc-8897-ea3bc31a8e0d", "13c57893-16db-478f-9b65-264ccb4898e6", 0, "會計學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "2bbc4650-a47f-45a9-a220-b41f976011e5", "fa044fea-06d3-4f0e-a354-f335d761e2cb", 0, "電子工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "2bbeb586-7c3f-4e17-ad2f-4d94b6adc796", "f07974ca-3ef4-4166-a5ea-60b19980c05e", 1, "都市計畫與空間資訊學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "2ccfe6b4-06fc-47d9-9c1b-82a501fa1058", "1ce0231c-283c-4636-979b-ba143fa42b62", 0, "財務金融學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "2d3d53f4-2f7c-481c-b6bb-6813839a69eb", "d8458f4f-6a6a-4bad-97ee-6d1ce9aaf93a", 0, "中國文學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "2daa2f2f-c287-473b-b0cf-05fa1ffb7e13", "13c57893-16db-478f-9b65-264ccb4898e6", 1, "商學院商學專業碩士在職專班" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "30b602f3-6586-4f43-99ae-bdef268d0b64", "227f18d6-d050-4147-aedd-ba775a71a292", 0, "國際雙學士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "335b234b-d7d4-4c55-840e-0e83d57da5cb", "227f18d6-d050-4147-aedd-ba775a71a292", 0, "美國普渡大學電機資訊雙學士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "367992b7-f691-4337-aade-f02336ea9859", "fa044fea-06d3-4f0e-a354-f335d761e2cb", 1, "資訊工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "36d94909-69ac-4f83-a541-54e5c02edd5f", "3e22a96f-c909-4c79-a903-8c0fef6c789a", 2, "材料科學與工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "3d788520-2708-433c-ab8a-9ffbd647faf5", "f07974ca-3ef4-4166-a5ea-60b19980c05e", 1, "水利工程與資源保育學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "3f07e6c0-36ea-4b4d-8793-74dcb1cadb7e", "3e22a96f-c909-4c79-a903-8c0fef6c789a", 1, "應用數學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "41496773-ee83-4cdf-b60d-bacc7689b9c0", "1ce0231c-283c-4636-979b-ba143fa42b62", 1, "財務金融學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "42946f30-e417-4347-b93e-f8fdfb8a34ad", "d8458f4f-6a6a-4bad-97ee-6d1ce9aaf93a", 2, "中國文學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "45436abe-74f2-45f2-a2f5-93c931cdf121", "13c57893-16db-478f-9b65-264ccb4898e6", 0, "商學進修學士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "4777fce8-c37d-43a4-9428-60abafbeb128", "13c57893-16db-478f-9b65-264ccb4898e6", 1, "國際經營與貿易學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "47f0dab8-4507-491a-8871-f057d856c55e", "fa044fea-06d3-4f0e-a354-f335d761e2cb", 1, "自動控制工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "49635313-4992-4e22-b1a6-192127777ee6", "f07974ca-3ef4-4166-a5ea-60b19980c05e", 0, "土地管理學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "49cac5bf-6920-4b47-a3a4-ba794f692515", "13c57893-16db-478f-9b65-264ccb4898e6", 1, "合作經濟暨社會事業經營學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "4ab35fc1-d012-44ae-a3a7-aa5f9333ab91", "3e22a96f-c909-4c79-a903-8c0fef6c789a", 0, "纖維與複合材料學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "4b70a8b9-55cc-4c59-961a-3e2e45205001", "3e22a96f-c909-4c79-a903-8c0fef6c789a", 1, "航太與系統工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "4bd31f0a-0e32-42bc-bd3d-369b2734e4d6", "3e22a96f-c909-4c79-a903-8c0fef6c789a", 1, "材料科學與工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "4d969aab-d6fc-4ca3-be20-5f71fd3e22ba", "3e22a96f-c909-4c79-a903-8c0fef6c789a", 0, "化學工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "4e1cd01d-94bd-42de-807b-2db5ad803059", "13c57893-16db-478f-9b65-264ccb4898e6", 0, "企業管理學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "4f8e93da-b82e-48da-b543-f3a524d6113c", "1ce0231c-283c-4636-979b-ba143fa42b62", 0, "財務工程與精算學士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "51b58c88-e5e6-4112-a2ea-f3ffd5018669", "13c57893-16db-478f-9b65-264ccb4898e6", 1, "財經法律研究所" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "52629b36-cb7c-4c34-af93-0f5b34110bd2", "fa044fea-06d3-4f0e-a354-f335d761e2cb", 1, "視光科技碩士在職學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "5336690e-f31c-4c33-837a-8be30a4a7ceb", "8f0bd676-a08b-43e1-a4bb-4f32dbf8174f", 0, "建築學士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "54a533be-ab21-4c64-8d4c-98b8f65ef77b", "3e22a96f-c909-4c79-a903-8c0fef6c789a", 0, "機械與電腦輔助工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "56582cf6-842d-47bb-a584-cb116e913690", "3e22a96f-c909-4c79-a903-8c0fef6c789a", 1, "光電科學與工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "567323ad-82f1-4531-b430-c6d0efb4b3dd", "13c57893-16db-478f-9b65-264ccb4898e6", 0, "合作經濟暨社會事業經營學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "5684f51e-7506-43d4-9253-c8b60912653d", "3e22a96f-c909-4c79-a903-8c0fef6c789a", 0, "光電科學與工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "576d232b-0660-4156-918f-a55e24c9ca99", "f07974ca-3ef4-4166-a5ea-60b19980c05e", 1, "西班牙薩拉戈薩大學物流供應鏈管理與創新創業雙碩士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "577e5175-c71b-4b17-890a-0f2733a4ca7c", "3e22a96f-c909-4c79-a903-8c0fef6c789a", 0, "材料科學與工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "57de29ef-f94c-4ade-bf49-3148c6668b72", "13c57893-16db-478f-9b65-264ccb4898e6", 1, "行銷學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "5894f2e6-09c3-4808-83fe-f28b170bc9ca", "3e22a96f-c909-4c79-a903-8c0fef6c789a", 2, "工業工程與系統管理學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "593441cb-d226-4a17-9f44-f43ed4ad3304", "3e22a96f-c909-4c79-a903-8c0fef6c789a", 1, "環境工程與科學學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "599a3556-ba31-460e-9922-ba345f9b83e6", "8f0bd676-a08b-43e1-a4bb-4f32dbf8174f", 0, "建築專業學院學士班" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "5aa9154a-de0a-4e15-8d73-855f570a8517", "a89c9f64-6562-44b9-b744-fc1050c575e5", 1, "經營管理碩士在職學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "5f3f21fb-3adf-4ca4-aef0-4cfd968c1f57", "fa044fea-06d3-4f0e-a354-f335d761e2cb", 1, "電子工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "5f683b1e-ffa1-48d3-a8bc-43740a755505", "d8458f4f-6a6a-4bad-97ee-6d1ce9aaf93a", 1, "外國語文學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "6046654d-8b50-4915-ba2b-e649ea32913f", "13c57893-16db-478f-9b65-264ccb4898e6", 1, "統計學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "621e0eb8-1bd2-4669-b6e7-43eed1596a1c", "fa044fea-06d3-4f0e-a354-f335d761e2cb", 0, "資訊電機學院學士班" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "629fd6d4-f81e-43f3-951c-d63e5ab9a78d", "f07974ca-3ef4-4166-a5ea-60b19980c05e", 0, "運輸與物流學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "658b7def-5592-45c5-9059-5628034e3d59", "fa044fea-06d3-4f0e-a354-f335d761e2cb", 0, "通訊工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "675390ab-fcad-4817-9868-a77bdf6b2e78", "13c57893-16db-478f-9b65-264ccb4898e6", 1, "商學專業碩士在職學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "6ae91071-ab05-4eca-94d5-fd2f89362e53", "d8458f4f-6a6a-4bad-97ee-6d1ce9aaf93a", 0, "人文社會學士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "6b9b4a9c-22bd-4deb-86b7-c7bc1705703d", "8f0bd676-a08b-43e1-a4bb-4f32dbf8174f", 1, "建築碩士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "6ff1a064-845c-4ca5-840a-ca29694592c5", "f07974ca-3ef4-4166-a5ea-60b19980c05e", 1, "景觀與遊憩碩士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "70488f20-cd2d-4b88-90e3-93cbab0c46a2", "13c57893-16db-478f-9b65-264ccb4898e6", 1, "經濟學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "76b83e56-7c5b-4595-97b1-49ea6106d2cf", "1ce0231c-283c-4636-979b-ba143fa42b62", 1, "金融碩士在職專班" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "7701b7ae-1dfa-459b-9490-129b6ea80f7f", "3e22a96f-c909-4c79-a903-8c0fef6c789a", 2, "化學工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "825646bc-8423-46ce-af79-169660dd2571", "f07974ca-3ef4-4166-a5ea-60b19980c05e", 0, "土木工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "829d2e71-7b53-4080-ba86-b82defea9099", "3e22a96f-c909-4c79-a903-8c0fef6c789a", 0, "環境工程與科學學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "86bc1a5e-0543-48a4-9d88-786e5e49b5b5", "3e22a96f-c909-4c79-a903-8c0fef6c789a", 0, "航太與系統工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "86e3f7a6-b12c-4c08-8733-aa125de779b9", "f07974ca-3ef4-4166-a5ea-60b19980c05e", 1, "建設學院專案管理碩士在職專班" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "86f378ce-6114-4808-952c-07668dbaf9fd", "13c57893-16db-478f-9b65-264ccb4898e6", 0, "國際企業管理全英語學士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "8a40ac5d-f752-41ec-8546-2d320515e62b", "3e22a96f-c909-4c79-a903-8c0fef6c789a", 2, "纖維與複合材料學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "8cbe768c-3397-4251-b784-49dd8e6e99cf", "227f18d6-d050-4147-aedd-ba775a71a292", 0, "美國加州聖荷西州立大學商學大數據分析雙學士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "9aa5dd68-26e8-4abe-959a-340539a4603a", "d8458f4f-6a6a-4bad-97ee-6d1ce9aaf93a", 1, "公共事務與社會創新碩士在職學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "9af2920a-888b-4d66-8ba8-79816b4cf9a8", "d8458f4f-6a6a-4bad-97ee-6d1ce9aaf93a", 1, "中國文學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "9dfcf85b-65da-4691-a864-ea1e478ec960", "8f0bd676-a08b-43e1-a4bb-4f32dbf8174f", 0, "創新設計學士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "9f7b6093-a30c-43f6-9b80-e65b919c26ae", "3e22a96f-c909-4c79-a903-8c0fef6c789a", 1, "綠色能源科技碩士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "a8fe8fe4-fecf-427d-8599-5150354cbb5a", "d8458f4f-6a6a-4bad-97ee-6d1ce9aaf93a", 1, "公共事務與社會創新研究所" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "b1bae1c5-6b9f-41de-b42f-cd82a7dad8f2", "f07974ca-3ef4-4166-a5ea-60b19980c05e", 1, "智慧城市碩士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "b33110d1-3c0d-491f-9cc3-a05bdff11f4d", "fa044fea-06d3-4f0e-a354-f335d761e2cb", 2, "智慧聯網產業博士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "b40215ab-2609-4464-b24e-bdfbc5d3ed2b", "fa044fea-06d3-4f0e-a354-f335d761e2cb", 0, "資訊工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "b404fded-a6bd-4648-93ff-32568b51c8b3", "fa044fea-06d3-4f0e-a354-f335d761e2cb", 1, "資訊電機工程碩士在職學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "b64bc109-a3c4-4aee-b581-5d6cdcb3ab0d", "13c57893-16db-478f-9b65-264ccb4898e6", 0, "商學進修學士班" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "bb7be155-823c-4337-90d3-1db9a587cb3e", "f07974ca-3ef4-4166-a5ea-60b19980c05e", 1, "運輸與物流學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "bbb9d4b7-bc53-4385-8ce5-a2913be97bcc", "13c57893-16db-478f-9b65-264ccb4898e6", 1, "會計學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "bc526bfb-6db9-44c0-bb88-44641663c933", "fa044fea-06d3-4f0e-a354-f335d761e2cb", 1, "光電能源與視覺科技碩士在職專班" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "c0e7ee82-ca03-4327-93c3-cfa7af9f69b7", "13c57893-16db-478f-9b65-264ccb4898e6", 2, "商學博士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "c134861a-3f57-4f83-a0b0-ea79024a96c8", "f07974ca-3ef4-4166-a5ea-60b19980c05e", 2, "土木水利工程與建設規劃博士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "c742d386-401e-44aa-8b3e-892f407ee6fc", "227f18d6-d050-4147-aedd-ba775a71a292", 0, "澳洲墨爾本皇家理工大學商學與創新雙學士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "c9e56c0e-2cff-459d-90ad-9efea62403e2", "3e22a96f-c909-4c79-a903-8c0fef6c789a", 1, "化學工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "cc3a263f-efff-4abc-8088-281a61a9c571", "13c57893-16db-478f-9b65-264ccb4898e6", 1, "企業管理學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "d77a826e-736f-49b6-8987-f4df06a71cf7", "fa044fea-06d3-4f0e-a354-f335d761e2cb", 1, "通訊工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "da19b793-5507-4363-9c82-c8779cad9a98", "3e22a96f-c909-4c79-a903-8c0fef6c789a", 0, "精密系統設計學士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "db06de5e-6308-4d04-a10e-aa174e32d17d", "1ce0231c-283c-4636-979b-ba143fa42b62", 0, "風險管理與保險學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "db431a07-9a7b-412c-9fb2-9ddbf7f29ac1", "1ce0231c-283c-4636-979b-ba143fa42b62", 1, "金融碩士在職學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "dc259920-dee3-48bd-8867-0a385abbadbe", "227f18d6-d050-4147-aedd-ba775a71a292", 0, "美國加州舊金山州立大學資訊工程雙學士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "ddde37e7-e11d-4006-9255-684e21ae97dd", "fa044fea-06d3-4f0e-a354-f335d761e2cb", 2, "電機與通訊工程博士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "de2fa011-c6d4-4fe3-8f09-4f3bbc8c476f", "13c57893-16db-478f-9b65-264ccb4898e6", 2, "統計學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "de446c35-bc1b-4df8-a471-f46ef2f7df27", "3e22a96f-c909-4c79-a903-8c0fef6c789a", 1, "工業工程與系統管理學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "de8c85e8-e2e2-4b66-850d-6559189448dc", "fa044fea-06d3-4f0e-a354-f335d761e2cb", 1, "資訊電機工程碩士在職專班" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "dfb33024-0039-4abc-ba22-4f33c5f541b7", "3e22a96f-c909-4c79-a903-8c0fef6c789a", 1, "電聲碩士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "e04de454-4806-4537-b90d-efee36d7e9cc", "f07974ca-3ef4-4166-a5ea-60b19980c05e", 0, "水利工程與資源保育學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "e203228a-8b3a-4e80-9272-349513e60b5d", "13c57893-16db-478f-9b65-264ccb4898e6", 2, "經濟學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "e38a5833-4636-4ebf-944c-eb235568b05f", "3e22a96f-c909-4c79-a903-8c0fef6c789a", 0, "應用數學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "e3c5d5cf-14ee-4019-92ee-11300a5572c6", "3e22a96f-c909-4c79-a903-8c0fef6c789a", 1, "數據科學碩士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "e3efe1d5-cc62-4b76-bab4-23597d6e73e4", "f07974ca-3ef4-4166-a5ea-60b19980c05e", 1, "土地管理學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "e67f99e3-92de-441c-b60b-9bea2901dad4", "8f0bd676-a08b-43e1-a4bb-4f32dbf8174f", 0, "室內設計進修學士班" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "e6b2f684-37ef-43ee-a166-edd71f4cdd9c", "d8458f4f-6a6a-4bad-97ee-6d1ce9aaf93a", 1, "文化與社會創新碩士學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "e74167e7-bfe8-49f1-96d0-60bd76cb346c", "3e22a96f-c909-4c79-a903-8c0fef6c789a", 1, "纖維與複合材料學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "e9001f5e-88a5-4aea-9acc-d3e5c471c306", "f07974ca-3ef4-4166-a5ea-60b19980c05e", 1, "土木工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "eaec0302-ce47-4e35-9a64-13e4c34cb289", "f07974ca-3ef4-4166-a5ea-60b19980c05e", 0, "都市計畫與空間資訊學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "ebdfb8e8-7a8d-495c-9429-12333ed77008", "fa044fea-06d3-4f0e-a354-f335d761e2cb", 1, "電機工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "ed0b6594-3e17-4c5a-8b61-232b2eb5d15d", "d8458f4f-6a6a-4bad-97ee-6d1ce9aaf93a", 0, "外國語文學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "efdd3c62-e7d0-4ba4-bb60-a5868ecc95f5", "3e22a96f-c909-4c79-a903-8c0fef6c789a", 0, "工業工程與系統管理學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "f0b0b676-0117-4fa3-921b-f075f694dff5", "a89c9f64-6562-44b9-b744-fc1050c575e5", 1, "經營管理碩士在職專班" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "f25888dc-8728-40a3-bd08-23f7e048a1d7", "f07974ca-3ef4-4166-a5ea-60b19980c05e", 1, "建設碩士在職學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "f2d5a4bd-df73-44eb-8383-7fccfafa6307", "fa044fea-06d3-4f0e-a354-f335d761e2cb", 0, "自動控制工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "f89178cf-ab6c-4a6c-b5f0-b3d2f2a4aaa7", "fa044fea-06d3-4f0e-a354-f335d761e2cb", 0, "電機工程學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "f9f38b0b-a95c-41a7-afb4-14bcfc3b00ae", "13c57893-16db-478f-9b65-264ccb4898e6", 0, "國際經營與貿易學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "fad8311c-37be-4e8a-a31b-81b0323db9c9", "1ce0231c-283c-4636-979b-ba143fa42b62", 1, "風險管理與保險學系" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "fc45c859-3016-494d-b878-257e8c4007a2", "8f0bd676-a08b-43e1-a4bb-4f32dbf8174f", 1, "建築碩士在職學位學程" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CollegeId", "Degree", "Name" },
                values: new object[] { "fd6d4884-2a28-4184-97ef-5af41f91aab6", "13c57893-16db-478f-9b65-264ccb4898e6", 0, "財稅學系" });

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
