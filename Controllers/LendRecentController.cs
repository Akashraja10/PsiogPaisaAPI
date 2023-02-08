using Microsoft.AspNetCore.Mvc;
using PsiogPaisaAPI.Models;

namespace PsiogPaisaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LendRecentController : Controller
    {
        private readonly PsiogpaisaContext _dbcontext;

        public LendRecentController(PsiogpaisaContext psiogpaisaContext)
        {
            _dbcontext = psiogpaisaContext;
        }


        [HttpGet("{id}")]
        public IActionResult LendShow(int id)
        {
            var lend = (from grp in _dbcontext.Groups
                           join lnd in _dbcontext.LendBacks on grp.GroupId equals lnd.GroupId
                        join req in _dbcontext.Requests on grp.ReqId equals req.ReqId
                        join emps in _dbcontext.Employees on req.EmpId equals emps.EmpId
                        join emp in _dbcontext.Employees on grp.ContributorId equals emp.EmpId
                        where emps.EmpId == id 
                           select new
                           {
                               lnd.LendId,
                               lnd.GroupId,                              
                               emp.EmpFname,
                               lnd.PaybackAmount

                           }).ToList();

            return Ok(lend);
        }

    }
}
