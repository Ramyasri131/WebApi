using EmployeeDirectory.BAL.DTO;
using EmployeeDirectory.BAL.Exceptions;
using EmployeeDirectory.BAL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmployeeDirectory.Controllers
{
    [ApiController]
    [Route("/Employee")]
    [Authorize]
    public class EmployeeController(IEmployeeProvider employeeProvider,IEmployeeValidator employeeValidator) : Controller
    {
        private readonly IEmployeeProvider _employeeProvider = employeeProvider;
        private readonly IEmployeeValidator _employeeValidator = employeeValidator;

        [HttpGet]
        public async Task<ActionResult<List<BAL.DTO.EmployeeDisplay>>> GetEmployees()
        {
            try
            {
                return await _employeeProvider.GetEmployees();
            }
            catch(FormatException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployeeById(string id)
        {
            try
            {
                return await _employeeProvider.GetEmployeeById(id);
            }
            catch (RecordNotFound ex)
            {
                return NotFound(ex.Message);
            }
            catch(InvalidData ex)
            {
                return BadRequest(ex.Message);
            }
            catch (FormatException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddEmployee([FromBody] Employee employee)
        {
            //await _employeeValidator.ValidateDetails(employee);
            try
            {
                employee.password = "12345";
                await _employeeProvider.AddEmployee(employee);
                return Ok();
            }
            catch (InvalidData ex)
            {
                return BadRequest(ex.Message);
            }
            catch (FormatException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DuplicateData ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateEmployee(Employee employee)
        {
            string id = User.FindFirstValue("Id")!;
            await _employeeValidator.ValidateDetails(employee);
            try
            {
                await _employeeProvider.EditEmployee(employee, id);
                return Ok();
            }
            catch (InvalidData ex)
            {
                return BadRequest(ex.Message);
            }
            catch (FormatException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DuplicateData ex)
            {
                return BadRequest(ex.Message);
            }
        }
    

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(string id)
        {
            try
            {
                await _employeeProvider.DeleteEmployee(id);
                return Ok();
            }
            catch(InvalidData ex)
            {
                return BadRequest(ex.Message);
            }
            catch(RecordNotFound ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
