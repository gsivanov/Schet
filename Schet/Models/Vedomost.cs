﻿using Schet.Models;


public class Vedomost
{
	public List<Period> Periods { get; set; }

	public Vedomost()
	{
		Periods = new List<Period>();	
	}

	public void AddDebitCredit(int DebitAccount, int CreditAccount, decimal Money, int periodId)
	{
		var period = GetPeriod(periodId);
		var accountForDebit = period.GetAccount(DebitAccount);
		var accountForCredit = period.GetAccount(CreditAccount);
		accountForDebit.Debit += Money;
		accountForCredit.Credit += Money;
	}

	public void AddDebit(int periodId, int accountId, decimal DebitMoney)
	{
		var period = GetPeriod(periodId);
		var account = period.GetAccount(accountId);
		account.Debit += DebitMoney;
	}

	public void AddCredit(int periodId, int accountId, decimal CreditMoney)
	{
		var period = GetPeriod(periodId);
		var account = period.GetAccount(accountId);
		account.Credit += CreditMoney;
	}

	private Period GetPeriod(int periodId)
	{
		foreach(var per in Periods)
		{
			if (per.Id == periodId)
			{
				return per;
			}
		}
		
		var period = new Period();
		Periods.Add(period);
		return period;
	}

}

