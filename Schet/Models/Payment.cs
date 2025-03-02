namespace Schet.Models;

public class Payment
{
	public DateTime DTime { get; set; }

	public string Contractor { get; set; }

	public string Description { get; set; }

	public decimal Debit { get; set; }

	public decimal Credit { get; set; }

	public Payment()
	{
	}

	public int Period
	{
		get { return DTime.Month; }
	}
}
