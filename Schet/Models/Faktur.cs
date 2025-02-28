namespace Schet.Models;

public class Faktur
{
	public string ID { get; set; }

	public DateTime DTime { get; set; }

	public string Product { get; set; }

	public decimal PriceBase { get; set; }

	public decimal PriceVAT_20 { get; set; }

	public decimal PriceVAT_09 { get; set; }

	public string ContractorID { get; set; }

	public string ContractorName { get; set; }

	public bool PaymentInCashe { get; set; }

	public int ServiceType { get; set; }

	public Faktur()
	{
	}

	public int Period
	{
		get { return DTime.Month; }
	}

	public bool Product_Electricity()
	{
		return (Product.Contains("ток") || Product.Contains("Ток")) ? true : false;
	}
	public bool Product_Water()
	{
		return (Product.Contains("вода") || Product.Contains("Вода")) ? true : false;
	}
	public bool Product_Support()
	{
		return Product.Contains("поддръжка") ? true : false;
	}
	public bool Product_Protokol()
	{
		return Product.Contains("протокол") ? true : false;
	}

	public bool Product_Accomodation()
	{
		return Product.Contains("нощувки") ? true : false;
	}


	public bool PaymentIsInCash()
	{
		return Product.Contains("брой") ? true : false;
	}

	// 06: 401/501 - 109.90  // в брой
}