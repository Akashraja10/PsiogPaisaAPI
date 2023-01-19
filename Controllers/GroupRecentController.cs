using Microsoft.AspNetCore.Mvc;
using PsiogPaisaAPI.Models;

namespace PsiogPaisaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupRecentController : Controller
    {
        private readonly PsiogpaisaContext _dbcontext;

        public GroupRecentController(PsiogpaisaContext psiogpaisaContext)
        {
            _dbcontext = psiogpaisaContext;
        }

        [HttpGet("{id}")]
        public IActionResult GetGroupTransaction(int id)
        {
            var hello = (from ind in _dbcontext.Groups
                         where id == ind.ContributorId
                         select new
                         {
                             ind.GroupId,
                             ind.ReqId,                         
                             ind.Amount,

                         }).ToList();

            return Ok(hello);
        }
    }
}
