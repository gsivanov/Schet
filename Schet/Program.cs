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
	var columns = Helper.SplitBySpaces(line);

	if (operation == Operations.Pokupki)
	{
		var pokupka = Helper.ParsePokupka(columns);
		// 01.3000000540/12.01.2024
		// BG207407068/МИГ-МАРКЕТ ЕООД
		// ток 12
		// 7.70
		// 1.34
		//  //   9.24
		//  epay_2024.04.02_

		if (pokupka.ContractorID == Contractors.DEGA )
			pokupka.ServiceType = ServiceType.Materials_601;

		if ((pokupka.ContractorID == Contractors.MIG_MARKET && pokupka.Product_Electricity()) ||
			(pokupka.ContractorID == Contractors.VIK_VARNA && pokupka.Product_Water()) ||
			(pokupka.ContractorID == Contractors.BULSATCOM)
			)
			pokupka.ServiceType = ServiceType.ExternalService_602;

		// 03.08.2023  602 401 195.75  Ф - ра    03.08.2023  1593161535  Booking.com B.V.комисион Вътреобщ.придобиване
		// 03.08.2023  453 / 1   453 / 2   39.15   ПЗДДС   03.08.2023  0000000010  Booking.com B.V.Самоначисляване на ДДС
		if (pokupka.ContractorID == Contractors.BOOKING	)
			pokupka.ServiceType = ServiceType.BookingCommision;

		if (pokupka.ContractorID == Contractors.TECHNOPOLIS ||
			pokupka.ContractorID == Contractors.TECHMART ||			
			pokupka.ContractorID == Contractors.IKEA ||
			pokupka.ContractorID == Contractors.METRO ||
			pokupka.ContractorID == Contractors.JYSK ||
			pokupka.ContractorID == Contractors.YANS
			)
			pokupka.ServiceType = ServiceType.OtherExpenses_609;

		// 193 30.09.2022  652    401  2275.80  Ф-ра   28.09.2022  5400001182  Mиr Mapкет OОД Такса за поддръжка и        09
		// 193 30.09.2022  453/1  401   455.16  Ф-ра   28.09.2022  5400001182  Mиr Mapкет OОД Такса за поддръжка и        09
		//bool buy652_Yearly = false;
		if (pokupka.ContractorID == Contractors.MIG_MARKET && pokupka.Product_Support())
			pokupka.ServiceType = ServiceType.FutureExpenses_652;



		if (pokupka.ServiceType == 0)
		{
			throw new Exception("unknown service type");
		}


		if (pokupka.ServiceType == ServiceType.Materials_601)
		{
			vedomost.AddDebitCredit( 601,  401, pokupka.PriceBase, pokupka.Period);
			vedomost.AddDebitCredit( 4531, 401, pokupka.PriceVAT_20, pokupka.Pariod );
		}
		if (pokupka.ServiceType == ServiceType.ExternalService_602)
		{
			vedomost.AddDebitCredit( 602, 401, pokupka.PriceBase, pokupka.Period );
			vedomost.AddDebitCredit( 4531, 401, pokupka.PriceVAT_20, pokupka.Period );
		}
		if (pokupka.ServiceType == ServiceType.OtherExpenses_609)
		{
			vedomost.AddDebitCredit( 609, 401, pokupka.PriceBase, pokupka.Period );
			vedomost.AddDebitCredit( 4531, 401, pokupka.PriceVAT_20, pokupka.Period );
		}
		if (pokupka.ServiceType == ServiceType.BookingCommision)
		{
			vedomost.AddDebitCredit( 602, 401, pokupka.PriceBase, pokupka.Period);
			vedomost.AddDebitCredit( 4531, 4532, pokupka.PriceVAT_20, pokupka.Period );
		}
		if (pokupka.ServiceType == ServiceType.FutureExpenses_652)
		{
			vedomost.AddDebitCredit( 652, 401, pokupka.PriceBase, pokupka.Period);
			vedomost.AddDebitCredit( 4531, 401, pokupka.PriceVAT_20, pokupka.Period);
		}


		if (pokupka.PaymentIsInCash())
		{
			// 401/501 - 6.06  // в брой
			vedomost.AddDebitCredit(401, 501, pokupka.PriceBase + pokupka.PriceVAT_20, pokupka.Period);
		}
		;
	}
	else if (operation == Operations.Prodagbi)
	{
		var prodagba = Helper.ParseProdagba(columns);
		// 01.3000000540/12.01.2024
		// BG207407068/МИГ-МАРКЕТ ЕООД
		// ток 12
		// 7.70
		// 1.34
		//  //   9.24
		//  epay_2024.04.02_


		// 0000000001  01.04.2023  411 703    33.03    Ф - ра    01.04.2023  0000000019  Физически лица  нощувки
		// 0000000001  01.04.2023  411 453/2   2.97    Ф - ра    01.04.2023  0000000019  Физически лица  нощувки
		if (prodagba.Product_Accomodation())
		{
			vedomost.AddDebitCredit( 411, 703, prodagba.PriceBase, prodagba.Period);
			vedomost.AddDebitCredit( 411, 4532, prodagba.PriceVAT_20, prodagba.Period);
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
	else if (operation == Operations.Bank)
	{
		var payment = Helper.ParsePayment(columns);
		// 12.03.2024
		// НАП	
		// 206450255 АПВ П 22220424031667 0 04 001 08 03 2024
		// 18.44
		// 0.00
	}
	else	
	{
		throw new Exception("");
	}


}

;