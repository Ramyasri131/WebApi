namespace EmployeeDirectory.DAL.Models;

public partial class Manager
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual List<Employee> Employees { get; set; } = new List<Employee>();
}
