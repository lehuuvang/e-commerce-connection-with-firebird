﻿using System;
using System.Collections.Generic;

namespace qlbanhang.Models;

public partial class Job
{
    public string JobCode { get; set; } = null!;

    public short JobGrade { get; set; }

    public string JobCountry { get; set; } = null!;

    public string JobTitle { get; set; } = null!;

    public string? JobRequirement { get; set; }

    public string? LanguageReq { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual Country JobCountryNavigation { get; set; } = null!;
}
