
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PsiogPaisaAPI.Models;

namespace PsiogPaisaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : Controller
    {
        private readonly PsiogpaisaContext _dbcontext;

        public GroupController(PsiogpaisaContext psiogpaisaContext)
        {
            _dbcontext = psiogpaisaContext;
        }

        [HttpPost("request")]
        public async Task<IActionResult> CreateRequest([FromBody] Request req)
        {

            if (req == null || req.QuotedAmount <= 0)
            {
                return BadRequest();
            }
            var request = new Request
            {
                EmpId = req.EmpId,
                Purpose = req.Purpose,
                QuotedAmount = req.QuotedAmount,
                RecievedAmount = 0,
                StatusId = 1,
            };

            await _dbcontext.Requests.AddAsync(request);
            await _dbcontext.SaveChangesAsync();
            return Ok(req);

        }

        [HttpGet("{Id}")]
          public IActionResult ShowRequest(int Id)
          {
            var wal4 = (from req in _dbcontext.Requests
                        join ep in _dbcontext.Employees on req.EmpId equals ep.EmpId 
                        where Id != req.EmpId && req.QuotedAmount!=req.RecievedAmount && req.QuotedAmount >=req.RecievedAmount
                        select new
                        {
                            requestID=req.ReqId,
                            name= ep.EmpFname,
                            reason=req.Purpose,
                            quotedAmount = req.QuotedAmount,
                            recievedAmount=req.RecievedAmount

                        }).ToList();


            return Ok(wal4);
          }


        [HttpPost("group")]

        public async Task<IActionResult> CreateGroup([FromBody] Group group)
        {
            if (group == null || group.Amount <= 0 )
            {
                return BadRequest();
            }

            var grp = new Group
            {
                ReqId = group.ReqId,
                ContributorId = group.ContributorId,
                Amount = group.Amount,
                StatusId = 1,
                TypeId =1,
            };

            await _dbcontext.Groups.AddAsync(grp);
            await _dbcontext.SaveChangesAsync();

            //update request table
            var req = _dbcontext.Requests.FirstOrDefault(e => e.ReqId == group.ReqId);
            req.RecievedAmount = req.RecievedAmount + group.Amount;

            if (req == null)
            {
                return BadRequest();
            }


            //update contributor wallet
            var wal1 = _dbcontext.SelfWallet.FirstOrDefault(e => e.EmpId == group.ContributorId);
           
            if (wal1 == null )
            {
                return NotFound();
            }
            wal1.WalletAmount = wal1.WalletAmount - group.Amount;

            if (wal1.WalletAmount <= 0)
            {
                return NotFound();
            }


            //update receiver wallet          


            var wal4 = (from gp in _dbcontext.Groups
                        join rq in _dbcontext.Requests on gp.ReqId equals rq.ReqId
                        join sw in _dbcontext.SelfWallet on rq.EmpId equals sw.EmpId
                        select new
                        {
                            emid = rq.EmpId,
                            walam = sw.WalletAmount

                        }).ToList();

            double? tfamount = wal4[1].walam + group.Amount;

            /*
                        var wal3 = _authContext.SelfWallet

                            .Join(_authContext.Requests,
                            p => p.EmpId,
                            c => c.EmpId,
                        (p, c) => new { SelfWallet = p, Requests = c }).Where(f => f.Requests.ReqId == group.ReqId).ToList();*/

            if (wal4 == null)
            {
                return NotFound();
            }

            _dbcontext.Requests.Update(req);
            _dbcontext.SelfWallet.Update(wal1);

            var swl = _dbcontext.SelfWallet.Find(wal4[1].emid);
            _dbcontext.Entry(swl).State = EntityState.Unchanged;
            swl.WalletAmount = tfamount;

            _dbcontext.SaveChanges();
           return Ok();
        


        }




        //request
        [HttpGet("ReqId")]
        public async Task<IActionResult> GetAccount(int ReqId)
        {
            var request = await _dbcontext.Requests.FirstOrDefaultAsync(x => x.ReqId == ReqId);

            if (request == null)
            {
                return NotFound();
            }
            return Ok(request);

        }
    }
}
