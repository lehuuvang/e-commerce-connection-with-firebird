namespace qlbanhang.ViewModel
{
    public class ProductViewModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; } 
		public short? UnitPrice { get; set; }
		public string? Picture { get; set; }
		public string? CategoryName { get; set; }
         public string? Description { get; set; }
    }
	public class DetailProductViewModel
	{
		public int ProductID { get; set; }
		public string ProductName { get; set; }
		public short? UnitPrice { get; set; }
		public string? Picture { get; set; }
		public short? QuantityUnit { get; set; }
		public string? CategoryName { get; set; }
		public string? Description { get; set; }
	}
}
