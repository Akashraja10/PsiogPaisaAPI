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
           
                if (individual == null || individual.Amount <= 0 )
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


                var wal = _dbcontext.SelfWallets.FirstOrDefault(e => e.EmpId == individual.SenderId);

                if (wal == null)
                {
                    return NotFound();
                }

              wal.WalletAmount = wal.WalletAmount - individual.Amount;

            if (wal.WalletAmount <= 0)
            {
                return NotFound();
            }
            _dbcontext.SelfWallets.Update(wal);
                

                
                var wal2 = _dbcontext.SelfWallets.FirstOrDefault(e => e.EmpId == individual.RecieverId);
                wal2.WalletAmount = wal2.WalletAmount + individual.Amount;

                if (wal2 == null)
                {
                    return NotFound();
                }
                _dbcontext.SelfWallets.Update(wal2);
                _dbcontext.SaveChanges();

            
            return Ok(ind);

        }
        //wallet 
        [HttpGet("WalId")]

        public async Task<IActionResult> GetWallet(int WalId)
        {
            var wallet = await _dbcontext.SelfWallets.FirstOrDefaultAsync(x => x.WalId == WalId);

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

        [HttpPost("{Id}")]
        public async Task<IActionResult> MatchPin(int Id, [FromBody] Employee employee)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var ex = await _dbcontext.Employees.FindAsync(Id);

                if (ex.Pin == employee.Pin)
                {
                    //await _dbcontext.SaveChangesAsync();
                    return Ok(ex);
                };
                return BadRequest(ex);

                //_dbcontext.Entry(employee).State = EntityState.Modified;
                //await _dbcontext.Employees.AddAsync(employee);
            }
            catch (Exception exp)
            {
                return BadRequest(exp);
            }

        }


    }
}
