using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TsheThauLoo.Entities.User;

namespace TsheThauLoo.Data
{
    public class DataSeeder
    {
        public static async Task Initialize(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<DataSeeder>>();
                var dbContext = services.GetRequiredService<TsheThauLooDbContext>();
                
                logger.LogInformation("開始創建資料庫");
                if (await dbContext.Database.EnsureCreatedAsync())
                {
                    logger.LogInformation("開始創建角色及角色聲明");
                    await CreateRoleAsync(services, logger);
                    logger.LogInformation("創建角色及角色聲明完成");
                }
                else
                {
                    logger.LogInformation("資料庫已存在");
                }
                
                logger.LogInformation("開始創建資料夾");
                CreateFolder(logger);
            }
        }
        
        private static async Task CreateRoleAsync(IServiceProvider services, ILogger<DataSeeder> logger)
        {
            var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
            var roleList = new List<ApplicationRole>
            {
                new ApplicationRole { Name = "Administrator" },
                new ApplicationRole { Name = "Alumnus" },
                new ApplicationRole { Name = "Employee" },
                new ApplicationRole { Name = "Examiner" },
                new ApplicationRole { Name = "Manager" },
                new ApplicationRole { Name = "Student" }
            };
            foreach (var role in roleList)
            {
                if (await roleManager.CreateAsync(role) != IdentityResult.Success)
                {
                    throw new DbUpdateException();
                }
                logger.LogInformation($"建立{role.Name}角色成功");
                var claim = new Claim(ClaimTypes.Role, $"{role.Name}");
                if (await roleManager.AddClaimAsync(role, claim) != IdentityResult.Success)
                {
                    throw new DbUpdateException();
                }
                logger.LogInformation($"建立{role.Name}聲明成功");
            }
        }

        private static void CreateFolder(ILogger<DataSeeder> logger)
        {
            var paths = new List<string>
            {
                $"users{Path.DirectorySeparatorChar}photo",
                $"users{Path.DirectorySeparatorChar}verify{Path.DirectorySeparatorChar}national",
                $"users{Path.DirectorySeparatorChar}verify{Path.DirectorySeparatorChar}national",
                $"users{Path.DirectorySeparatorChar}verify{Path.DirectorySeparatorChar}student",
                $"resumes",
                $"companies{Path.DirectorySeparatorChar}logo",
                $"companies{Path.DirectorySeparatorChar}verify",
                $"activities{Path.DirectorySeparatorChar}event",
                $"activities{Path.DirectorySeparatorChar}campaign",
                $"activities{Path.DirectorySeparatorChar}general-campaign",
                $"activities{Path.DirectorySeparatorChar}recruitment-campaign",
            };
            
            foreach (var path in paths)
            {
                if (!Directory.Exists($"wwwroot{Path.DirectorySeparatorChar}{path}"))
                {
                    Directory.CreateDirectory($"wwwroot{Path.DirectorySeparatorChar}{path}");
                    logger.LogInformation($"{path} 建立成功");
                }
                else
                {
                    logger.LogInformation($"{path} 已經存在");
                }
            }
        }
    }
}