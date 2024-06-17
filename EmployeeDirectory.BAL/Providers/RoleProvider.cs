using EmployeeDirectory.BAL.Exceptions;
using EmployeeDirectory.BAL.Extensions;
using EmployeeDirectory.DAL.Interfaces;
using EmployeeDirectory.BAL.Interfaces;
using EmployeeDirectory.DAL.Repository;
using EmployeeDirectory.DAL.Models;


namespace EmployeeDirectory.BAL.Providers
{
    public class RoleProvider(IRoleRepository roleRepository,ILocationRepository locationRepository,IDepartmentRepository departmentRepository) : IRoleProvider
    {
        public static Dictionary<int, string> Roles = new();
        private readonly IRoleRepository _roleRepository = roleRepository;
        private readonly ILocationRepository _locationRepository = locationRepository;  
        private readonly IDepartmentRepository _departmentRepository = departmentRepository;

        public async Task AddRole(DTO.Role roleInput)
        {
            if(roleInput.Name.IsNullOrEmptyOrWhiteSpace())
            {
                throw new InvalidData("Enter Role Name");
            }
            List<DAL.Models.Role> RoleData= await  _roleRepository.GetAll();
            if (RoleData.Any(role => string.Equals(role.Name,roleInput.Name)))
            {
                throw new InvalidData("Role Exists");
            }
            DAL.Models.Role user = new();
            List<DAL.Models.Location> locations = await _locationRepository.GetAll();
            foreach (DAL.Models.Location location in locations)
            {
                user.Location = string.Equals(location.Name, roleInput.Location) ? location.Id : user.Location;
            }
            if (user.Location == 0)
            {
                throw new InvalidData("Location does not exists");
            }
            List<DAL.Models.Department> departments = await _departmentRepository.GetAll();
            foreach (DAL.Models.Department department in departments)
            {
                user.Department = string.Equals(department.Name, roleInput.Department) ? department.Id : user.Department;
            }
            if (user.Department == 0)
            {
                throw new InvalidData("Department does not exists");
            }
            user.Name = roleInput.Name!;
            user.Description = roleInput.Description;
            await _roleRepository.Add(user);
        }

        public  async Task<List<DTO.Role>> GetRoles()
        {
            List<DAL.Models.Role> roles = await _roleRepository.GetAll();
            List<Location> locations = await _locationRepository.GetAll();
            List<Department> departments = await _departmentRepository.GetAll();
            List<DTO.Role> roleList = new();
            foreach(DAL.Models.Role item in roles)
            {
                DTO.Role user = new()
                {
                   Name = item.Name,
                   Location = locations[item.Location-1].Name,
                   Department = departments[item.Department-1].Name,
                   Description = item.Description
                };

                roleList.Add(user);
            }
             return roleList;
        }

        public async Task GenerateRoleList()
        {
            List<DAL.Models.Role> roles = await _roleRepository.GetAll() ;
            foreach (DAL.Models.Role role in roles)
            {
                Roles.Add(role.Id, role.Name);
            }
        }

    }
}