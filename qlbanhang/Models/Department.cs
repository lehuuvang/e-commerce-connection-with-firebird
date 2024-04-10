using System;
using System.Collections.Generic;

namespace qlbanhang.Models;

public partial class Department
{
    public string DeptNo { get; set; } = null!;

    public string Department1 { get; set; } = null!;

    public string? HeadDept { get; set; }

    public short? MngrNo { get; set; }

    public decimal? Budget { get; set; }

    public string? Location { get; set; }

    public string? PhoneNo { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual Department? HeadDeptNavigation { get; set; }

    public virtual ICollection<Department> InverseHeadDeptNavigation { get; set; } = new List<Department>();

    public virtual Employee? MngrNoNavigation { get; set; }

    public virtual ICollection<ProjDeptBudget> ProjDeptBudgets { get; set; } = new List<ProjDeptBudget>();
}
