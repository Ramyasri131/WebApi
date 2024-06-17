using EmployeeDirectory.BAL.Exceptions;
using EmployeeDirectory.BAL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeDirectory.Controllers
{
    [ApiController]
    [Route("/Role")]
    public class RoleController(IRoleProvider roleProvider) : Controller
    {
        private readonly IRoleProvider _roleProvider=roleProvider;

        [HttpGet]
        public async Task<ActionResult<List<BAL.DTO.Role>>> GetRoles()
        {
            try
            {
                return await _roleProvider.GetRoles();
            }
            catch (FormatException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddRole(BAL.DTO.Role role)
        {
            try
            {
                await _roleProvider.AddRole(role);
                return Ok();
            }
            catch (InvalidData ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
