using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PsiogPaisaAPI.Models;

namespace PsiogPaisaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly PsiogpaisaContext _dbcontext;

        public EmployeeController(PsiogpaisaContext psiogpaisaContext)
        {
            _dbcontext = psiogpaisaContext;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Employee employee)
        {
            if (employee == null)
            {
                return BadRequest();
            }
            var emp = await _dbcontext.Employees
                .FirstOrDefaultAsync(x => x.Username == employee.Username && x.Password == employee.Password);


            if (emp == null)
            {
                return NotFound(new { Message = "Employee Not Found !" });

            }

            emp.Token = CreateJwt(emp);

            return Ok(new
            {
                Token = emp.Token,
                Message = "Login Success"
            });
        }

        [HttpPost("admin")]
        public async Task<IActionResult> AdminLogin([FromBody] Employee employee)
        {
            if (employee == null)
            {
                return BadRequest();
            }
            var emp = await _dbcontext.Employees
                .FirstOrDefaultAsync(x => x.Username == "admin" && x.Password == "admin");


            if (emp == null)
            {
                return NotFound(new { Message = "Employee Not Found !" });

            }


            return Ok(emp);
        }

        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            var hello = (from ind in _dbcontext.Employees

                         select new
                         {
                             ind.EmpId,
                             ind.Username,
                             ind.EmpFname,
                             ind.Age,

                         }).ToList();

            return Ok(hello);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _dbcontext.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }
            _dbcontext.Employees.Remove(employee);
            await _dbcontext.SaveChangesAsync();
            return Ok(employee);
        }




        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Employee employee)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var user = new Employee
                {
                    Username = employee.Username,
                    Password = employee.Password,
                    EmpFname = employee.EmpFname,
                    EmpLname = employee.EmpLname,
                    Email = employee.Email,
                    Age = employee.Age,
                    Gender = employee.Gender,
                   
                };
                await _dbcontext.Employees.AddAsync(employee);
                await _dbcontext.SaveChangesAsync();

                
               EmailGeneration.Sendemail(user.Email, user.EmpFname, user.Username);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> GetPin(int id,[FromBody]Employee employee)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var ex = await _dbcontext.Employees.FindAsync(id);
                ex.Pin = employee.Pin;


                //_dbcontext.Entry(employee).State = EntityState.Modified;
                //await _dbcontext.Employees.AddAsync(employee);
                await _dbcontext.SaveChangesAsync();

                return Ok(ex);
            }
            catch (Exception exp)
            {
                return BadRequest(exp);
            }

        }


        private string CreateJwt(Employee emp)
        {
            var jwt = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("superSecretKey@345");
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.SerialNumber,$"{emp.EmpId}"),
                new Claim(ClaimTypes.Name, $"{emp.EmpFname}"),
                new Claim(JwtRegisteredClaimNames.Jti,$"{emp.Pin}")
             });
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescripter = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddMinutes(100),
                SigningCredentials = credentials,
            };
            var token = jwt.CreateToken(tokenDescripter);
            return jwt.WriteToken(token);

        }
        public class EmailGeneration
        {
            public static void Sendemail(string Email, string EmpFname, string Username)
            {
                var fromEmail = new MailAddress("customercrud123@gmail.com");
                var toEmail = new MailAddress(Email);
                var fromEmailPassword = "zkjvuorvpkkhmaem";

                string subject = "PSIOGPAISA SIGNUP";
                string body = "<br/><br/> " + "Hi " + EmpFname + ": " + "<br/>" +
                    "Your Username:" + Username + " for the employee registration has been added to the PsiogPaisa." + "<br/>" +                   
                     "Warm Regards" +"<br/>"+
                     "<img src=https://media.licdn.com/dms/image/C511BAQGHEv_vDRHNHg/company-background_10000/0/1552019596670?e=1676271600&v=beta&t=g7tpr4UHCaEMBjqEEgCyWepMbv7XBow7PsnCpeP4kEc />";
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("customercrud123@gmail.com", fromEmailPassword)

                };
                using (var message = new MailMessage(fromEmail, toEmail)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                }) 
                smtp.Send(message);
            }
        }

            [HttpPost("forgotPassword")]
        public async Task<IActionResult> Forgotpassword([FromBody] ForgotPassword forgotPassword)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var fp = new ForgotPassword
                {
                    Username = forgotPassword.Username,
                    Email = forgotPassword.Email,
                    ConfirmPassword = forgotPassword.ConfirmPassword,
                };


                var login = _dbcontext.Employees.FirstOrDefault(e => e.Username == forgotPassword.Username && e.Email == forgotPassword.Email);
                login.Password = forgotPassword.ConfirmPassword;

                if (login == null)
                {
                    return NotFound(new { Message = "Employee Not Found !" });

                }

                await _dbcontext.SaveChangesAsync();
                _dbcontext.Employees.Update(login);

                return Ok(fp);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        //get all employees except specified id
        [HttpGet("{empid}")]
        public async Task<IActionResult> GetEmployees(int empid)
        {
            var employee = await _dbcontext.Employees.Where(e => e.EmpId != empid).Select(e => new
            {
                e.EmpId,
                e.EmpFname,
            })

        .ToListAsync();

            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        [HttpPost("contact")]
        public async Task<IActionResult> ContactForm([FromBody] Contact cont)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var contacts = new Contact
                {
                    Name = cont.Name,
                    Email = cont.Email,

                };

                await _dbcontext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }


        }

        [HttpGet("my/{roles}")]
     public async Task<IActionResult> GetAllDetails(string roles)
    {
          
            var end = await _dbcontext.Employees.Where(e => e.Username == roles).Select(e => new
            {
                empid=e.EmpId,
                emplfname=e.EmpFname,
            }).ToListAsync();

            if (end == null)
            {
                return NotFound();
            }
            return Ok();
        }
   



            /*        //employees
                    [HttpGet("empid/{empid}")]
                    public async Task<IActionResult> GetEmployee(int empid)
                    {
                        var employee = await _authContext.Employees.FirstOrDefaultAsync(x => x.EmpId == empid);

                        if (employee == null)
                        {
                            return NotFound();
                        }
                        return Ok(employee);
                    }*/





            /*        //all employees
                    [HttpGet]
                    public async Task<IActionResult> GetAllEmployees()
                    {
                        var employees = await _authContext.Employees.ToListAsync();

                        return Ok(employees);
                    }*/







            /*
                    [HttpGet("id")]
                    public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees(int EmpId)
                    {

                        var employees = await _authContext.Employees

                            .Where(a=>a.EmpId==EmpId).ToListAsync();
                        return Ok(employees);
                    }
            */


            /*        [HttpGet]
                    public async Task<IActionResult> GetAllEmployees(Employee employee)
                    {
                        return await _authContext.Employees

                      .Select(x => ItemToDTO(x))
                      .Where (x => x.EmpId != employee.EmpId)
                      .ToListAsync();



                        return Ok(employees);
                    }*/


        }
}

/*var user = (from e in _authContext.Employees
            where e.EmpId != employee.EmpId
            select e).FirstOrDefaultAsync();*/


/*            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokeOptions = new JwtSecurityToken(
                issuer: "https://localhost:7290",
                audience: "https://localhost:7290",
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signinCredentials
            );         
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, $"{employee.EmpFname}{employee.EmpLname}")
            });
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return Ok(new AuthenticatedResponse { Token = tokenString});*/