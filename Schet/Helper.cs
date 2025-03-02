using Schet.Models;

namespace Schet;

public static class Helper
{

	public static string[] SplitBySpaces(string line)
	{
		List<string> result = new List<string>();

		int spaces = 0;
		int wordStart = 0;
		bool addWord = false;
		for (int i = 0; i < line.Length; i++)
		{
			char ch = line[i];
			if (ch == ' ')
			{
				spaces++;
				if (spaces > 3)
					addWord = true;
			}
			else
				spaces = 0;

			if (addWord || i == line.Length - 1)
			{
				if (i == line.Length - 1)
					;

				string word = line.Substring(wordStart, i - wordStart + 1).Trim();
				if (!string.IsNullOrEmpty(word))
				{
					result.Add(word);
				}
				addWord = false;
				wordStart = i;
				spaces = 0;
			}
		}

		return result.ToArray();
	}

	public static Faktur ParsePokupka(string[] splits)
	{
		/////////////////////////////////////////////////
		//  ff.0000000633/10.01.2023    BG121248181/ИНА ХИМ ДИМИТРОВ        Проверка по ДДС       60.00      0.00      0.00
		var faktur = new Faktur();

		var number_date = splits[0].Trim().Split('/');
		var fakNumber = number_date[0];
		faktur.ID = fakNumber.Substring(3);

		var fakDate = number_date[1];
		var splitDate = fakDate.Split('.');
		int day = int.Parse(splitDate[0].Trim());
		int month = int.Parse(splitDate[1].Trim());
		int year = int.Parse(splitDate[2].Trim());
		faktur.DTime = new DateTime(year, month, day);

		var bgvat_name = splits[1].Trim().Split('/');
		faktur.ContractorID = bgvat_name[0].Trim();
		faktur.ContractorName = bgvat_name[1].Trim();

		faktur.Product = splits[2].Trim();

		string strBasePrice = splits[3].Trim().Replace(".", ",");
		faktur.PriceBase = decimal.Parse(strBasePrice);

		string strVATPrice = splits[4].Trim().Replace(".", ",");
		faktur.PriceVAT_20 = decimal.Parse(strVATPrice);

		var last = splits[splits.Length - 1].Trim();
		if (last.Contains("в брой"))
		{
			faktur.PaymentInCashe = true;
		}
		return faktur;
	}

	public static Faktur ParseProdagba(string[] splits)
	{
		// Дневник Продажби - 2024.06 Юни ------------------------------------------------- кл.09 --- кл.10 ---- кл.14 --- кл.15 ---- кл.17 --- кл.18 -------------
		// 01.0000000030/27.06.2024    9999999999/Kovacs Zsolt        нощувки B16         1337.61    120.39                         1337.61    120.39    // 1458.00
		// 02.0000000015/03.08.2024    NL805734958B01/BOOKING.COM     протокол 15          597.04    119.41     597.04    119.41          0         0    //  716.45
		var faktur = new Faktur();
		var number_date = splits[0].Trim().Split('/');
		var fakNumber = number_date[0];
		faktur.ID = fakNumber.Substring(3);

		var fakDate = number_date[1];
		var splitDate = fakDate.Split('.');
		int day = int.Parse(splitDate[0].Trim());
		int month = int.Parse(splitDate[1].Trim());
		int year = int.Parse(splitDate[2].Trim());
		faktur.DTime = new DateTime(year, month, day);

		var bgvat_name = splits[1].Trim().Split('/');
		faktur.ContractorID = bgvat_name[0].Trim();
		faktur.ContractorName = bgvat_name[1].Trim();

		faktur.Product = splits[2].Trim();

		string strBasePrice = splits[3].Trim().Replace(".", ",");
		faktur.PriceBase = decimal.Parse(strBasePrice);

		string strVATPrice = splits[4].Trim().Replace(".", ",");

		if (faktur.Product_Protokol())
		{
			faktur.PriceVAT_20 = decimal.Parse(strVATPrice);
		}
		else
		{
			faktur.PriceVAT_09 = decimal.Parse(strVATPrice); 
		}

		return faktur;
	}


	public static Payment ParsePayment(string[] columns)
	{
		// -------------------------------------------------------------------------------------------------------------------- кредит --- дебит
		// 12.03.2024    НАП                      206450255 АПВ П 22220424031667 0 04 001 08 03 2024                             18.44     0.00
		// 02.04.2024    МИГ МАРКЕТ ВАРНА ООД     Вносна бележка: Ток 2000000303,3000000450,3000000540,3000000630,3000000721      0.00     44.79
		var payment = new Payment();
		var split_date = columns[0].Trim().Split('.');
		int day = int.Parse(split_date[0].Trim());
		int month = int.Parse(split_date[1].Trim());
		int year = int.Parse(split_date[2].Trim());
		payment.DTime = new DateTime(year, month, day);

		payment.Contractor = columns[1].Trim();
		payment.Description = columns[2].Trim();
		payment.Debit = decimal.Parse(columns[3].Trim());
		payment.Credit = decimal.Parse(columns[4].Trim());
		return payment;
	}
}
