using System;
using System.Collections.Generic;

namespace qlbanhang.Model1s;

public partial class OrderDetail
{
    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public short Quantity { get; set; }

    public float Discount { get; set; }

    public short? UnitPrice { get; set; }

    public short? Status { get; set; }
}
