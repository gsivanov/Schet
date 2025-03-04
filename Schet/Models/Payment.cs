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
	public bool Contractor_Booking
	{
		get { return Contractor.Contains("BOOKING") ? true : false; }
	}
	public bool Contractor_NAP
	{
		get { return Contractor.Contains("НАП") ? true : false; }
	}
	public bool Contractor_VIK_VARNA
	{
		get { return Contractor.Contains("ВИК Варна") ? true : false; }
	}
	public bool Contractor_MigMarket
	{
		get { return Contractor.Contains("МИГ МАРКЕТ") ? true : false; }
	}
	public bool Contractor_InaHim
	{
		get { return Contractor.Contains("ИНА ХИМ") ? true : false; }
	}
	public bool Contractor_Svetlozar
	{
		get { return Contractor.Contains("Светлозар") ? true : false; }
	}

	public bool Contractor_BankTax
	{
		get { return Description.Contains("Такса банков превод") ? true : false; }
	}

}
