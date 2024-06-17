using EmployeeDirectory.BAL.Exceptions;
using EmployeeDirectory.BAL.Extensions;
using EmployeeDirectory.DAL.Interfaces;
using EmployeeDirectory.BAL.Interfaces;
using System.Globalization;
using EmployeeDirectory.DAL.Models;

namespace EmployeeDirectory.BAL.Providers
{
    public class EmployeeProvider(IEmployeeRepository employeeRepository,ILocationRepository locationRepository,IDepartmentRepository departmentRepository,IManagerRepository managerRepository,IProjectRepository projectRepository,IRoleRepository roleRepository) : IEmployeeProvider
    {

        private readonly IEmployeeRepository _employeeRepository = employeeRepository;
        private readonly ILocationRepository _locationRepository = locationRepository;
        private readonly IDepartmentRepository _departmentRepository = departmentRepository;
        private readonly IManagerRepository _managerRepository = managerRepository;
        private readonly IProjectRepository _projectRepository = projectRepository;
        private readonly IRoleRepository _roleRepository = roleRepository;


        public async Task AddEmployee(DTO.Employee employee)
        {
            List<Employee>employees;
            employees = await _employeeRepository.GetAll();
            DAL.Models.Employee user=new Employee();
            if (employees.Any(emp=> string.Equals(emp.Email, employee.Email)))
            {
                throw new DuplicateData("Employee with mail exists");
            }
            int employeeCount = int.Parse(employees[^1].Id[2..]) + 1;
            string id = string.Format("{0:0000}", employeeCount);
            id = "TZ" + id;
            user.Id = id;
            List<Location> locations = await _locationRepository.GetAll();
            foreach (DAL.Models.Location location in locations)
            {
                user.Location = string.Equals(location.Name, employee.Location) ? location.Id : user.Location;
            }
            List<DAL.Models.Department> departments = await _departmentRepository.GetAll();
            foreach (DAL.Models.Department department in departments)
            {
                user.Department = string.Equals(department.Name, employee.Department) ? department.Id : user.Department;
            }
            List<Manager> managers = await _managerRepository.GetAll();
            foreach (Manager manager in managers)
            {
                user.Manager = string.Equals(manager.Name, employee.Manager) ? manager.Id : user.Manager;
            }
            List<Project> projects = await _projectRepository.GetAll();
            foreach (Project project in projects)
            {
                user.Project = string.Equals(project.Name, employee.Project) ? project.Id : user.Project;
            }
            List<Role> roles = await _roleRepository.GetAll();
            foreach (Role role in roles)
            {
                user.JobTitle = string.Equals(role.Name, employee.JobTitle) ? role.Id : user.JobTitle;
            }
            user.FirstName = employee.FirstName!;
            user.LastName = employee.LastName!;
            user.DateOfBirth = DateOnly.ParseExact(employee.DateOfBirth!, "yyyy-mm-dd", CultureInfo.InvariantCulture);
            user.MobileNumber = employee.MobileNumber;
            user.DateOfJoin = DateOnly.ParseExact(employee.DateOfJoin!, "yyyy-mm-dd", CultureInfo.InvariantCulture);
            user.Email = employee.Email!;
            user.password = employee.password;
            await _employeeRepository.Add(user);
        }

        public async Task<List<BAL.DTO.EmployeeDisplay>> GetEmployees()
        {
            List<Location> locations = await _locationRepository.GetAll();
            List<Department> departments = await _departmentRepository.GetAll();
            List<Role> roles= await _roleRepository.GetAll();
            List<Project> projects = await _projectRepository.GetAll();
            List<Manager> managers = await _managerRepository.GetAll();
            List<Employee> employeeData;
            employeeData = await _employeeRepository.GetAll();
            List<BAL.DTO.EmployeeDisplay> employees= new List<BAL.DTO.EmployeeDisplay>();
            foreach (DAL.Models.Employee employee in employeeData)
            {
                BAL.DTO.EmployeeDisplay employeeInput = new()
                {
                    Id = employee.Id,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    DateOfBirth = employee.DateOfBirth.ToString(),
                    Email = employee.Email,
                    MobileNumber = employee.MobileNumber,
                    DateOfJoin = employee.DateOfJoin.ToString(),
                    Location = locations[employee.Location - 1].Name,
                    JobTitle = roles[employee.JobTitle - 1].Name,
                    Department = departments[employee.Department - 1].Name,
                    Manager = managers[employee.Manager - 1].Name,
                    Project = projects[employee.Project - 1].Name,
                };
                employees.Add(employeeInput);
            }
            return employees;
        }

