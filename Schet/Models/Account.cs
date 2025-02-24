namespace Schet.Models;

public class Account
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public decimal Debit { get; set; }
	public decimal Credit { get; set; }

	public Account()
	{
	}
}
