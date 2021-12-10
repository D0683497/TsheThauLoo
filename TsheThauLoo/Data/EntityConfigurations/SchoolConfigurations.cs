using Microsoft.EntityFrameworkCore;
using TsheThauLoo.Entities.School;
using TsheThauLoo.Enums;

namespace TsheThauLoo.Data.EntityConfigurations;

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
        var ces = new College("orgyTzqhLxdCWYZrjnfNMwE88", "工程與科學學院");
        builder.Entity<College>().HasData(ces);
        builder.Entity<Department>().HasData(
            new ("E4TLO6Rlk845z2oUdcY4Iv06m", "機械與電腦輔助工程學系", DegreeType.Bachelor, ces.Id),
            new ("Ef1eMd6LtaZP03ZpODKQnkPrr", "纖維與複合材料學系", DegreeType.Bachelor, ces.Id),
            new ("1aVlra0qikcAV2JRV7TMRp7iI", "工業工程與系統管理學系", DegreeType.Bachelor, ces.Id),
            new ("lziprhEYXoNTIaifDM1jg2aDl", "化學工程學系", DegreeType.Bachelor, ces.Id),
            new ("V2mx0chIIFPm47C9QShaDcOse", "應用數學系", DegreeType.Bachelor, ces.Id),
            new ("t6iTp3drNHeKklGvntaCj4Y3x", "航太與系統工程學系", DegreeType.Bachelor, ces.Id),
            new ("jipCkuWAE0Du3bpRCRx8bsh6a", "環境工程與科學學系", DegreeType.Bachelor, ces.Id),
            new ("qqt2F3NiHmRZC3JQ2swBtlkAU", "材料科學與工程學系", DegreeType.Bachelor, ces.Id),
            new ("hpp8lWqxvHlO4UL5Jv8vIiyGV", "光電科學與工程學系", DegreeType.Bachelor, ces.Id),
            new ("M7uQ44lkDMXGUXTVIllFHIu0U", "精密系統設計學士學位學程", DegreeType.Bachelor, ces.Id),
            new ("W7hjovei6RLK2AL7i1wZAFaDM", "機械與電腦輔助工程學系", DegreeType.Master, ces.Id),
            new ("O9obZd6NxvP91r0V1HizlRmzx", "纖維與複合材料學系", DegreeType.Master, ces.Id),
            new ("vmfuonsJ6lEsGrfg1zor4wgjT", "工業工程與系統管理學系", DegreeType.Master, ces.Id),
            new ("Td6VsS8gaAJJFiqqG9rYtb8Ka", "化學工程學系", DegreeType.Master, ces.Id),
            new ("ShJZ82POiUdB5PCThigo9NHCJ", "數據科學碩士學位學程", DegreeType.Master, ces.Id),
            new ("MzTRWWzVUx4We6TIkh4N6UxzH", "航太與系統工程學系", DegreeType.Master, ces.Id),
            new ("8Qxn6SnBisKk0zL5ZaCNHzHNU", "環境工程與科學學系", DegreeType.Master, ces.Id),
            new ("wftcQpHOTzWoLLrA43mbVuSij", "材料科學與工程學系", DegreeType.Master, ces.Id),
            new ("ELz9CQQsBKPWDhP8dblVbBF6A", "光電科學與工程學系", DegreeType.Master, ces.Id),
            new ("E5JhN19uEM9iT39gupupFtKqb", "電聲碩士學位學程", DegreeType.Master, ces.Id),
            new ("mZItIEcOqGsf2D1CDB75ATNNb", "綠色能源科技碩士學位學程", DegreeType.Master, ces.Id),
            new ("z2GeW6JHLUxUyZp6ej88KdJ2z", "智能製造與工程管理碩士在職學位學程", DegreeType.Master, ces.Id),
            new ("27kTZ7FpUueqEEXZYr5VFvjaS", "應用數學系", DegreeType.Master, ces.Id),
            new ("alfxjGB18v40cO9P1exRs9bch", "材料科學與工程學系", DegreeType.Doctor, ces.Id),
            new ("7FSHfS7OyuHS5Dz0BoHkNvXSN", "化學工程學系", DegreeType.Doctor, ces.Id),
            new ("hclqd3Hyx4ibaMgdNbj4eWqwu", "纖維與複合材料學系", DegreeType.Doctor, ces.Id),
            new ("fHZZYLLRYQfq5zVpWYw2loUei", "環境工程與科學學系", DegreeType.Doctor, ces.Id),
            new ("ZIt17vZJhZYKuNu8uip8mpL4k", "機械與航空工程博士學位學程", DegreeType.Doctor, ces.Id),
            new ("ZMbKwvFHC8uItgeEHlBXvEHKU", "工業工程與系統管理學系", DegreeType.Doctor, ces.Id)
        );

        var business = new College("0dI3qiJ4paEsZAffJarvrZtL3", "商學院");
        builder.Entity<College>().HasData(business);
        builder.Entity<Department>().HasData(
            new ("CsgftORomoxhN0uKAtYU7smKM", "會計學系", DegreeType.Bachelor, business.Id),
            new ("cvje3HNALJWaFcUJfhOv1BT7f", "企業管理學系", DegreeType.Bachelor, business.Id),
            new ("DqV7WwMlTC5QCzi5rCvIh6hIm", "國際經營與貿易學系", DegreeType.Bachelor, business.Id),
            new ("DZCrDRmTMETLmS41PoVLReTcT", "財稅學系", DegreeType.Bachelor, business.Id),
            new ("JdbXmSGoUw4piAV9Ytx2AHq6G", "統計學系", DegreeType.Bachelor, business.Id),
            new ("0yn8nTHeTq33d7r8HGT972YKf", "經濟學系", DegreeType.Bachelor, business.Id),
            new ("YB0mkKPStKJuB1LIxf7YW1ZPf", "合作經濟暨社會事業經營學系", DegreeType.Bachelor, business.Id),
            new ("6hqWk2IvTqFH56Ul0UCmptGmu", "行銷學系", DegreeType.Bachelor, business.Id),
            new ("q3nlnIpsjsxXYpgNKlMttHXea", "國際企業管理全英語學士學位學程", DegreeType.Bachelor, business.Id),
            new ("SJNlUG18aLT02IvfQgsXTuK2H", "商學進修學士學位學程", DegreeType.Bachelor, business.Id),
            new ("de1Jnf9QD3gvMTPRwz4NxhVDw", "商學進修學士班", DegreeType.Bachelor, business.Id),
            new ("JkL8NFfUSTFriQGFfhftPkmpO", "會計學系", DegreeType.Master, business.Id),
            new ("1KMvherE36HxeETXr3xse9HZk", "企業管理學系", DegreeType.Master, business.Id),
            new ("leYi4G3rCzMLNDkQuA2F4DcH1", "國際經營與貿易學系", DegreeType.Master, business.Id),
            new ("JlZcnZDK4kpGlRLUPr6fk9xzb", "財稅學系", DegreeType.Master, business.Id),
            new ("DlcGMcK0dUdA5gRwdxmbM7oZG", "統計學系", DegreeType.Master, business.Id),
            new ("fhbrQBPIlbOHqqDpVel1bofqj", "經濟學系", DegreeType.Master, business.Id),
            new ("DEH5ixlYgCzxPcE1Npro3xXef", "合作經濟暨社會事業經營學系", DegreeType.Master, business.Id),
            new ("WWW1UII5bCGy6mZYliLWqTb3Y", "行銷學系", DegreeType.Master, business.Id),
            new ("DMYlpsYuNEgYb49p4pQpqa89V", "財經法律研究所", DegreeType.Master, business.Id),
            new ("qDSwwlratoGaz84ii6LibEFs2", "科技管理碩士學位學程", DegreeType.Master, business.Id),
            new ("5IhwNG2y1wlsaIfty7gyggsuL", "商學專業碩士在職學位學程", DegreeType.Master, business.Id),
            new ("WRxZ6INKBMk3Luk9pdlxWVbD8", "商學院商學專業碩士在職專班", DegreeType.Master, business.Id),
            new ("IgGhCciiJgrkqhSlPwFAURR2A", "統計學系", DegreeType.Doctor, business.Id),
            new ("Gl9ruZax8lgwKo8FrscM7G9wy", "經濟學系", DegreeType.Doctor, business.Id),
            new ("d0tSW85jXda65OgJG7QvJqA87", "商學博士學位學程", DegreeType.Doctor, business.Id)
        );

        var smd = new College("ZJQvZKxgdLSVgeAjnIBNkLUoD", "經營管理學院");
        builder.Entity<College>().HasData(smd);
        builder.Entity<Department>().HasData(
            new ("a8qq4y4du8qQFQ1E47zkSxRFj", "經營管理碩士在職學位學程", DegreeType.Master, smd.Id),
            new ("vaGAaenkbRlrHWXbU6GanCA2a", "經營管理碩士在職專班", DegreeType.Master, smd.Id)
        );

        var cohss = new College("Bem4x4ONNKfMVLge333K97tpP", "人文社會學院");
        builder.Entity<College>().HasData(cohss);
        builder.Entity<Department>().HasData(
            new ("wdVJDbdDmhz27blrxFLKIELqj", "中國文學系", DegreeType.Bachelor, cohss.Id),
            new ("7CoXdHz2bWFuTk7xE4jD3O626", "外國語文學系", DegreeType.Bachelor, cohss.Id),
            new ("Z3JpsBrwJF8BnPjrLnKyB1oHs", "人文社會學士學位學程", DegreeType.Bachelor, cohss.Id),
            new ("fDaJWG0j4tZKJD7LZxf84ERNh", "中國文學系", DegreeType.Master, cohss.Id),
            new ("quIjrAqQ12NKLW2LL0DIDbMdS", "外國語文學系", DegreeType.Master, cohss.Id),
            new ("XqGWrO7VR16BZRlJBbOixhfAz", "歷史與文物研究所", DegreeType.Master, cohss.Id),
            new ("VTUjz812ZDfz7dcl6oNf8yHqJ", "公共事務與社會創新研究所", DegreeType.Master, cohss.Id),
            new ("Dh4oZEA9zkc3ACZaRM9VtdQBH", "文化與社會創新碩士學位學程", DegreeType.Master, cohss.Id),
            new ("2Ao6OxweQUKatNTwLqL1SWUIx", "公共事務與社會創新碩士在職學位學程", DegreeType.Master, cohss.Id),
            new ("OYM5tHafmDVssmZLQHtK5Sit2", "中國文學系", DegreeType.Doctor, cohss.Id)
        );

        var coiee = new College("if0gyRKzbTjqNb2BCn89dAVfA", "資訊電機學院");
        builder.Entity<College>().HasData(coiee);
        builder.Entity<Department>().HasData(
            new ("oW8SWY9EukV5Ywid5WrxTVHau", "資訊工程學系", DegreeType.Bachelor, coiee.Id),
            new ("90GsDYOXGVH4l3KVGxYol1W2o", "電子工程學系", DegreeType.Bachelor, coiee.Id),
            new ("rxjYhZnZzLRGlbkWdLbRpGCgD", "電機工程學系", DegreeType.Bachelor, coiee.Id),
            new ("HZToLVKBIxKg1pcG4s8YqT3eo", "自動控制工程學系", DegreeType.Bachelor, coiee.Id),
            new ("RLi7RVRW3IUQCOCLQQZIUrfIs", "通訊工程學系", DegreeType.Bachelor, coiee.Id),
            new ("HjMLEuFapCEXLd1M1TUxLd9sL", "資訊電機學院學士班", DegreeType.Bachelor, coiee.Id),
            new ("5kPtkwk26CsmYF92Yjm7eFS25", "資訊工程學系", DegreeType.Master, coiee.Id),
            new ("nONXyEz08bBBJdn66Cvq3oMIt", "電子工程學系", DegreeType.Master, coiee.Id),
            new ("5zNvuPL5SJVEYi3a10zPEQO8p", "電機工程學系", DegreeType.Master, coiee.Id),
            new ("2L99Mx5zbL0nVEVZ5SQwrdvPp", "自動控制工程學系", DegreeType.Master, coiee.Id),
            new ("dPxcwXdkxCCGd02FuI0mhswrG", "通訊工程學系", DegreeType.Master, coiee.Id),
            new ("oJ9mWh0t2QzrKyVbqOKy6Uxla", "生醫資訊暨生醫工程碩士學位學程", DegreeType.Master, coiee.Id),
            new ("PZBs3lnNWZ1KJKiC91m09gRwG", "光電能源與視覺科技碩士在職專班", DegreeType.Master, coiee.Id),
            new ("5BMNEb4vjpRMSBwQZP3zCDnpB", "視光科技碩士在職學位學程", DegreeType.Master, coiee.Id),
            new ("E21Gz0eq5OnXxHgLXGJsYIKmn", "資訊電機工程碩士在職學位學程", DegreeType.Master, coiee.Id),
            new ("5mecRMeCuQzDF9PrBjSUsCDtc", "資訊電機工程碩士在職專班", DegreeType.Master, coiee.Id),
            new ("12Hz7QGyAnqdqT7hMVpIX3ouB", "資訊工程學系", DegreeType.Doctor, coiee.Id),
            new ("ZOog2nEV9O1zrMv4DVRyQU55I", "電機與通訊工程博士學位學程", DegreeType.Doctor, coiee.Id),
            new ("6AEGePPNeOcgGUdevl7XD9v69", "智慧聯網產業博士學位學程", DegreeType.Doctor, coiee.Id)
        );

        var cocd = new College("QGqpVsNzjcusLw2lisuECLEaT", "建設學院");
        builder.Entity<College>().HasData(cocd);
        builder.Entity<Department>().HasData(
            new ("QxgyniralF6EKpnroqXQdcCtS", "土木工程學系", DegreeType.Bachelor, cocd.Id),
            new ("3qR46zhPrQyWVkMfXVWT5ikFs", "水利工程與資源保育學系", DegreeType.Bachelor, cocd.Id),
            new ("9cQA7y6zMOXj4VrpoNbukepq3", "運輸與物流學系", DegreeType.Bachelor, cocd.Id),
            new ("", "都市計畫與空間資訊學系", DegreeType.Bachelor, cocd.Id),
            new ("AhS7F9gUmxfbQ45agBzV6ZqyK", "土地管理學系", DegreeType.Bachelor, cocd.Id),
            new ("qlJ5WI3YfX90fA4NKrt3EKXzI", "土木工程學系", DegreeType.Master, cocd.Id),
            new ("mroQH0gDaaGHw3B31MDeWX2cO", "水利工程與資源保育學系", DegreeType.Master, cocd.Id),
            new ("OD1yUIm1w5q3rV5VKxKZY6aJg", "運輸與物流學系", DegreeType.Master, cocd.Id),
            new ("8CDofLpCA2z33Y4pXEKpPmCrf", "都市計畫與空間資訊學系", DegreeType.Master, cocd.Id),
            new ("rm3BkevoEpzLf1eMVqE5Yo0ce", "土地管理學系", DegreeType.Master, cocd.Id),
            new ("117awZ9RQvBTaStkYBP3jk9qL", "景觀與遊憩碩士學位學程", DegreeType.Master, cocd.Id),
            new ("WucZWrsuYrvozv4TWeWJW61IU", "專案管理碩士在職學位學程", DegreeType.Master, cocd.Id),
            new ("C3ps4hxYL8vu0hl6hpAcRYL7i", "建設學院專案管理碩士在職專班", DegreeType.Master, cocd.Id),
            new ("zYc5qrrI7ZTyUdEY8mmOX2aN8", "建設碩士在職學位學程", DegreeType.Master, cocd.Id),
            new ("J3ppFIMFhj8EuCGUOOmCP0bL6", "智慧城市碩士學位學程", DegreeType.Master, cocd.Id),
            new ("Iw4sxzEHJ2ohiitIGz8RAPpoA", "西班牙薩拉戈薩大學物流供應鏈管理與創新創業雙碩士學位學程", DegreeType.Master, cocd.Id),
            new ("cbKctLMMlvmtYSLCDezoMWaDx", "土木水利工程與建設規劃博士學位學程", DegreeType.Doctor, cocd.Id)
        );

        var cof = new College("iQJHwPwg4VAFwjXDJPVH2wtE0", "金融學院");
        builder.Entity<College>().HasData(cof);
        builder.Entity<Department>().HasData(
            new ("UvKL9QK7ThPRzLH7V8Zrp1Chu", "風險管理與保險學系", DegreeType.Bachelor, cof.Id),
            new ("z4sSzyyUCZGGcmj2hvL9y8XCf", "財務金融學系", DegreeType.Bachelor, cof.Id),
            new ("i5Fj90mDet7rvSsNYDPY5YU24", "財務工程與精算學士學位學程", DegreeType.Bachelor, cof.Id),
            new ("jKDRVp3ZfjFQsTIho96YUFmAj", "風險管理與保險學系", DegreeType.Master, cof.Id),
            new ("PAkm3KYC60qsMU9o9xuczaz65", "財務金融學系", DegreeType.Master, cof.Id),
            new ("pel3fUEJ3dUuTmhngqXuMARH7", "金融碩士在職學位學程", DegreeType.Master, cof.Id),
            new ("o7HQY0mdOG2FCCMfPv3A0eiyU", "金融碩士在職專班", DegreeType.Master, cof.Id),
            new ("b7Qo0Z5zzhREULXvVgAsQbawy", "金融博士學位學程", DegreeType.Doctor, cof.Id)
        );

        var istm = new College("9xP6LTPTAcSWkRF6cx9Iq7cDv", "國際科技與管理學院");
        builder.Entity<College>().HasData(istm);
        builder.Entity<Department>().HasData(
            new ("ZGSzhoQ0SYpxasVopp9KRZha7", "澳洲墨爾本皇家理工大學商學與創新雙學士學位學程", DegreeType.Bachelor, istm.Id),
            new ("YX6zcrayVMdPrflcZBZVF58BR", "美國普渡大學電機資訊雙學士學位學程", DegreeType.Bachelor, istm.Id),
            new ("AawwxbqguYo02vgUmruY8q1H5", "美國加州聖荷西州立大學商學大數據分析雙學士學位學程", DegreeType.Bachelor, istm.Id),
            new ("0IAQg7YRS9A2v1yDs3V6DS3fE", "美國加州聖荷西州立大學工程雙學士學位學程", DegreeType.Bachelor, istm.Id),
            new ("mB10Nk6FneHFt04XuF84iJKc7", "美國加州舊金山州立大學資訊工程雙學士學位學程", DegreeType.Bachelor, istm.Id),
            new ("a6VsUoLnqqvOYVFOOYZXP9vW6", "國際雙學士學位學程", DegreeType.Bachelor, istm.Id),
            new ("Lxx5OqarnVEwetnl8RCX4oAdy", "國際經營管理碩士學位學程", DegreeType.Master, istm.Id)
        );

        var archschool = new College("woYRzMjOYVT0LlwVEvqarGaoS", "建築專業學院");
        builder.Entity<College>().HasData(archschool);
        builder.Entity<Department>().HasData(
            new ("nzgPilWPhn1M0OWMU3dirxo0P", "建築專業學院學士班", DegreeType.Bachelor, archschool.Id),
            new ("yRkkVDq2xMAOV6meUnmb1O4iV", "建築學士學位學程", DegreeType.Bachelor, archschool.Id),
            new ("aBP9gqZAzXx56nmIgKvFnDz3O", "創新設計學士學位學程", DegreeType.Bachelor, archschool.Id),
            new ("IKoM8CjCMFhQIkxBq3VPnMMp9", "室內設計學士學位學程", DegreeType.Bachelor, archschool.Id),
            new ("TpmRpkDFfhrtlg14kuI55C29i", "室內設計進修學士班", DegreeType.Bachelor, archschool.Id),
            new ("yXJHEN90oBLQmE5rmnVgFP88G", "建築碩士學位學程", DegreeType.Master, archschool.Id),
            new ("oQnr4w8CM9SqRcM75XIOJhfNc", "建築碩士在職學位學程", DegreeType.Master, archschool.Id),
            new ("Dm3w09uagkM33e8ZrKJjWyk1p", "創意設計碩士學位學程", DegreeType.Master, archschool.Id)
        );

        var ischool = new College("ggRSStSxsHsmiT3yvftunUmCm", "創能學院");
        builder.Entity<College>().HasData(ischool);
        builder.Entity<Department>().HasData(new Department("4d9tHiUYHz5Xe7Wzepc6RShZ7", "人工智慧技術與應用學士學位學程", DegreeType.Bachelor, ischool.Id));
    }
}