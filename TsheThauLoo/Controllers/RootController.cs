using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace TsheThauLoo.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api")]
    public class RootController : ControllerBase
    {
        private readonly ILogger<RootController> _logger;

        public RootController(ILogger<RootController> logger)
        {
            _logger = logger;
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
            }

            return  Problem(title: title, detail: detail);
        } 
    }
}