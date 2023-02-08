using Microsoft.AspNetCore.Mvc;
using PsiogPaisaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace PsiogPaisaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExternalController : Controller
    {
        private readonly PsiogpaisaContext _dbcontext;

        public ExternalController(PsiogpaisaContext psiogpaisaContext)
        {
            _dbcontext = psiogpaisaContext;
        }


        [HttpPost("external")]
        public async Task<IActionResult> TransactionExternal([FromBody] External external)
        {

            if (external == null || external.Amount <= 0 )
            {
                return BadRequest();
            }
            var ext = new External
            {
                Amount= external.Amount,
                Content= external.Content,
                TypeId = 1,
                EmpId=external.EmpId

            };

            await _dbcontext.Externals.AddAsync(ext);
            await _dbcontext.SaveChangesAsync();


            var wal = _dbcontext.SelfWallets.FirstOrDefault(e => e.EmpId == external.EmpId);

            if (wal == null)
            {
                return NotFound();
            }
            wal.WalletAmount = wal.WalletAmount - external.Amount;

            if (wal.WalletAmount <= 0)
            {
                return NotFound();
            }

            _dbcontext.SelfWallets.Update(wal);
            _dbcontext.SaveChanges();

            return Ok(ext);

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

        [HttpGet("{id}")]
        public IActionResult GetExternalTransaction(int id)
        {
            var hello = (from ind in _dbcontext.Externals
                         where id == ind.EmpId
                         select new
                         {
                             ind.ExtId,
                             ind.Content,
                             ind.Amount,



                         }).ToList();

            return Ok(hello);
        }


    }
}
