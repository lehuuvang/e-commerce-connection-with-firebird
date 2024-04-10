using System;
using System.Collections.Generic;

namespace qlbanhang.Models;

public partial class Country
{
    public string Country1 { get; set; } = null!;

    public string Currency { get; set; } = null!;

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();
}
