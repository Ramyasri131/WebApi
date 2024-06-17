using EmployeeDirectory.BAL.Interfaces;
using EmployeeDirectory.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeDirectory.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController(IConfiguration configuration,IEmployeeProvider employeeProvider) : Controller
    {
        private readonly IConfiguration _configuration=configuration;
        private readonly IEmployeeProvider _employeeProvider=employeeProvider;

        [HttpPost]
        public async Task<ActionResult<string>> Login([FromBody] LoginRequest loginRequest)
        {
            List<BAL.DTO.Employee> employees = await _employeeProvider.GetEmployeeDetails();
            //Find user/employee
            //Check 
            //Check for password
            var item = employees.FirstOrDefault(_ => _.Id.ToLower() == loginRequest.Id.ToLower());
            if (item == null)
            {
                return NotFound("User does not exists");
            }

            if (string.Equals(item.Id, loginRequest.Id) && string.Equals(item.password, loginRequest.Password))
            {
                List<Claim> claims = new List<Claim>()
                    {
                       new Claim("Email", item.Email!),
                       new Claim("Password",loginRequest.Password),
                       new Claim("Id", loginRequest.Id),
                       new Claim("role",item.JobTitle!)
                    };
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
                var jwtToken = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(1),
                    signingCredentials: credentials);
                var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
                return token;
            }
            return NotFound("User does not exists");
        }
    }
}
