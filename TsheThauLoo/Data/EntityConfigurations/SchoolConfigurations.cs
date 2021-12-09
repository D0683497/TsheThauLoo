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
            var ces = new College("工程與科學學院");
            builder.Entity<College>().HasData(ces);
            builder.Entity<Department>().HasData(
                new ("機械與電腦輔助工程學系", DegreeType.Bachelor, ces.Id),
                new ("纖維與複合材料學系", DegreeType.Bachelor, ces.Id),
                new ("工業工程與系統管理學系", DegreeType.Bachelor, ces.Id),
                new ("化學工程學系", DegreeType.Bachelor, ces.Id),
                new ("應用數學系", DegreeType.Bachelor, ces.Id),
                new ("航太與系統工程學系", DegreeType.Bachelor, ces.Id),
                new ("環境工程與科學學系", DegreeType.Bachelor, ces.Id),
                new ("材料科學與工程學系", DegreeType.Bachelor, ces.Id),
                new ("光電科學與工程學系", DegreeType.Bachelor, ces.Id),
                new ("精密系統設計學士學位學程", DegreeType.Bachelor, ces.Id),
                new ("機械與電腦輔助工程學系", DegreeType.Master, ces.Id),
                new ("纖維與複合材料學系", DegreeType.Master, ces.Id),
                new ("工業工程與系統管理學系", DegreeType.Master, ces.Id),
                new ("化學工程學系", DegreeType.Master, ces.Id),
                new ("數據科學碩士學位學程", DegreeType.Master, ces.Id),
                new ("航太與系統工程學系", DegreeType.Master, ces.Id),
                new ("環境工程與科學學系", DegreeType.Master, ces.Id),
                new ("材料科學與工程學系", DegreeType.Master, ces.Id),
                new ("光電科學與工程學系", DegreeType.Master, ces.Id),
                new ("電聲碩士學位學程", DegreeType.Master, ces.Id),
                new ("綠色能源科技碩士學位學程", DegreeType.Master, ces.Id),
                new ("智能製造與工程管理碩士在職學位學程", DegreeType.Master, ces.Id),
                new ("應用數學系", DegreeType.Master, ces.Id),
                new ("材料科學與工程學系", DegreeType.Doctor, ces.Id),
                new ("化學工程學系", DegreeType.Doctor, ces.Id),
                new ("纖維與複合材料學系", DegreeType.Doctor, ces.Id),
                new ("環境工程與科學學系", DegreeType.Doctor, ces.Id),
                new ("機械與航空工程博士學位學程", DegreeType.Doctor, ces.Id),
                new ("工業工程與系統管理學系", DegreeType.Doctor, ces.Id)
            );

            var business = new College("商學院");
            builder.Entity<College>().HasData(business);
            builder.Entity<Department>().HasData(
                new ("會計學系", DegreeType.Bachelor, business.Id),
                new ("企業管理學系", DegreeType.Bachelor, business.Id),
                new ("國際經營與貿易學系", DegreeType.Bachelor, business.Id),
                new ("財稅學系", DegreeType.Bachelor, business.Id),
                new ("統計學系", DegreeType.Bachelor, business.Id),
                new ("經濟學系", DegreeType.Bachelor, business.Id),
                new ("合作經濟暨社會事業經營學系", DegreeType.Bachelor, business.Id),
                new ("行銷學系", DegreeType.Bachelor, business.Id),
                new ("國際企業管理全英語學士學位學程", DegreeType.Bachelor, business.Id),
                new ("商學進修學士學位學程", DegreeType.Bachelor, business.Id),
                new ("商學進修學士班", DegreeType.Bachelor, business.Id),
                new ("會計學系", DegreeType.Master, business.Id),
                new ("企業管理學系", DegreeType.Master, business.Id),
                new ("國際經營與貿易學系", DegreeType.Master, business.Id),
                new ("財稅學系", DegreeType.Master, business.Id),
                new ("統計學系", DegreeType.Master, business.Id),
                new ("經濟學系", DegreeType.Master, business.Id),
                new ("合作經濟暨社會事業經營學系", DegreeType.Master, business.Id),
                new ("行銷學系", DegreeType.Master, business.Id),
                new ("財經法律研究所", DegreeType.Master, business.Id),
                new ("科技管理碩士學位學程", DegreeType.Master, business.Id),
                new ("商學專業碩士在職學位學程", DegreeType.Master, business.Id),
                new ("商學院商學專業碩士在職專班", DegreeType.Master, business.Id),
                new ("統計學系", DegreeType.Doctor, business.Id),
                new ("經濟學系", DegreeType.Doctor, business.Id),
                new ("商學博士學位學程", DegreeType.Doctor, business.Id)
            );

            var smd = new College("經營管理學院");
            builder.Entity<College>().HasData(smd);
            builder.Entity<Department>().HasData(
                new ("經營管理碩士在職學位學程", DegreeType.Master, smd.Id),
                new ("經營管理碩士在職專班", DegreeType.Master, smd.Id)
            );

            var cohss = new College("人文社會學院");
            builder.Entity<College>().HasData(cohss);
            builder.Entity<Department>().HasData(
                new ("中國文學系", DegreeType.Bachelor, cohss.Id),
                new ("外國語文學系", DegreeType.Bachelor, cohss.Id),
                new ("人文社會學士學位學程", DegreeType.Bachelor, cohss.Id),
                new ("中國文學系", DegreeType.Master, cohss.Id),
                new ("外國語文學系", DegreeType.Master, cohss.Id),
                new ("歷史與文物研究所", DegreeType.Master, cohss.Id),
                new ("公共事務與社會創新研究所", DegreeType.Master, cohss.Id),
                new ("文化與社會創新碩士學位學程", DegreeType.Master, cohss.Id),
                new ("公共事務與社會創新碩士在職學位學程", DegreeType.Master, cohss.Id),
                new ("中國文學系", DegreeType.Doctor, cohss.Id)
            );

            var coiee = new College("資訊電機學院");
            builder.Entity<College>().HasData(coiee);
            builder.Entity<Department>().HasData(
                new ("資訊工程學系", DegreeType.Bachelor, coiee.Id),
                new ("電子工程學系", DegreeType.Bachelor, coiee.Id),
                new ("電機工程學系", DegreeType.Bachelor, coiee.Id),
                new ("自動控制工程學系", DegreeType.Bachelor, coiee.Id),
                new ("通訊工程學系", DegreeType.Bachelor, coiee.Id),
                new ("資訊電機學院學士班", DegreeType.Bachelor, coiee.Id),
                new ("資訊工程學系", DegreeType.Master, coiee.Id),
                new ("電子工程學系", DegreeType.Master, coiee.Id),
                new ("電機工程學系", DegreeType.Master, coiee.Id),
                new ("自動控制工程學系", DegreeType.Master, coiee.Id),
                new ("通訊工程學系", DegreeType.Master, coiee.Id),
                new ("生醫資訊暨生醫工程碩士學位學程", DegreeType.Master, coiee.Id),
                new ("光電能源與視覺科技碩士在職專班", DegreeType.Master, coiee.Id),
                new ("視光科技碩士在職學位學程", DegreeType.Master, coiee.Id),
                new ("資訊電機工程碩士在職學位學程", DegreeType.Master, coiee.Id),
                new ("資訊電機工程碩士在職專班", DegreeType.Master, coiee.Id),
                new ("資訊工程學系", DegreeType.Doctor, coiee.Id),
                new ("電機與通訊工程博士學位學程", DegreeType.Doctor, coiee.Id),
                new ("智慧聯網產業博士學位學程", DegreeType.Doctor, coiee.Id)
            );

            var cocd = new College("建設學院");
            builder.Entity<College>().HasData(cocd);
            builder.Entity<Department>().HasData(
                new ("土木工程學系", DegreeType.Bachelor, cocd.Id),
                new ("水利工程與資源保育學系", DegreeType.Bachelor, cocd.Id),
                new ("運輸與物流學系", DegreeType.Bachelor, cocd.Id),
                new ("都市計畫與空間資訊學系", DegreeType.Bachelor, cocd.Id),
                new ("土地管理學系", DegreeType.Bachelor, cocd.Id),
                new ("土木工程學系", DegreeType.Master, cocd.Id),
                new ("水利工程與資源保育學系", DegreeType.Master, cocd.Id),
                new ("運輸與物流學系", DegreeType.Master, cocd.Id),
                new ("都市計畫與空間資訊學系", DegreeType.Master, cocd.Id),
                new ("土地管理學系", DegreeType.Master, cocd.Id),
                new ("景觀與遊憩碩士學位學程", DegreeType.Master, cocd.Id),
                new ("專案管理碩士在職學位學程", DegreeType.Master, cocd.Id),
                new ("建設學院專案管理碩士在職專班", DegreeType.Master, cocd.Id),
                new ("建設碩士在職學位學程", DegreeType.Master, cocd.Id),
                new ("智慧城市碩士學位學程", DegreeType.Master, cocd.Id),
                new ("西班牙薩拉戈薩大學物流供應鏈管理與創新創業雙碩士學位學程", DegreeType.Master, cocd.Id),
                new ("土木水利工程與建設規劃博士學位學程", DegreeType.Doctor, cocd.Id)
            );

            var cof = new College("金融學院");
            builder.Entity<College>().HasData(cof);
            builder.Entity<Department>().HasData(
                new ("風險管理與保險學系", DegreeType.Bachelor, cof.Id),
                new ("財務金融學系", DegreeType.Bachelor, cof.Id),
                new ("財務工程與精算學士學位學程", DegreeType.Bachelor, cof.Id),
                new ("風險管理與保險學系", DegreeType.Master, cof.Id),
                new ("財務金融學系", DegreeType.Master, cof.Id),
                new ("金融碩士在職學位學程", DegreeType.Master, cof.Id),
                new ("金融碩士在職專班", DegreeType.Master, cof.Id),
                new ("金融博士學位學程", DegreeType.Doctor, cof.Id)
            );

            var istm = new College("國際科技與管理學院");
            builder.Entity<College>().HasData(istm);
            builder.Entity<Department>().HasData(
                new ("澳洲墨爾本皇家理工大學商學與創新雙學士學位學程", DegreeType.Bachelor, istm.Id),
                new ("美國普渡大學電機資訊雙學士學位學程", DegreeType.Bachelor, istm.Id),
                new ("美國加州聖荷西州立大學商學大數據分析雙學士學位學程", DegreeType.Bachelor, istm.Id),
                new ("美國加州聖荷西州立大學工程雙學士學位學程", DegreeType.Bachelor, istm.Id),
                new ("美國加州舊金山州立大學資訊工程雙學士學位學程", DegreeType.Bachelor, istm.Id),
                new ("國際雙學士學位學程", DegreeType.Bachelor, istm.Id),
                new ("國際經營管理碩士學位學程", DegreeType.Master, istm.Id)
            );

            var archschool = new College("建築專業學院");
            builder.Entity<College>().HasData(archschool);
            builder.Entity<Department>().HasData(
                new("建築專業學院學士班", DegreeType.Bachelor, archschool.Id),
                new("建築學士學位學程", DegreeType.Bachelor, archschool.Id),
                new("創新設計學士學位學程", DegreeType.Bachelor, archschool.Id),
                new("室內設計學士學位學程", DegreeType.Bachelor, archschool.Id),
                new("室內設計進修學士班", DegreeType.Bachelor, archschool.Id),
                new("建築碩士學位學程", DegreeType.Master, archschool.Id),
                new("建築碩士在職學位學程", DegreeType.Master, archschool.Id),
                new("創意設計碩士學位學程", DegreeType.Master, archschool.Id)
            );

            var ischool = new College("創能學院");
            builder.Entity<College>().HasData(ischool);
            builder.Entity<Department>().HasData(new Department("人工智慧技術與應用學士學位學程", DegreeType.Bachelor, ischool.Id));
        }
    }
}
