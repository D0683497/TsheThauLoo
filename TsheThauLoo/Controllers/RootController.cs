using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TsheThauLoo.Data;
using TsheThauLoo.Dtos;

namespace TsheThauLoo.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api")]
    public class RootController : ControllerBase
    {
        private readonly ILogger<RootController> _logger;
        private readonly IMapper _mapper;
        private readonly TsheThauLooDbContext _dbContext;

        public RootController(ILogger<RootController> logger, IMapper mapper, TsheThauLooDbContext dbContext)
        {
            _logger = logger;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        [AllowAnonymous]
        [HttpGet("about", Name = nameof(About))]
        public async Task<ActionResult<IEnumerable<AboutDto>>> About()
        {
            var entities = await _dbContext.Users
                .AsNoTracking()
                .Include(x => x.Administrator)
                .Include(x => x.Administrator.Responsibilities)
                .Where(x => x.IsEnable && x.Administrator.AdministratorConfirmed && x.Administrator.ShowAbout)
                .ToListAsync();
            var dtos = _mapper.Map<IEnumerable<AboutDto>>(entities);
            return Ok(dtos);
        }
        
        [AllowAnonymous]
        [Route("error")]
        public IActionResult Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context.Error;
            var title = "發生未知錯誤";
            var detail = "請聯絡管理員";

            switch (exception)
            {
                case DbUpdateException:
                    title = "資料庫存取錯誤";
                    detail = "請稍後再試，若持續出現此情況，請聯絡管理員";
                    break;
                case IOException:
                    title = "檔案存取錯誤";
                    detail = "請稍後再試，若持續出現此情況，請聯絡管理員";
                    break;
                case SmtpCommandException commandException:
                    title = "電子郵件服務發生錯誤";
                    detail = commandException.ErrorCode switch
                    {
                        SmtpErrorCode.RecipientNotAccepted => "請檢查收件人電子郵件是否填寫正確",
                        SmtpErrorCode.SenderNotAccepted => "請聯絡管理員",
                        SmtpErrorCode.MessageNotAccepted => "郵件未被接受，請稍後再試，若持續出現此情況，請聯絡管理員",
                        SmtpErrorCode.UnexpectedStatusCode => "請稍後再試，若持續出現此情況，請聯絡管理員",
                        _ => detail
                    };
                    break;
            }

            return  Problem(title: title, detail: detail);
        } 
    }
}