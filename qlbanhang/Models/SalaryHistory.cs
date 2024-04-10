using System;
using System.Collections.Generic;

namespace qlbanhang.Models;

public partial class SalaryHistory
{
    public short EmpNo { get; set; }

    public DateTime ChangeDate { get; set; }

    public string UpdaterId { get; set; } = null!;

    public double PercentChange { get; set; }

    public double? NewSalary { get; set; }

    public virtual Employee EmpNoNavigation { get; set; } = null!;
}
