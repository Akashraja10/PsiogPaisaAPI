using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PsiogPaisaAPI.Models;

namespace PsiogPaisaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecentController : Controller
    {

        private readonly PsiogpaisaContext _dbcontext;

        public RecentController(PsiogpaisaContext psiogpaisaContext)
        {
            _dbcontext = psiogpaisaContext;
        }

       
        [HttpGet("{hi}")]
        public IActionResult GetIndividualTransaction(int hi)
        {
            var hello = (from ind in _dbcontext.Individuals
                         where hi == ind.SenderId
                         select new
                         {
                             
                             ind.RecieverId,
                             ind.Amount,
                             ind.Time,


                         }).ToList();

            return Ok(hello);
        }

       
       
    }
}
