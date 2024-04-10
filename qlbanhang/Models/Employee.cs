using System;
using System.Collections.Generic;

namespace qlbanhang.Models;

public partial class Employee
{
    public short EmpNo { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? PhoneExt { get; set; }

    public DateTime HireDate { get; set; }

    public string DeptNo { get; set; } = null!;

    public string JobCode { get; set; } = null!;

    public short JobGrade { get; set; }

    public string JobCountry { get; set; } = null!;

    public string? FullName { get; set; }

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    public virtual Department DeptNoNavigation { get; set; } = null!;

    public virtual Job Job { get; set; } = null!;

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    public virtual ICollection<SalaryHistory> SalaryHistories { get; set; } = new List<SalaryHistory>();

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

    public virtual ICollection<Project> Projs { get; set; } = new List<Project>();
}
