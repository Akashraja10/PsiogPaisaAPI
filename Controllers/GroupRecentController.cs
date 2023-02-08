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
            var hello = (from grp in _dbcontext.Groups
                         join req in _dbcontext.Requests on grp.ReqId equals req.ReqId
                         join emp in _dbcontext.Employees on req.EmpId equals emp.EmpId
                         where id == grp.ContributorId
                         select new
                         {
                             emp.EmpFname,
                             grp.GroupId,
                             grp.ReqId,
                             grp.Amount,

                         }).ToList();

            return Ok(hello);
        }
    }
}
