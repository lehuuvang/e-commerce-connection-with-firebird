namespace qlbanhang.ViewModel
{
	public class orderViewModel
	{
		public int CustomerId { get; set; }
		public string shipName { get; set; }
		public string ShipAddress { get; set; }
		public int ShipVia { get; set; }																																																													
		public DateTime ShipDate { get; set; }	
	}
    public class orderViewModelforadmin
    {
		public int orderid { get; set; }
        public string CustomerName { get; set; }

		public DateTime orderDate { get; set; }
        public DateTime ShipDate { get; set; }
        public string shipName { get; set; }
        public string ShipAddress { get; set; }
		public string customerid { get; set; }
        public int accountid { get; set; }
    }
    public class orderdetailViewModel
	{
		public int OrderId { get; set; }
		public int ProductId{ get; set; }
		public string ProductName { get; set; }
		public string customerName { get; set; }
		public short Quantity { get; set; }
		public float Discount { get; set; }
		public short UnitPrice { get; set; }
		public short Status { get; set; }
	}
}
