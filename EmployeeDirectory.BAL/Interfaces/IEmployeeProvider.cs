namespace EmployeeDirectory.BAL.Interfaces
{
    public interface IEmployeeProvider
    {
        public Task AddEmployee(DTO.Employee employee);

        public Task<List<BAL.DTO.EmployeeDisplay>> GetEmployees();
        public Task<List<BAL.DTO.Employee>> GetEmployeeDetails();

        public Task EditEmployee(BAL.DTO.Employee employeeInput, string id);

        public Task DeleteEmployee(string? id);

        public Task<BAL.DTO.Employee> GetEmployeeById(string? id);
    }
}
