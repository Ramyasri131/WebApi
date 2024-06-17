using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using EmployeeDirectory.BAL.Exceptions;
using EmployeeDirectory.DAL.Interfaces;
using EmployeeDirectory.BAL.Interfaces;

namespace EmployeeDirectory.BAL.Validators
{
    public class EmployeeValidator(ILocationRepository locationRepository, IRoleRepository roleRepository, IDepartmentRepository departmentRepository, IManagerRepository managerRepository, IProjectRepository projectRepository) : IEmployeeValidator
    {
        private readonly ILocationRepository _locationRepository = locationRepository;
        private readonly IRoleRepository _roleRepository = roleRepository;
        private readonly IDepartmentRepository _departmentRepository = departmentRepository;
        private readonly IManagerRepository _managerRepository = managerRepository;
        private readonly IProjectRepository _projectRepository = projectRepository;

        public async Task ValidateDetails(DTO.Employee employee)
        {
            string? option;
            foreach (PropertyInfo propertyInfo in employee.GetType().GetProperties())
            {
                string? input = propertyInfo.GetValue(employee)?.ToString();
                if (propertyInfo.Name != "Id")
                {
                    switch (propertyInfo.Name)
                    {
                        case "DateOfBirth":
                            DateTime today = DateTime.Now;
                            if (!DateTime.TryParseExact(employee.DateOfBirth, new string[] { "dd/MM/yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                            {
                                throw new InvalidData("date of Birth");
                            }
                            else
                            {
                                int age = today.Year - DateTime.ParseExact(employee.DateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture).Year;
                                if (age < 18)
                                {
                                    throw new InvalidData("date of Birth");
                                }
                            }
                            break;
                        case "DateOfJoin":
                            if (!DateTime.TryParseExact(employee.DateOfJoin, new string[] { "dd/MM/yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                            {
                                throw new InvalidData("date of Join");
                            }
                            break;
                        case "Email":
                            Regex formatOfEmail = new Regex("^[a-zA-Z0-9._%+-]+@tezo.com$");
                            if (!formatOfEmail.IsMatch(employee.Email!))
                            {
                                throw new InvalidData("Email");
                            }
                            break;
                        case "MobileNumber":
                            if (employee.MobileNumber.ToString()!.Length != 10)
                            {
                                throw new InvalidData("Mobile Number");
                            }
                            break;
                        case "Location":
                            option = employee.Location;
                            List<DAL.Models.Location> locations = await _locationRepository.GetAll();
                            if (!locations.Any(location => string.Equals(location.Name, option)))
                            {
                                throw new InvalidData("Location");
                            }
                            break;
                        case "Department":
                            option = employee.Department;
                            List<DAL.Models.Department> departments = await _departmentRepository.GetAll();
                            if (!departments.Any(department => string.Equals(department.Name, option)))
                            {
                                throw new InvalidData("Department");
                            }
                            break;
                        case "Manager":
                            option = employee.Manager;
                            List<DAL.Models.Manager> managers = await _managerRepository.GetAll();
                            if (!managers.Any(manager => string.Equals(manager.Name, option)))
                            {
                                throw new InvalidData("Manager");
                            }
                            break;
                        case "Project":
                            option = employee.Project;
                            List<DAL.Models.Project> projects = await _projectRepository.GetAll();
                            if (!projects.Any(project => string.Equals(project.Name, option)))
                            {
                                throw new InvalidData("Project");
                            }
                            break;
                        case "JobTitle":
                            option = employee.JobTitle;
                            List<DAL.Models.Role> roles = await _roleRepository.GetAll();
                            if (!roles.Any(role => string.Equals(role.Name, option)))
                            {
                                throw new InvalidData("Role");
                            }
                            break;
                    }
                }
            }
        }
    }
}