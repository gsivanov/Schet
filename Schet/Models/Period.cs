namespace Schet.Models;

public class Period
{
	public int Id { get; set; }


	public List<Account> Accounts = new List<Account>();

	public Period()
	{
	}

	public Account GetAccount(int accountId)
	{
		foreach (var acc in Accounts)
		{
			if (acc.Id == accountId)
			{
				return acc;
			}
		}

		var account = new Account();
		Accounts.Add(account);
		return account;
	}

}