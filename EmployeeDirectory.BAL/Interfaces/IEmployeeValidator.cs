namespace EmployeeDirectory.BAL.Interfaces
{
    public interface IEmployeeValidator
    {
        public Task ValidateDetails(DTO.Employee employee);

    }
}
