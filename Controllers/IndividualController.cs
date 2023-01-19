using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PsiogPaisaAPI.Models;

namespace PsiogPaisaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndividualController : Controller
    {
        private readonly PsiogpaisaContext _dbcontext;

        public IndividualController(PsiogpaisaContext psiogpaisaContext)
        {
            _dbcontext = psiogpaisaContext;
        }


        [HttpPost("individual")]
        public async Task<IActionResult> TransactionIndividual([FromBody] Individual individual)
        {
           
                if (individual == null || individual.Amount <= 0 || individual.Amount >=1000)
                {
                    return BadRequest();
                }
                var ind = new Individual {

                    SenderId = individual.SenderId,
                    RecieverId = individual.RecieverId,
                    TypeId = 1,
                    Amount = individual.Amount,
                    Time = DateTime.Now,
                    StatusId = 1,

                };

                await _dbcontext.Individuals.AddAsync(ind);
                await _dbcontext.SaveChangesAsync();


                var wal = _dbcontext.SelfWallet.FirstOrDefault(e => e.EmpId == individual.SenderId);
                wal.WalletAmount= wal.WalletAmount-individual.Amount;

                if (wal == null)
                {
                    return NotFound();
                }
                _dbcontext.SelfWallet.Update(wal);
                

                
                var wal2 = _dbcontext.SelfWallet.FirstOrDefault(e => e.EmpId == individual.RecieverId);
                wal2.WalletAmount = wal2.WalletAmount + individual.Amount;

                if (wal2 == null)
                {
                    return NotFound();
                }
                _dbcontext.SelfWallet.Update(wal2);
                _dbcontext.SaveChanges();

            
            return Ok(ind);

        }
        //wallet 
        [HttpGet("WalId")]

        public async Task<IActionResult> GetWallet(int WalId)
        {
            var wallet = await _dbcontext.SelfWallet.FirstOrDefaultAsync(x => x.WalId == WalId);

            if (wallet == null)
            {
                return NotFound();
            }
            return Ok(wallet);

        }

        //show the amount and time of individual transaction 
        [HttpGet("{senderId}")]

        public  IActionResult GetChart(int senderId)
        {
            var chart = (from ind in _dbcontext.Individuals
                        join ma in _dbcontext.Employees on ind.SenderId equals ma.EmpId
                        where senderId == ind.SenderId
                         select new
                        {
                            ind.Time,
                            ind.Amount

                        }).ToList();

            return Ok(chart);
        }
       
      

    }
}
