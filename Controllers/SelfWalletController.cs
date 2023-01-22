using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PsiogPaisaAPI.Models;

namespace PsiogPaisaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SelfWalletController : Controller
    {
        private readonly PsiogpaisaContext _dbcontext;

        public SelfWalletController(PsiogpaisaContext psiogpaisaContext)
        {
            _dbcontext = psiogpaisaContext;
        }

        [HttpGet("{empid}")]
        public IActionResult GetWalletAndMasterAcc(int empid)
        {
            var wal4 = (from wal in _dbcontext.SelfWallet
                        join ma in _dbcontext.MasterAccounts on wal.EmpId equals ma.EmployeeEmpId
                        where empid== wal.EmpId 
                        select new
                        {
                            ma.TotalAmount,
                            wal.WalletAmount

                        }).ToList();

            if (wal4 == null)
            {
                return NotFound();
            }
            return Ok(wal4);
        }


        //self-wallet
        [HttpGet("WalId")]
        public async Task<IActionResult> GetWallet(int WalId)
        {
            var wal = await _dbcontext.SelfWallet.FirstOrDefaultAsync(x => x.WalId == WalId);

            if (wal == null)
            {
                return NotFound();
            }
            return Ok(wal);

        }


        //master account
        [HttpGet("MasterId")]
        public async Task<IActionResult> GetAccount(int MasterId)
        {
            var mas = await _dbcontext.MasterAccounts.FirstOrDefaultAsync(x => x.MasterId == MasterId);

            if (mas == null)
            {
                return NotFound();
            }
            return Ok(mas);

        }

        //update self wallet
        [HttpPut("{empID}")]
        public async Task<IActionResult>UpdateAcc(int empID, SelfWallet selfWallet)
        {
            var self = await _dbcontext.SelfWallet.FindAsync(empID);
            var mas= await _dbcontext.MasterAccounts.FirstOrDefaultAsync(e=>e.EmployeeEmpId== empID);

            if(self == null)
            {
                return BadRequest();
            }
          
            //self.EmpId = selfWallet.EmpId;
            self.WalletAmount = self.WalletAmount+ selfWallet.WalletAmount;
            mas.TotalAmount= (double)(mas.TotalAmount-selfWallet.WalletAmount);

            if (mas.TotalAmount <= 0)
            {
                return BadRequest();
            }
/*
            if (wal.WalletAmount <= 0)
            {
                return NotFound();
            }*/

            _dbcontext.SelfWallet.Update(self);
            _dbcontext.MasterAccounts.Update(mas);

            _dbcontext.SaveChanges();
            return Ok(self);

        }



/*
        //employee
        [HttpGet("EmpId")]
        public async Task<IActionResult> GetEmployee(int EmpId)
        {
            var emp = await _authContext.Employees.FirstOrDefaultAsync(x => x.EmpId == EmpId);

            if (emp == null)
            {
                return NotFound();
            }
            return Ok(emp);

        }*/
    }
}
