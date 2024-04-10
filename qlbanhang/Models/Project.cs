using System;
using System.Collections.Generic;

namespace qlbanhang.Models;

public partial class Project
{
    public string ProjId { get; set; } = null!;

    public string ProjName { get; set; } = null!;

    public string? ProjDesc { get; set; }

    public short? TeamLeader { get; set; }

    public string Product { get; set; } = null!;

    public virtual ICollection<ProjDeptBudget> ProjDeptBudgets { get; set; } = new List<ProjDeptBudget>();

    public virtual Employee? TeamLeaderNavigation { get; set; }

    public virtual ICollection<Employee> EmpNos { get; set; } = new List<Employee>();
}