        public async Task<List<BAL.DTO.Employee>> GetEmployeeDetails()
        {
            List<Location> locations = await _locationRepository.GetAll();
            List<Department> departments = await _departmentRepository.GetAll();
            List<Role> roles = await _roleRepository.GetAll();
            List<Project> projects = await _projectRepository.GetAll();
            List<Manager> managers = await _managerRepository.GetAll();
            List<Employee> employeeData;
            employeeData = await _employeeRepository.GetAll();
            List<BAL.DTO.Employee> employees = new List<BAL.DTO.Employee>();
            foreach (DAL.Models.Employee employee in employeeData)
            {
                BAL.DTO.Employee employeeInput = new()
                {
                    Id = employee.Id,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    DateOfBirth = employee.DateOfBirth.ToString(),
                    Email = employee.Email,
                    MobileNumber = employee.MobileNumber,
                    DateOfJoin = employee.DateOfJoin.ToString(),
                    Location = locations[employee.Location - 1].Name,
                    JobTitle = roles[employee.JobTitle - 1].Name,
                    Department = departments[employee.Department - 1].Name,
                    Manager = managers[employee.Manager - 1].Name,
                    Project = projects[employee.Project - 1].Name,
                    password = employee.password
                };
                employees.Add(employeeInput);
            }
            return employees;
        }

        public async Task EditEmployee(BAL.DTO.Employee employeeInput, string id)
        {
            if (id.IsNullOrEmptyOrWhiteSpace())
            {
                throw new InvalidData("Enter Employee Id");
            }
            else
            {
                DAL.Models.Employee? employee = await _employeeRepository.GetById(id);
                if(employee is null)
                {
                    throw new RecordNotFound("Employee not found");
                }
                else
                {
                    List<Location> locations = await _locationRepository.GetAll();
                    List<Department> departments = await _departmentRepository.GetAll();
                    List<Role> roles = await _roleRepository.GetAll();
                    List<Project> projects = await _projectRepository.GetAll();
                    List<Manager> managers = await _managerRepository.GetAll();
                    foreach (Location location in locations)
                    {
                        employee.Location = string.Equals(location.Name, employee.Location) ? location.Id : employee.Location;
                    }
                    foreach (DAL.Models.Department department in departments)
                    {     
                        employee.Department = string.Equals(department.Name, employee.Department) ? department.Id : employee.Department;
                    }
                    foreach (Manager manager in managers)
                    {
                        employee.Manager = string.Equals(manager.Name, employee.Manager) ? manager.Id : employee.Manager;
                    }
                    foreach (Project project in projects)
                    {
                        employee.Project = string.Equals(project.Name, employee.Project) ? project.Id : employee.Project;
                    }
                    foreach (Role role in roles)
                    {
                        employee.JobTitle = string.Equals(role.Name, employee.JobTitle) ? role.Id : employee.JobTitle;
                    }
                    employee.FirstName = employeeInput.FirstName!;
                    employee.LastName = employeeInput.LastName!;
                    employee.DateOfBirth = DateOnly.Parse(employeeInput.DateOfBirth!);
                    employee.Email = employeeInput.Email!;
                    employee.MobileNumber = employeeInput.MobileNumber;
                    employee.DateOfJoin = DateOnly.Parse(employeeInput.DateOfJoin!);
                    await _employeeRepository.Update(employee);
                }
            }
        }

        public async Task DeleteEmployee(string? id)
        {
            if (id.IsNullOrEmptyOrWhiteSpace())
            {
                throw new InvalidData("Invalid Employee Id");
            }
            else
            {
                DAL.Models.Employee? employee = await _employeeRepository.GetById(id);
                if(employee is null)
                {
                    throw new RecordNotFound("Employee not found");
                }
                else
                {
                    await _employeeRepository.Delete(employee);
                }
            }
        }

        public async Task<BAL.DTO.Employee> GetEmployeeById(string? id)
        {
            if (id.IsNullOrEmptyOrWhiteSpace())
            {
                throw new InvalidData("Invalid Employee Id");
            }
            else
            {
                DAL.Models.Employee? employee = await _employeeRepository.GetById(id);
                if (employee is null)
                {
                    throw new RecordNotFound("Employee not found");
                }
                else
                {
                    List<Location> locations = await _locationRepository.GetAll();
                    List<Department> departments = await _departmentRepository.GetAll();
                    List<Role> roles = await _roleRepository.GetAll();
                    List<Project> projects = await _projectRepository.GetAll();
                    List<Manager> managers = await _managerRepository.GetAll();
                    BAL.DTO.Employee employeeInput = new()
                    {
                        Id = employee.Id,
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        DateOfBirth = employee.DateOfBirth.ToString(),
                        Email = employee.Email,
                        MobileNumber = employee.MobileNumber,
                        DateOfJoin = employee.DateOfJoin.ToString(),
                        Location = locations[employee.Location - 1].Name,
                        JobTitle = roles[employee.JobTitle - 1].Name,
                        Department = departments[employee.Department - 1].Name,
                        Manager = managers[employee.Manager - 1].Name,
                        Project = projects[employee.Project - 1].Name,
                    };
                    return employeeInput;
                }
            }
        }
    }
}