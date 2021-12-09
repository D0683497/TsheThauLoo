using Microsoft.EntityFrameworkCore;
using TsheThauLoo.Entities.School;
using TsheThauLoo.Enums;

namespace TsheThauLoo.Data.EntityConfigurations
{
    public static class SchoolConfigurations
    {
        public static void Relation(ModelBuilder builder)
        {
            builder.Entity<College>(b =>
            {
                b.HasMany(e => e.Departments)
                    .WithOne(e => e.College)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        public static void Initialize(ModelBuilder builder)
        {
            var ces = new College("3e22a96f-c909-4c79-a903-8c0fef6c789a", "工程與科學學院");
            builder.Entity<College>().HasData(ces);
            builder.Entity<Department>().HasData(
                new ("54a533be-ab21-4c64-8d4c-98b8f65ef77b", "機械與電腦輔助工程學系", DegreeType.Bachelor, ces.Id),
                new ("4ab35fc1-d012-44ae-a3a7-aa5f9333ab91", "纖維與複合材料學系", DegreeType.Bachelor, ces.Id),
                new ("efdd3c62-e7d0-4ba4-bb60-a5868ecc95f5", "工業工程與系統管理學系", DegreeType.Bachelor, ces.Id),
                new ("4d969aab-d6fc-4ca3-be20-5f71fd3e22ba", "化學工程學系", DegreeType.Bachelor, ces.Id),
                new ("e38a5833-4636-4ebf-944c-eb235568b05f", "應用數學系", DegreeType.Bachelor, ces.Id),
                new ("86bc1a5e-0543-48a4-9d88-786e5e49b5b5", "航太與系統工程學系", DegreeType.Bachelor, ces.Id),
                new ("829d2e71-7b53-4080-ba86-b82defea9099", "環境工程與科學學系", DegreeType.Bachelor, ces.Id),
                new ("577e5175-c71b-4b17-890a-0f2733a4ca7c", "材料科學與工程學系", DegreeType.Bachelor, ces.Id),
                new ("5684f51e-7506-43d4-9253-c8b60912653d", "光電科學與工程學系", DegreeType.Bachelor, ces.Id),
                new ("da19b793-5507-4363-9c82-c8779cad9a98", "精密系統設計學士學位學程", DegreeType.Bachelor, ces.Id),
                new ("119e1c67-f803-4dc3-959b-a0673220c71c", "機械與電腦輔助工程學系", DegreeType.Master, ces.Id),
                new ("e74167e7-bfe8-49f1-96d0-60bd76cb346c", "纖維與複合材料學系", DegreeType.Master, ces.Id),
                new ("de446c35-bc1b-4df8-a471-f46ef2f7df27", "工業工程與系統管理學系", DegreeType.Master, ces.Id),
                new ("c9e56c0e-2cff-459d-90ad-9efea62403e2", "化學工程學系", DegreeType.Master, ces.Id),
                new ("e3c5d5cf-14ee-4019-92ee-11300a5572c6", "數據科學碩士學位學程", DegreeType.Master, ces.Id),
                new ("4b70a8b9-55cc-4c59-961a-3e2e45205001", "航太與系統工程學系", DegreeType.Master, ces.Id),
                new ("593441cb-d226-4a17-9f44-f43ed4ad3304", "環境工程與科學學系", DegreeType.Master, ces.Id),
                new ("4bd31f0a-0e32-42bc-bd3d-369b2734e4d6", "材料科學與工程學系", DegreeType.Master, ces.Id),
                new ("56582cf6-842d-47bb-a584-cb116e913690", "光電科學與工程學系", DegreeType.Master, ces.Id),
                new ("dfb33024-0039-4abc-ba22-4f33c5f541b7", "電聲碩士學位學程", DegreeType.Master, ces.Id),
                new ("9f7b6093-a30c-43f6-9b80-e65b919c26ae", "綠色能源科技碩士學位學程", DegreeType.Master, ces.Id),
                new ("0753a654-47eb-417d-ace0-9e8ebd6a814c", "智能製造與工程管理碩士在職學位學程", DegreeType.Master, ces.Id),
                new ("3f07e6c0-36ea-4b4d-8793-74dcb1cadb7e", "應用數學系", DegreeType.Master, ces.Id),
                new ("36d94909-69ac-4f83-a541-54e5c02edd5f", "材料科學與工程學系", DegreeType.Doctor, ces.Id),
                new ("7701b7ae-1dfa-459b-9490-129b6ea80f7f", "化學工程學系", DegreeType.Doctor, ces.Id),
                new ("8a40ac5d-f752-41ec-8546-2d320515e62b", "纖維與複合材料學系", DegreeType.Doctor, ces.Id),
                new ("0aec387a-093b-4dc0-a9b8-21a684c94d95", "環境工程與科學學系", DegreeType.Doctor, ces.Id),
                new ("0a05fc3d-08a2-42e0-aeda-1efac68e94b5", "機械與航空工程博士學位學程", DegreeType.Doctor, ces.Id),
                new ("5894f2e6-09c3-4808-83fe-f28b170bc9ca", "工業工程與系統管理學系", DegreeType.Doctor, ces.Id)
            );

            var business = new College("13c57893-16db-478f-9b65-264ccb4898e6", "商學院");
            builder.Entity<College>().HasData(business);
            builder.Entity<Department>().HasData(
                new ("266c7fd6-474e-43dc-8897-ea3bc31a8e0d", "會計學系", DegreeType.Bachelor, business.Id),
                new ("4e1cd01d-94bd-42de-807b-2db5ad803059", "企業管理學系", DegreeType.Bachelor, business.Id),
                new ("f9f38b0b-a95c-41a7-afb4-14bcfc3b00ae", "國際經營與貿易學系", DegreeType.Bachelor, business.Id),
                new ("fd6d4884-2a28-4184-97ef-5af41f91aab6", "財稅學系", DegreeType.Bachelor, business.Id),
                new ("1faab6e3-45e9-4f2a-b9c6-d515a80ee74e", "統計學系", DegreeType.Bachelor, business.Id),
                new ("14f08a09-0f6d-4a8c-aa7a-31999d32ab94", "經濟學系", DegreeType.Bachelor, business.Id),
                new ("567323ad-82f1-4531-b430-c6d0efb4b3dd", "合作經濟暨社會事業經營學系", DegreeType.Bachelor, business.Id),
                new ("1d50af56-b8e1-4aef-b19f-b6954e3223c5", "行銷學系", DegreeType.Bachelor, business.Id),
                new ("86f378ce-6114-4808-952c-07668dbaf9fd", "國際企業管理全英語學士學位學程", DegreeType.Bachelor, business.Id),
                new ("45436abe-74f2-45f2-a2f5-93c931cdf121", "商學進修學士學位學程", DegreeType.Bachelor, business.Id),
                new ("b64bc109-a3c4-4aee-b581-5d6cdcb3ab0d", "商學進修學士班", DegreeType.Bachelor, business.Id),
                new ("bbb9d4b7-bc53-4385-8ce5-a2913be97bcc", "會計學系", DegreeType.Master, business.Id),
                new ("cc3a263f-efff-4abc-8088-281a61a9c571", "企業管理學系", DegreeType.Master, business.Id),
                new ("4777fce8-c37d-43a4-9428-60abafbeb128", "國際經營與貿易學系", DegreeType.Master, business.Id),
                new ("037122b4-04e7-4666-a9f4-4cbaf3b1790a", "財稅學系", DegreeType.Master, business.Id),
                new ("6046654d-8b50-4915-ba2b-e649ea32913f", "統計學系", DegreeType.Master, business.Id),
                new ("70488f20-cd2d-4b88-90e3-93cbab0c46a2", "經濟學系", DegreeType.Master, business.Id),
                new ("49cac5bf-6920-4b47-a3a4-ba794f692515", "合作經濟暨社會事業經營學系", DegreeType.Master, business.Id),
                new ("57de29ef-f94c-4ade-bf49-3148c6668b72", "行銷學系", DegreeType.Master, business.Id),
                new ("51b58c88-e5e6-4112-a2ea-f3ffd5018669", "財經法律研究所", DegreeType.Master, business.Id),
                new ("08392375-344e-472d-a4e1-e0dce298f3b5", "科技管理碩士學位學程", DegreeType.Master, business.Id),
                new ("675390ab-fcad-4817-9868-a77bdf6b2e78", "商學專業碩士在職學位學程", DegreeType.Master, business.Id),
                new ("2daa2f2f-c287-473b-b0cf-05fa1ffb7e13", "商學院商學專業碩士在職專班", DegreeType.Master, business.Id),
                new ("de2fa011-c6d4-4fe3-8f09-4f3bbc8c476f", "統計學系", DegreeType.Doctor, business.Id),
                new ("e203228a-8b3a-4e80-9272-349513e60b5d", "經濟學系", DegreeType.Doctor, business.Id),
                new ("c0e7ee82-ca03-4327-93c3-cfa7af9f69b7", "商學博士學位學程", DegreeType.Doctor, business.Id)
            );

            var smd = new College("a89c9f64-6562-44b9-b744-fc1050c575e5", "經營管理學院");
            builder.Entity<College>().HasData(smd);
            builder.Entity<Department>().HasData(
                new ("5aa9154a-de0a-4e15-8d73-855f570a8517", "經營管理碩士在職學位學程", DegreeType.Master, smd.Id),
                new ("f0b0b676-0117-4fa3-921b-f075f694dff5", "經營管理碩士在職專班", DegreeType.Master, smd.Id)
            );

            var cohss = new College("d8458f4f-6a6a-4bad-97ee-6d1ce9aaf93a", "人文社會學院");
            builder.Entity<College>().HasData(cohss);
            builder.Entity<Department>().HasData(
                new ("2d3d53f4-2f7c-481c-b6bb-6813839a69eb", "中國文學系", DegreeType.Bachelor, cohss.Id),
                new ("ed0b6594-3e17-4c5a-8b61-232b2eb5d15d", "外國語文學系", DegreeType.Bachelor, cohss.Id),
                new ("6ae91071-ab05-4eca-94d5-fd2f89362e53", "人文社會學士學位學程", DegreeType.Bachelor, cohss.Id),
                new ("9af2920a-888b-4d66-8ba8-79816b4cf9a8", "中國文學系", DegreeType.Master, cohss.Id),
                new ("5f683b1e-ffa1-48d3-a8bc-43740a755505", "外國語文學系", DegreeType.Master, cohss.Id),
                new ("236e7e9d-dc6c-441a-96b1-7f89a42293e5", "歷史與文物研究所", DegreeType.Master, cohss.Id),
                new ("a8fe8fe4-fecf-427d-8599-5150354cbb5a", "公共事務與社會創新研究所", DegreeType.Master, cohss.Id),
                new ("e6b2f684-37ef-43ee-a166-edd71f4cdd9c", "文化與社會創新碩士學位學程", DegreeType.Master, cohss.Id),
                new ("9aa5dd68-26e8-4abe-959a-340539a4603a", "公共事務與社會創新碩士在職學位學程", DegreeType.Master, cohss.Id),
                new ("42946f30-e417-4347-b93e-f8fdfb8a34ad", "中國文學系", DegreeType.Doctor, cohss.Id)
            );

            var coiee = new College("fa044fea-06d3-4f0e-a354-f335d761e2cb", "資訊電機學院");
            builder.Entity<College>().HasData(coiee);
            builder.Entity<Department>().HasData(
                new ("b40215ab-2609-4464-b24e-bdfbc5d3ed2b", "資訊工程學系", DegreeType.Bachelor, coiee.Id),
                new ("2bbc4650-a47f-45a9-a220-b41f976011e5", "電子工程學系", DegreeType.Bachelor, coiee.Id),
                new ("f89178cf-ab6c-4a6c-b5f0-b3d2f2a4aaa7", "電機工程學系", DegreeType.Bachelor, coiee.Id),
                new ("f2d5a4bd-df73-44eb-8383-7fccfafa6307", "自動控制工程學系", DegreeType.Bachelor, coiee.Id),
                new ("658b7def-5592-45c5-9059-5628034e3d59", "通訊工程學系", DegreeType.Bachelor, coiee.Id),
                new ("621e0eb8-1bd2-4669-b6e7-43eed1596a1c", "資訊電機學院學士班", DegreeType.Bachelor, coiee.Id),
                new ("367992b7-f691-4337-aade-f02336ea9859", "資訊工程學系", DegreeType.Master, coiee.Id),
                new ("5f3f21fb-3adf-4ca4-aef0-4cfd968c1f57", "電子工程學系", DegreeType.Master, coiee.Id),
                new ("ebdfb8e8-7a8d-495c-9429-12333ed77008", "電機工程學系", DegreeType.Master, coiee.Id),
                new ("47f0dab8-4507-491a-8871-f057d856c55e", "自動控制工程學系", DegreeType.Master, coiee.Id),
                new ("d77a826e-736f-49b6-8987-f4df06a71cf7", "通訊工程學系", DegreeType.Master, coiee.Id),
                new ("05d8034e-7a32-4244-af88-a8e82885c002", "生醫資訊暨生醫工程碩士學位學程", DegreeType.Master, coiee.Id),
                new ("bc526bfb-6db9-44c0-bb88-44641663c933", "光電能源與視覺科技碩士在職專班", DegreeType.Master, coiee.Id),
                new ("52629b36-cb7c-4c34-af93-0f5b34110bd2", "視光科技碩士在職學位學程", DegreeType.Master, coiee.Id),
                new ("b404fded-a6bd-4648-93ff-32568b51c8b3", "資訊電機工程碩士在職學位學程", DegreeType.Master, coiee.Id),
                new ("de8c85e8-e2e2-4b66-850d-6559189448dc", "資訊電機工程碩士在職專班", DegreeType.Master, coiee.Id),
                new ("0db86b82-81ca-42ed-bc5f-a18ebbab6924", "資訊工程學系", DegreeType.Doctor, coiee.Id),
                new ("ddde37e7-e11d-4006-9255-684e21ae97dd", "電機與通訊工程博士學位學程", DegreeType.Doctor, coiee.Id),
                new ("b33110d1-3c0d-491f-9cc3-a05bdff11f4d", "智慧聯網產業博士學位學程", DegreeType.Doctor, coiee.Id)
            );

            var cocd = new College("f07974ca-3ef4-4166-a5ea-60b19980c05e", "建設學院");
            builder.Entity<College>().HasData(cocd);
            builder.Entity<Department>().HasData(
                new ("825646bc-8423-46ce-af79-169660dd2571", "土木工程學系", DegreeType.Bachelor, cocd.Id),
                new ("e04de454-4806-4537-b90d-efee36d7e9cc", "水利工程與資源保育學系", DegreeType.Bachelor, cocd.Id),
                new ("629fd6d4-f81e-43f3-951c-d63e5ab9a78d", "運輸與物流學系", DegreeType.Bachelor, cocd.Id),
                new ("eaec0302-ce47-4e35-9a64-13e4c34cb289", "都市計畫與空間資訊學系", DegreeType.Bachelor, cocd.Id),
                new ("49635313-4992-4e22-b1a6-192127777ee6", "土地管理學系", DegreeType.Bachelor, cocd.Id),
                new ("e9001f5e-88a5-4aea-9acc-d3e5c471c306", "土木工程學系", DegreeType.Master, cocd.Id),
                new ("3d788520-2708-433c-ab8a-9ffbd647faf5", "水利工程與資源保育學系", DegreeType.Master, cocd.Id),
                new ("bb7be155-823c-4337-90d3-1db9a587cb3e", "運輸與物流學系", DegreeType.Master, cocd.Id),
                new ("2bbeb586-7c3f-4e17-ad2f-4d94b6adc796", "都市計畫與空間資訊學系", DegreeType.Master, cocd.Id),
                new ("e3efe1d5-cc62-4b76-bab4-23597d6e73e4", "土地管理學系", DegreeType.Master, cocd.Id),
                new ("6ff1a064-845c-4ca5-840a-ca29694592c5", "景觀與遊憩碩士學位學程", DegreeType.Master, cocd.Id),
                new ("0ebdf6ea-88f1-499f-88ae-f6d3646d92af", "專案管理碩士在職學位學程", DegreeType.Master, cocd.Id),
                new ("86e3f7a6-b12c-4c08-8733-aa125de779b9", "建設學院專案管理碩士在職專班", DegreeType.Master, cocd.Id),
                new ("f25888dc-8728-40a3-bd08-23f7e048a1d7", "建設碩士在職學位學程", DegreeType.Master, cocd.Id),
                new ("b1bae1c5-6b9f-41de-b42f-cd82a7dad8f2", "智慧城市碩士學位學程", DegreeType.Master, cocd.Id),
                new ("576d232b-0660-4156-918f-a55e24c9ca99", "西班牙薩拉戈薩大學物流供應鏈管理與創新創業雙碩士學位學程", DegreeType.Master, cocd.Id),
                new ("c134861a-3f57-4f83-a0b0-ea79024a96c8", "土木水利工程與建設規劃博士學位學程", DegreeType.Doctor, cocd.Id)
            );

            var cof = new College("1ce0231c-283c-4636-979b-ba143fa42b62", "金融學院");
            builder.Entity<College>().HasData(cof);
            builder.Entity<Department>().HasData(
                new ("db06de5e-6308-4d04-a10e-aa174e32d17d", "風險管理與保險學系", DegreeType.Bachelor, cof.Id),
                new ("2ccfe6b4-06fc-47d9-9c1b-82a501fa1058", "財務金融學系", DegreeType.Bachelor, cof.Id),
                new ("4f8e93da-b82e-48da-b543-f3a524d6113c", "財務工程與精算學士學位學程", DegreeType.Bachelor, cof.Id),
                new ("fad8311c-37be-4e8a-a31b-81b0323db9c9", "風險管理與保險學系", DegreeType.Master, cof.Id),
                new ("41496773-ee83-4cdf-b60d-bacc7689b9c0", "財務金融學系", DegreeType.Master, cof.Id),
                new ("db431a07-9a7b-412c-9fb2-9ddbf7f29ac1", "金融碩士在職學位學程", DegreeType.Master, cof.Id),
                new ("76b83e56-7c5b-4595-97b1-49ea6106d2cf", "金融碩士在職專班", DegreeType.Master, cof.Id),
                new ("08dd0723-8c09-4b5c-b4e4-86d48b8bb144", "金融博士學位學程", DegreeType.Doctor, cof.Id)
            );

            var istm = new College("227f18d6-d050-4147-aedd-ba775a71a292", "國際科技與管理學院");
            builder.Entity<College>().HasData(istm);
            builder.Entity<Department>().HasData(
                new ("c742d386-401e-44aa-8b3e-892f407ee6fc", "澳洲墨爾本皇家理工大學商學與創新雙學士學位學程", DegreeType.Bachelor, istm.Id),
                new ("335b234b-d7d4-4c55-840e-0e83d57da5cb", "美國普渡大學電機資訊雙學士學位學程", DegreeType.Bachelor, istm.Id),
                new ("8cbe768c-3397-4251-b784-49dd8e6e99cf", "美國加州聖荷西州立大學商學大數據分析雙學士學位學程", DegreeType.Bachelor, istm.Id),
                new ("0b4046c5-ff28-4b77-bd47-262b7def018d", "美國加州聖荷西州立大學工程雙學士學位學程", DegreeType.Bachelor, istm.Id),
                new ("dc259920-dee3-48bd-8867-0a385abbadbe", "美國加州舊金山州立大學資訊工程雙學士學位學程", DegreeType.Bachelor, istm.Id),
                new ("30b602f3-6586-4f43-99ae-bdef268d0b64", "國際雙學士學位學程", DegreeType.Bachelor, istm.Id),
                new ("21879ce1-3894-4d02-97d3-b222a21b439c", "國際經營管理碩士學位學程", DegreeType.Master, istm.Id)
            );

            var archschool = new College("8f0bd676-a08b-43e1-a4bb-4f32dbf8174f", "建築專業學院");
            builder.Entity<College>().HasData(archschool);
            builder.Entity<Department>().HasData(
                new("599a3556-ba31-460e-9922-ba345f9b83e6", "建築專業學院學士班", DegreeType.Bachelor, archschool.Id),
                new("5336690e-f31c-4c33-837a-8be30a4a7ceb", "建築學士學位學程", DegreeType.Bachelor, archschool.Id),
                new("9dfcf85b-65da-4691-a864-ea1e478ec960", "創新設計學士學位學程", DegreeType.Bachelor, archschool.Id),
                new("0b7b6c54-59c8-4eb7-a47d-477572bf464c", "室內設計學士學位學程", DegreeType.Bachelor, archschool.Id),
                new("e67f99e3-92de-441c-b60b-9bea2901dad4", "室內設計進修學士班", DegreeType.Bachelor, archschool.Id),
                new("6b9b4a9c-22bd-4deb-86b7-c7bc1705703d", "建築碩士學位學程", DegreeType.Master, archschool.Id),
                new("fc45c859-3016-494d-b878-257e8c4007a2", "建築碩士在職學位學程", DegreeType.Master, archschool.Id),
                new("03492289-e244-4aa9-90ff-58190616ab12", "創意設計碩士學位學程", DegreeType.Master, archschool.Id)
            );

            var ischool = new College("3041f226-9230-4756-935d-8e0fc1ed112d", "創能學院");
            builder.Entity<College>().HasData(ischool);
            builder.Entity<Department>().HasData(new Department("25c0ee9a-c2fe-480b-9526-61f01f68d527", "人工智慧技術與應用學士學位學程", DegreeType.Bachelor, ischool.Id));
        }
    }
}
