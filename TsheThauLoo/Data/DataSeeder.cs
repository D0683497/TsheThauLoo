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
                    
                    logger.LogInformation("開始創建使用者");
                    await CreateUserAsync(services, logger);
                    logger.LogInformation("創建使用者完成");
                }
                else
                {
                    logger.LogInformation("資料庫已存在");
                }
                
                logger.LogInformation("開始創建資料夾");
                CreateFolder(logger);
            }
        }

        private static async Task CreateUserAsync(IServiceProvider services, ILogger<DataSeeder> logger)
        {
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            #region Administrator

            for (int i = 1; i <= 10; i++)
            {
                var user = new ApplicationUser
                {
                    UserName = $"Administrator{i}",
                    Email = $"Administrator{i}@Administrator.com",
                    EmailConfirmed = true,
                    IsEnable = true,
                    Name = $"Administrator{i}",
                    NationalVerify = new NationalVerify(),
                    Administrator = new Administrator
                    {
                        AdministratorConfirmed = true,
                        NetworkId = $"NetworkId{i}",
                        Dept = $"Dept{i}",
                    }
                };
                
                #region 建立使用者

                if (await userManager.CreateAsync(user, $"Administrator{i}") != IdentityResult.Success)
                {
                    throw new DbUpdateException();
                }

                #endregion
                
                #region 添加 Claim

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Sid, user.SecurityStamp)
                };

                if (await userManager.AddClaimsAsync(user, claims) != IdentityResult.Success)
                {
                    throw new DbUpdateException();
                }

                #endregion
                
                #region 添加角色

                if (await userManager.AddToRoleAsync(user, "Administrator") != IdentityResult.Success)
                {
                    throw new DbUpdateException();
                }

                #endregion
            }

            #endregion

            #region Manager
            
            for (int i = 1; i <= 5; i++)
            {
                var user = new ApplicationUser
                {
                    UserName = $"Manager{i}",
                    Email = $"Manager{i}@Manager.com",
                    EmailConfirmed = true,
                    IsEnable = true,
                    Name = $"Manager{i}",
                    NationalVerify = new NationalVerify(),
                    Manager = new Manager
                    {
                        ManagerConfirmed = true,
                        DivisionName = $"DivisionName{i}",
                        JobTitle = $"JobTitle{i}",
                        ContactEmail = $"Manager{i}@Manager.com",
                        ContactPhone = "04-24517250",
                        ContactAddress = "台中市西屯區文華路100號",
                        Substitute = new Substitute
                        {
                            Name = $"Substitute{i}",
                            DivisionName = $"DivisionName{i}",
                            JobTitle = $"JobTitle{i}",
                            ContactEmail = $"Substitute{i}@Substitute.com",
                            ContactPhone = "04-24517250",
                            ContactAddress = "台中市西屯區文華路100號"
                        }
                    }
                };
                
                #region 建立使用者

                if (await userManager.CreateAsync(user, $"Manager{i}") != IdentityResult.Success)
                {
                    throw new DbUpdateException();
                }

                #endregion
                
                #region 添加 Claim

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Sid, user.SecurityStamp)
                };

                if (await userManager.AddClaimsAsync(user, claims) != IdentityResult.Success)
                {
                    throw new DbUpdateException();
                }

                #endregion
                
                #region 添加角色

                if (await userManager.AddToRoleAsync(user, "Manager") != IdentityResult.Success)
                {
                    throw new DbUpdateException();
                }

                #endregion
            }
            
            #endregion
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
                $"users{Path.DirectorySeparatorChar}verify{Path.DirectorySeparatorChar}alumnus",
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