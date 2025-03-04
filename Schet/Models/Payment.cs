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

	public bool IsDebit
	{
		get { return Debit > 0 ? true : false;  }
	}
	public bool IsCredit
	{
		get { return Credit > 0 ? true : false; }
	}
	public bool Contractor_NAP
	{
		get { return Contractor.Contains("НАП") ? true : false; }
	}

}
