namespace EmployeeDirectory.DAL.Models;

public partial class Location
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual List<Employee> Employees { get; set; } = new List<Employee>();

    public virtual List<Role> Roles { get; set; } = new List<Role>();
}
