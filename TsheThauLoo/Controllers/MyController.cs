using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TsheThauLoo.Data;
using TsheThauLoo.Dtos.Company;
using TsheThauLoo.Utilities;

namespace TsheThauLoo.Controllers
{
    [ApiController]
    [AuthAuthorize]
    [Route("api/my")]
    public class MyController : ControllerBase
    {
        private readonly ILogger<MyController> _logger;
        private readonly TsheThauLooDbContext _dbContext;
        private readonly IMapper _mapper;

        public MyController(
            ILogger<MyController> logger, 
            TsheThauLooDbContext dbContext, 
            IMapper mapper)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [AuthAuthorize(Roles = "Manager")]
        [HttpGet("company", Name = nameof(MyCompany))]
        public async Task<ActionResult<CompanyDto>> MyCompany()
        {
            var userId = User.Claims
                .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var manager = await _dbContext.Managers
                .AsNoTracking()
                .Include(x => x.Company)
                .Include(x => x.Company.CompanyLogo)
                .Include(x => x.Company.IndustrialClassifications)
                .SingleOrDefaultAsync(x => x.ApplicationUserId == userId);
            if (manager.Company == null)
            {
                return NotFound();
            }
            var dto = _mapper.Map<CompanyDto>(manager.Company);
            return Ok(dto);
        }
    }
}