// See https://aka.ms/new-console-template for more information
using Schet;
using Schet.Constants;
using Schet.Models;



//Parsers.Parse_Hrono();


//var ddd = Helper.SplitBySpaces("ff33ff    dddddd  ggg   dddd    s1234s");
;

//string[] lines = File.ReadAllLines("C:\\gsiv\\EOOD\\2024\\reg_2024.cs");
string[] lines = File.ReadAllLines("C:\\gsiv\\DDS\\register\\reg_2024.cs");

var operation = string.Empty;
var vedomost = new Vedomost();

for (int i = 0; i < lines.Length; i++)
{
	var line = lines[i].Trim();

	if (String.IsNullOrEmpty(line))
		continue;
	if (line.StartsWith("//"))
		continue;

	if (line.StartsWith("operation:"))
	{
		var parts = line.Split(':');
		var split1 = parts[1].Trim();
		if (split1 == Operations.Pokupki)
			operation = Operations.Pokupki;
		else if (split1 == Operations.Prodagbi)
			operation = Operations.Prodagbi;
		else if (split1 == Operations.Bank)
			operation = Operations.Bank;
		else
			throw new Exception();

		continue;
	}

	//  Дневник Покупки - 2024.01 януари ------------------------------------------------------ кл.10 --- кл.11 --- всичко -- плащане -------
	//  01.3000000540/12.01.2024    BG207407068/МИГ-МАРКЕТ ЕООД        ток 12                    7.70      1.54     //   9.24    epay_2024.04.02_3
	var splits = Helper.SplitBySpaces(line);

	if (operation == Operations.Pokupki)
	{
		var pokupka = Helper.ParsePokupka(splits);
		// 01.3000000540/12.01.2024
		// BG207407068/МИГ-МАРКЕТ ЕООД
		// ток 12
		// 7.70
		// 1.34
		//  //   9.24
		//  epay_2024.04.02_
		int periodId = pokupka.DateCreated.Month;

		bool buy602_ExternalService = false;
		if (pokupka.ContractorVAT == Contractors.MIG_MARKET && pokupka.Product_Electricity())
			buy602_ExternalService = true;
		if (pokupka.ContractorVAT == Contractors.VIK_VARNA && pokupka.Product_Water())
			buy602_ExternalService = true;
		if (pokupka.ContractorVAT == Contractors.BULSATCOM)
			buy602_ExternalService = true;

		// 03.08.2023  602 401 195.75  Ф - ра    03.08.2023  1593161535  Booking.com B.V.комисион Вътреобщ.придобиване
		// 03.08.2023  453 / 1   453 / 2   39.15   ПЗДДС   03.08.2023  0000000010  Booking.com B.V.Самоначисляване на ДДС
		bool buyBooking = false;
		if (pokupka.ContractorVAT == Contractors.BOOKING)
			buyBooking = true;

		// 193 30.09.2022  652    401  2275.80  Ф-ра   28.09.2022  5400001182  Mиr Mapкет OОД Такса за поддръжка и        09
		// 193 30.09.2022  453/1  401   455.16  Ф-ра   28.09.2022  5400001182  Mиr Mapкет OОД Такса за поддръжка и        09
		bool buy652_Yearly = false;
		if (pokupka.ContractorVAT == Contractors.MIG_MARKET && pokupka.Product_Support())
			buy652_Yearly = true;

		bool buy609 = false;
		if (pokupka.ContractorVAT == Contractors.TECHNOPOLIS)
			buy609 = true;
		if (pokupka.ContractorVAT == Contractors.TECHMART)
			buy609 = true;
		if (pokupka.ContractorVAT == Contractors.IKEA)
			buy609 = true;
		if (pokupka.ContractorVAT == Contractors.METRO)
			buy609 = true;
		if (pokupka.ContractorVAT == Contractors.JYSK)
			buy609 = true;
		if (pokupka.ContractorVAT == Contractors.YANS)
			buy609 = true;

		//bool buy601_Material = false;
		//if (pokupka.ContractorVAT == Contractors.DEGA)
		//	buy601_Material = true;

		if (pokupka.ContractorVAT == Contractors.DEGA)
		{
			vedomost.AddDebitCredit(periodId, 601, 401, pokupka.PriceBase);		// 601 материали
			vedomost.AddDebitCredit(periodId, 4531, 401, pokupka.PriceVAT_20);
		}
		else if (buy602_ExternalService)
		{
			vedomost.AddDebitCredit(periodId, 602, 401, pokupka.PriceBase);
			vedomost.AddDebitCredit(periodId, 4531, 401, pokupka.PriceVAT_20);
		}
		else if (buy609)
		{
			vedomost.AddDebitCredit(periodId, 609, 401, pokupka.PriceBase);
			vedomost.AddDebitCredit(periodId, 4531, 401, pokupka.PriceVAT_20);
		}
		else if (buyBooking)
		{
			vedomost.AddDebitCredit(periodId, 602, 401, pokupka.PriceBase);
			vedomost.AddDebitCredit(periodId, 4531, 4532, pokupka.PriceVAT_20);
		}
		else if (buy652_Yearly)
		{
			vedomost.AddDebitCredit(periodId, 652, 401, pokupka.PriceBase);
			vedomost.AddDebitCredit(periodId, 4531, 401, pokupka.PriceVAT_20);
		}
		else
		{
			Console.WriteLine(line);
			throw new Exception();
		}

		if (line.Contains("брой"))
			;

		if (pokupka.Pay_InCash())
		{
			// 401/501 - 6.06  // в брой
			vedomost.AddDebitCredit(periodId, 401, 501, pokupka.PriceBase + pokupka.PriceVAT_20);
		}
		;
	}

	if (operation == Operations.Prodagbi)
	{
		var prodagba = Helper.ParseProdagba(splits);
		// 01.3000000540/12.01.2024
		// BG207407068/МИГ-МАРКЕТ ЕООД
		// ток 12
		// 7.70
		// 1.34
		//  //   9.24
		//  epay_2024.04.02_
		int periodId = prodagba.DateCreated.Month;


		// 0000000001  01.04.2023  411 703    33.03    Ф - ра    01.04.2023  0000000019  Физически лица  нощувки
		// 0000000001  01.04.2023  411 453/2   2.97    Ф - ра    01.04.2023  0000000019  Физически лица  нощувки
		if (prodagba.Product_Accomodation())
		{
			vedomost.AddDebitCredit(periodId, 411,  703, prodagba.PriceBase);
			vedomost.AddDebitCredit(periodId, 411, 4532, prodagba.PriceVAT_20);
		}
		else if (prodagba.Product_Protokol())
		{
			//
		}
		else
		{
			throw new Exception("");
		}


	}
}

;