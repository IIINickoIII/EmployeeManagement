namespace EmployeeManagement.Api.Entities;

public class EmployeeDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? ManagerId { get; set; }
    public bool Enable { get; set; }
    public ICollection<EmployeeDto> Employees { get; set; }
}
