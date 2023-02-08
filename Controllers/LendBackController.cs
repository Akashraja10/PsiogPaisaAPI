using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PsiogPaisaAPI.Models;

namespace PsiogPaisaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LendBackController : Controller
    {
        private readonly PsiogpaisaContext _dbcontext;

        public LendBackController(PsiogpaisaContext psiogpaisaContext)
        {
            _dbcontext = psiogpaisaContext;
        }

        [HttpPost("lendback")]
        public async Task<IActionResult> CreateLendBack([FromBody] LendBack lendBack)
        {
            if (lendBack == null)
            {
                return BadRequest();
            }

            var lend = new LendBack
            {
                PaybackAmount = lendBack.PaybackAmount,
                GroupId = lendBack.GroupId,
                TypeId = 1,
            };

            await _dbcontext.LendBacks.AddAsync(lend);
            await _dbcontext.SaveChangesAsync();

   
            //update contributor wallet
            var wal1 = (from lb in _dbcontext.LendBacks
                        join gp in _dbcontext.Groups on lb.GroupId equals gp.GroupId
                        join sw in _dbcontext.SelfWallets on gp.ContributorId equals sw.EmpId
                        select new
                        {
                            gid = gp.ContributorId,
                            wa = sw.WalletAmount,
                        }).ToList();

            double? tfamount = wal1[1].wa + lendBack.PaybackAmount;

            var swl1 = _dbcontext.SelfWallets.Find(wal1[1].gid);

            _dbcontext.Entry(swl1).State = EntityState.Unchanged;
            swl1.WalletAmount = tfamount;

            //update lendback person wallet
            
            var wal2 = (from lb in _dbcontext.LendBacks
                        join gp in _dbcontext.Groups on lb.GroupId equals gp.GroupId
                        join req in _dbcontext.Requests on gp.ReqId equals req.ReqId
                        join sw in _dbcontext.SelfWallets on req.EmpId equals sw.EmpId
                        select new
                        {
                            rid = req.EmpId,
                            wam = sw.WalletAmount,
                        }).ToList();

            double? famount= wal2[1].wam - lendBack.PaybackAmount;

            var swl2 = _dbcontext.SelfWallets.Find(wal2[1].rid);
            if (famount <= 0)
            {
                return NotFound();
            }
            _dbcontext.Entry(swl2).State= EntityState.Unchanged;
            swl2.WalletAmount = famount;

            //update status id
            var status = _dbcontext.Groups.FirstOrDefault(e => e.GroupId == lendBack.GroupId);
            status.StatusId = 4;
            if (status == null)
            {
                return BadRequest();
            }



            _dbcontext.SaveChanges();
            return Ok();
        }



        [HttpGet("{id}")]
        public IActionResult GroupShow(int id)
        {
            var request = (from req in _dbcontext.Requests
                           join gp in _dbcontext.Groups on req.ReqId equals gp.ReqId
                           join emp in _dbcontext.Employees on gp.ContributorId equals emp.EmpId
                           where req.EmpId == id & gp.StatusId!=4
                           select new
                           {
                               gp.GroupId,
                               gp.ContributorId,
                               emp.EmpFname,
                               gp.Amount

                           }).ToList();

            return Ok(request);
        }
    }
}
