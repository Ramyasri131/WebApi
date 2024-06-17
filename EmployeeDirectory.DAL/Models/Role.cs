using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeDirectory.DAL.Models;

public partial class Role
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    [ForeignKey("LocationNavigation")]
    public int Location { get; set; }

    [ForeignKey("DepartmentNavigation")]
    public int Department { get; set; }

    public string? Description { get; set; }

    public virtual Department DepartmentNavigation { get; set; } = null!;

    public virtual List<Employee> Employees { get; set; } = new List<Employee>();

    public virtual Location LocationNavigation { get; set; } = null!;
}