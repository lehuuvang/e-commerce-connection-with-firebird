namespace qlbanhang.ViewModel
{
	public class CartItem
	{
		public int ProductId { get; set; }

		public string ProductName { get; set; }		
		public string? Picture { get; set; }
		public short? UnitPriceSS{ get; set; }
		public short? quantity { get; set; }
		public int TotalUnits => (short)(UnitPriceSS * quantity);

	}
}
