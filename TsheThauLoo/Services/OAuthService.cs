using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TsheThauLoo.Data;
using TsheThauLoo.Dtos.Account.Login;
using TsheThauLoo.Entities.User;
using TsheThauLoo.Services.Interface;

namespace TsheThauLoo.Services
{
    public class OAuthService : IOAuthService
    {
        private readonly ILogger<OAuthService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;
        private readonly TsheThauLooDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public OAuthService(
            ILogger<OAuthService> logger, 
            IConfiguration configuration, 
            IHttpClientFactory clientFactory, 
            TsheThauLooDbContext dbContext, 
            IMapper mapper, 
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _configuration = configuration;
            _clientFactory = clientFactory;
            _dbContext = dbContext;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<NIDUserInfoDto> GetNIDUserInfoAsync(string userCode)
        {
            var url = $"fcuapi/api/GetUserInfo?" +
                      $"client_id={_configuration["NIDSettings:ClientId"]}&" +
                      $"user_code={userCode}";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var client = _clientFactory.CreateClient("nid");

            _logger.LogInformation($"NID 登入開始: {url}");

            try
            {
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode(); // 不是 200 會 HttpRequestException
                
                var responseStream = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<UserInfoDto>(responseStream);
                
                var dto = result.UserInfo.First(); // NullReferenceException
                _logger.LogInformation($"NID 登入成功: {dto.Id},{dto.Type},{dto.DeptName},{dto.UnitName},{dto.ClassName}");
                return dto;
            }
            catch (HttpRequestException e)
            {
                _logger.LogInformation($"NID 登入失敗{e}");
                throw;
            }
            catch (NullReferenceException e)
            {
                _logger.LogInformation($"NID 登入解析失敗: {e}");
                throw;
            }
        }
        
        public async Task<ApplicationUser> HandleNIDLoginAsync(NIDUserInfoDto info)
        {
            switch (info.Type)
            {
                case "學生":
                {
                    var user = await _dbContext.Users
                        .AsNoTracking()
                        .Include(x => x.Student)
                        .FirstOrDefaultAsync(x => x.Student.NetworkId == info.Id);
                    if (user != null)
                    {
                        return user;
                    }
                    
                    #region 註冊

                    var newUser = _mapper.Map<ApplicationUser>(info);

                    await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            #region 建立使用者

                            if (await _userManager.CreateAsync(newUser) != IdentityResult.Success)
                            {
                                throw new DbUpdateException();
                            }

                            #endregion
                            
                            #region 添加 Claim

                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.NameIdentifier, newUser.Id),
                                new Claim(ClaimTypes.Name, newUser.UserName),
                                new Claim(ClaimTypes.Email, newUser.Email),
                                new Claim(ClaimTypes.Sid, newUser.SecurityStamp)
                            };

                            if (await _userManager.AddClaimsAsync(newUser, claims) != IdentityResult.Success)
                            {
                                throw new DbUpdateException();
                            }

                            #endregion
                            
                            #region 添加角色

                            if (await _userManager.AddToRoleAsync(newUser, "Student") != IdentityResult.Success)
                            {
                                throw new DbUpdateException();
                            }

                            #endregion
                            
                            await transaction.CommitAsync();
                        }
                        catch (DbUpdateException)
                        {
                            await transaction.RollbackAsync();
                            throw;
                        }
                    }

                    return newUser;

                    #endregion
                }
                case "教職員工":
                {
                    var user = await _dbContext.Users
                        .AsNoTracking()
                        .Include(x => x.Administrator)
                        .Include(x => x.Employee)
                        .FirstOrDefaultAsync(x =>
                            x.Administrator.NetworkId == info.Id || x.Employee.NetworkId == info.Id.ToUpper());
                    if (user != null)
                    {
                        return user;
                    }

                    #region 註冊

                    var newUser = _mapper.Map<ApplicationUser>(info);

                    await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            #region 建立使用者

                            if (await _userManager.CreateAsync(newUser) != IdentityResult.Success)
                            {
                                throw new DbUpdateException();
                            }

                            #endregion
                            
                            #region 添加 Claim

                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.NameIdentifier, newUser.Id),
                                new Claim(ClaimTypes.Name, newUser.UserName),
                                new Claim(ClaimTypes.Email, newUser.Email),
                                new Claim(ClaimTypes.Sid, newUser.SecurityStamp)
                            };

                            if (await _userManager.AddClaimsAsync(newUser, claims) != IdentityResult.Success)
                            {
                                throw new DbUpdateException();
                            }

                            #endregion
                            
                            #region 添加角色

                            if (await _userManager.AddToRoleAsync(newUser, "Employee") != IdentityResult.Success)
                            {
                                throw new DbUpdateException();
                            }

                            #endregion
                            
                            await transaction.CommitAsync();
                        }
                        catch (DbUpdateException)
                        {
                            await transaction.RollbackAsync();
                            throw;
                        }
                    }

                    return newUser;

                    #endregion
                }
                default:
                    throw new ArgumentException();
            }
        }
        
        private class UserInfoDto
        {
            [BindProperty(Name = "UserInfo")]
            [JsonPropertyName("UserInfo")]
            public List<NIDUserInfoDto> UserInfo { get; set; }
        }
    }
}