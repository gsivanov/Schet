using System.Globalization;
using System.Text;

namespace Schet;

public class Parsers
{
	public static void Parse_Hrono()
	{
		string readFile = "C:\\gsiv\\DDS\\register\\ХронРег2023_tabs.txt";
		string saveFile = "C:\\gsiv\\DDS\\register\\ХронРег2023.html";

		string[] lines = File.ReadAllLines(readFile);

		string header = @"<tr style='color:white; background-color:gray;  border-top: 1px solid red;'>
		<th > Контиране </th>
		<th > Дата </th>
		<th > Дебит  </th>
		<th > Кредит  </th>
		<th > Сума </th>
		<th > Док.вид </th>
		<th > Док.дата </th>
		<th > Документ Ном. </th>
		<th > Партньор </th>
		<th > Основание </th>
		<th > Забележка </th>
		<!-- <th> Експорт </th>  -->
		<!-- <th> Потребител </th> -->
		</tr>
		";

		// table th { position: sticky; top: 0;  z-index: 1; border-bottom: 1px solid red; }
		var sb = new StringBuilder(@"
		<html>
		<head>
		<style>
		table    { border-right: 1px solid black; }
		// table th { position: sticky; top: 0;  z-index: 1; border-bottom: 1px solid red; }
		// table tr:nth-child(2)  { top: 20px; border-right: 1px solid green; }
		table td { border-bottom: 1px solid green; border-left: 1px solid green; padding: 2px 2px 2px 2px; }
        table td:nth-child(2) { text-align: begin; border-right: 1px solid green; }
		table td:nth-child(3) { text-align: begin;  }
		table td:nth-child(4) { text-align: begin; border-right: 1px solid green; }
		table td:nth-child(5) { text-align: end; }
		table td:nth-child(6) { text-align: end; }
		table td:nth-child(7) { text-align: end; }
		table td:nth-child(8) { text-align: end; }
		</style>
		</head>
		<body>
		<table>
		");

		int oldMonth = 0;

		for (int i = 0; i < lines.Length; i++)
		{
			string line = lines[i].Trim();
			if (string.IsNullOrEmpty(line))
				continue;

			// 0000000132	01.01.2023	121/2	123	10 640.69							01.2023	Служебен потребител
			line = line.Replace("ЕТ ИНА-ХИМ-ДИМИТЪР ДИМИТРОВ", "ЕТ ИНА-ХИМ.ДИМ");
			line = line.Replace("МЕТРО КЕШ ЕНД КЕРИ БЪЛГАРИЯ ЕООД", "МЕТРО КЕШ КЕРИ");
			line = line.Replace("МИГ МАРКЕТ ВАРНА ЕООД", "МИГ МАРКЕТ ЕООД");
			line = line.Replace("Мьомакс България ООД", "Мьомакс ООД");
			line = line.Replace("Светозар Георгиев Иванов", "Светлозар Г Иванов");
			line = line.Replace("Апартамент А98", "Ап.А98");
			line = line.Replace("Служебен потребител", "Служ.потребител");
			line = line.Replace("Вътреобщностн о придобиване", "Вътреобщ.придобиване");
			line = line.Replace("Самоначисляване на ДДС при услуга", "Самоначисляване на ДДС");
			line = line.Replace("Самоначисляване на ДДС при ВОП", "Самоначисляване на ДДС");
			try
			{
				var splits = line.Split('\t');
				var contirane = splits[00].Trim();
				var dTime = splits[01].Trim();
				var debitAcc = splits[02].Trim();
				var creditAcc = splits[03].Trim();
				var sum = splits[04].Trim();
				var docVid = splits[05].Trim();
				var docDate = splits[06].Trim();
				var docNum = splits[07].Trim();
				var partner = splits[08].Trim();
				var osnovanie = splits[09].Trim();
				var zabelezka = splits[10].Trim();
				var export = splits[11].Trim();
				var potrebitel = splits[12].Trim();

				var mm_yyyy = dTime.Split('.');
				int dateMonth = int.Parse(mm_yyyy[1]);

				var dd_mm_yyyy = export.Split('.');
				int exportMonth2 = int.Parse(dd_mm_yyyy[0]);

				if (dateMonth != exportMonth2)
					;

				if (oldMonth < dateMonth)
				{
					sb.AppendLine(header);
					oldMonth = dateMonth;
				}

				sb.AppendLine("<tr>");
				sb.AppendLine("<td>" + contirane + "</td>");
				sb.AppendLine("<td>" + dTime + "</td>");
				sb.AppendLine("<td>" + debitAcc + "</td>");
				sb.AppendLine("<td>" + creditAcc + "</td>");
				sb.AppendLine("<td>" + sum + "</td>");
				sb.AppendLine("<td>" + docVid + "</td>");
				sb.AppendLine("<td>" + docDate + "</td>");
				sb.AppendLine("<td>" + docNum + "</td>");
				sb.AppendLine("<td>" + partner + "</td>");
				sb.AppendLine("<td>" + osnovanie + "</td>");
				sb.AppendLine("<td>" + zabelezka + "</td>");
				//sb.AppendLine("<td>" + export + "</td>");
				//sb.AppendLine("<td>" + potrebitel + "</td>");
				sb.AppendLine("</tr>");

			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
			;
		}

		sb.AppendLine("</table>");
		sb.AppendLine("</body>");
		sb.AppendLine("</html>");

		// C:\\gsiv\\EOOD\\2024.06_ГФО\\hron2023tabs.txt";
		File.WriteAllText(saveFile, sb.ToString());
		;
	}

	public static void Parse_Bank()
	{
		// from https://www.epay.bg/v3main/report/mvmnts#
		// from C:\\gi\\EOOD\\2024\\bank\\2023.epay.txt
		//   to C:\\gi\\EOOD\\2024\\bank\\2023.epay.html

		string read_from_file = "C:\\gsiv\\EOOD\\2025\\2024.epay.txt";

		var sb = new StringBuilder(@"
		<html>
		<head>
		<style>
		table    { border: 1px solid white; }
		table th { position: sticky; top: 0;  background: white; z-index: 1; border-bottom: 1px solid red; }
		table tr:nth-child(2) th { top: 20px; }
		table td { border-top: 1px solid green; border-left: 1px solid green; padding: 2px 2px 2px 2px; }
		table td:nth-child(3) {  }
		table td:nth-child(4) {                  border-right: 1px solid green; }
		table td:nth-child(5) { text-align: end; }
		table td:nth-child(6) { text-align: end; border-right: 1px solid green; }
		table td:nth-child(7) { text-align: end; }
		table td:nth-child(8) { text-align: end; }
		</style>
		</head>
		<body>
		<table>
		<tr>
		<th> Дата </th>
		<th> Кореспондент </th>
		<th> Описание </th>
		<th> Приход </th>
		<th> Разход </th>
		</tr>
		");
		sb.AppendLine();

		string[] lines = File.ReadAllLines(read_from_file);

		sb.AppendLine("<tr>");
		int column = 0;
		for (int j = 0; j < lines.Length; j++)
		{
			var line = lines[j].Trim();
			column++;
			if (String.IsNullOrEmpty(line))
				continue;
			if (line.StartsWith("//"))
				continue;
			if (line == "Микросметка")
				continue;

			if (line.EndsWith(".2024"))
			{
				column = 1;
				sb.AppendLine();
				sb.AppendLine("</tr><tr>");
			}

			if (column == 5)
			{
				string[] splits = line.Split('\t');
				string prihod = splits[0].Trim().Replace("0.00", "");
				string razhod = splits[1].Trim().Replace("0.00", "");
				sb.AppendLine("<td>" + prihod + "</td><td>" + razhod + "</td>");
			}
			else
			{
				sb.AppendLine("<td>" + line + "</td>");
			}
		}
		File.WriteAllText("C:\\gsiv\\EOOD\\2025\\2024.epay_bank.html", sb.ToString());
	}

	public static void Parse_Osigur()
	{
		var sb = new StringBuilder(@"
		<html>
		<head>
		<style>
		table    { border: 1px solid black; }
		table th { position: sticky; top: 0;  background: white; z-index: 1; border-bottom: 1px solid red; }
		table tr:nth-child(2) th { top: 20px; }
		table td { border-top: 1px solid green; border-left: 1px solid green; padding: 2px 2px 2px 2px; }
		table td:nth-child(3) { text-align: end; }
		table td:nth-child(4) { text-align: end; }
		table td:nth-child(5) { text-align: end; }
		table td:nth-child(6) { text-align: end; }
		table td:nth-child(7) { text-align: end; }
		table td:nth-child(8) { text-align: end; }
		</style>
		</head>
		<body>
		<table>
		");
		sb.AppendLine();

		string[] lines = File.ReadAllLines("C:\\gi\\EOOD\\2024\\osigur.csv");
		for (int i = 0; i < lines.Length; i++)
		{
			var line = lines[i].Trim();
			if (String.IsNullOrEmpty(line))
				continue;
			if (line.StartsWith("//"))
				continue;

			sb.AppendLine("<tr>");
			var splits = line.Split('\t');
			var first = splits[0].Trim();
			for (int k = 0; k < splits.Length; k++)
			{
				var word = splits[k].Trim();
				sb.AppendLine("<td>" + word + "</td>");
			}
			sb.AppendLine("</tr>");
		}

		File.WriteAllText("C:\\gi\\EOOD\\2024\\osigur.html", sb.ToString());
	}

	public static void Parse_Old()
	{

		//  C:\gi\EOOD\2022_Ina_Dimitrova\2022_хронология.csv
		//  C:\gi\EOOD\2022_Ina_Dimitrova\2022_хронология.html
		{
			var sb = new StringBuilder(@"
    <html>
    <head>
    <style>
    table    { border: 1px solid black; }
    table th { position: sticky; top: 0;  background: white; z-index: 1; border-bottom: 1px solid red; }
    table tr:nth-child(2) th { top: 20px; }
    table td { border-top: 1px solid green; border-left: 1px solid green; padding: 2px 2px 2px 2px; }
    table td:nth-child(3) { text-align: end; }
    table td:nth-child(4) { text-align: end; }
    table td:nth-child(5) { text-align: end; }
    table td:nth-child(6) { text-align: end; }
    table td:nth-child(7) { text-align: end; }
    table td:nth-child(8) { text-align: end; }
    </style>
    </head>
    <body>
    <table>
    ");
			sb.AppendLine();

			string[] lines = File.ReadAllLines("C:\\gi\\EOOD\\2022_Ina_Dimitrova\\2022_хронология.csv");
			for (int i = 0; i < lines.Length; i++)
			{
				var line = lines[i].Trim();
				if (String.IsNullOrEmpty(line))
					continue;
				if (line.StartsWith("//"))
					continue;

				sb.AppendLine("<tr>");

				var splits = line.Split(',');
				var first = splits[0].Trim();
				if (String.IsNullOrEmpty(first))
					continue;

				// Контиране,Дата,Дебит,Кредит,Сума,Док.,Док. дата,Документ,Партньор,Основание,Забележка,Месец за ,,Потребител
				for (int k = 0; k < splits.Length; k++)
				{
					var word = splits[k].Trim();
					if (k == 11)
					{
						string[] temp = word.Split('.');
						if (temp.Length == 2)
						{
							int month = int.Parse(temp[0]);
							word = month.ToString("00");
						}
					}
					if (word == "Потребител")
						word = "";
					if (word == "Служебен потребител")
						word = "";
					if (word == "Месец за")
						word = "Месец";

					sb.Append("<td>" + word + "</td>");
				}
				sb.AppendLine("</tr>");
			}

			File.WriteAllText("C:\\gi\\EOOD\\2022_Ina_Dimitrova\\2022_хронология.html", sb.ToString());
		}
		return;

		// C:\gi\EOOD\2024\bank\2023.01_fibank.txt
		// C:\gi\EOOD\2024\bank\2023.02_fibank.txt
		// C:\gi\EOOD\2024\bank\2023.03_fibank.txt
		// C:\gi\EOOD\2024\bank\2023.04_fibank.txt
		{
			var sb = new StringBuilder(@"
    <html>
    <head>
    <style>
    table    { border: 1px solid black; }
    table th { position: sticky; top: 0;  background: white; z-index: 1; border-bottom: 1px solid red; }
    table tr:nth-child(2) th { top: 20px; }
    table td { border-top: 1px solid green; border-left: 1px solid green; padding: 2px 2px 2px 2px; }
    table td:nth-child(3) { text-align: end; }
    table td:nth-child(4) { text-align: end; border-right: 1px solid green; }
    table td:nth-child(5) { text-align: end; }
    table td:nth-child(6) { text-align: end; border-right: 1px solid green; }
    table td:nth-child(7) { text-align: end; }
    table td:nth-child(8) { text-align: end; }
    </style>
    </head>
    <body>
    <table>
    ");
			sb.AppendLine();

			for (int i = 1; i < 5; i++)
			{
				string file_name = "C:\\gi\\EOOD\\2024\\bank\\2023." + i.ToString("00") + "_fibank.txt";
				string[] lines = File.ReadAllLines(file_name);
				for (int j = 0; j < lines.Length; j++)
				{
					var line = lines[j].Trim();
					if (String.IsNullOrEmpty(line))
						continue;
					if (line.StartsWith("//"))
						continue;

					sb.AppendLine("<tr>");
					string styling = String.Empty;
					var splits = line.Split('\t');
					string first = splits[0].Trim();
					if (first == "Референция:")
						styling = " style='background-color:yellow'";
					for (int k = 0; k < splits.Length; k++)
					{
						var word = splits[k].Trim();
						if (k == 2)
							continue;
						if (String.IsNullOrEmpty(styling) && !String.IsNullOrEmpty(word))
						{
							if (k == 1 || k == 2)
							{
								DateTime dt = DateTime.ParseExact(word, "dd/MM/yyyy", CultureInfo.InvariantCulture);
								word = dt.ToString("yyyy.MM.dd");
							}
							if (k == 3 || k == 4)
							{
								decimal dcm = decimal.Parse(word);
								string ff = dcm.ToString("f2");
								word = ff.Replace(",", ".");
							}
						}
						if (word == "Получен превод в лева")
							word = "Получен превод";
						if (word == "Нареден превод в лева-e-banking")
							word = "Нареден превод";
						if (word == "Такса за нареден левов превод")
							word = "Такса нареден превод";
						if (word == "Такса за поддръжка на сметка")
							word = "Такса сметка";
						sb.Append("<td " + styling + ">" + word + "</td>");
					}
					sb.AppendLine("</tr>");
				}
			}
			File.WriteAllText("C:\\gi\\EOOD\\2024\\bank\\2023.fibank.html", sb.ToString());
		}
		return;






		;
		return;
		;
		{
			// C:\\gi\\EOOD\\2022_Ina_Dimitrova\\2022_bank\\22_01.txt";
			// C:\\gi\\EOOD\\2022_Ina_Dimitrova\\2022_bank\\22_12.txt";
			// C:\\gi\\EOOD\\2022_Ina_Dimitrova\\2022_bank\\22__bank.html";  // result
			var sb = new StringBuilder(@"
    <html>
    <head>
    <style>
    table, th, td {  border: 1px solid black;  }
    </style>
    </head>
    <body>
    <table>
    ");
			sb.AppendLine();

			for (int i = 1; i < 13; i++)
			{
				string periodID = i.ToString("00");
				string file_name = "C:\\gi\\EOOD\\2022_Ina_Dimitrova\\2022_bank\\22_" + periodID + ".txt";

				sb.AppendLine("<tr><td>2022." + periodID + "</td></tr>");
				string[] lines = File.ReadAllLines(file_name);

				for (int j = 0; j < lines.Length; j++)
				{
					var line = lines[j];


					if (String.IsNullOrEmpty(line))
						continue;
					if (line.StartsWith("//"))
						continue;
					var splits = line.Split('\t');
					sb.Append("<tr>");
					for (int s = 0; s < splits.Length; s++)
					{
						var strSplit = splits[s].Trim();
						if (strSplit.EndsWith(':'))
							strSplit = strSplit.Trim(':');

						if (strSplit == "Плащане / отмяна ПОС")
							strSplit = "Плащане ПОС";


						sb.Append("<td>" + strSplit + "</td>");
					}
					sb.AppendLine("</tr>");
				}
			}

			sb.AppendLine(@"
    </table>
    </body>
    </html>
    ");

			string output_file = "C:\\gi\\EOOD\\2022_Ina_Dimitrova\\2022_bank\\22__bank.html";
			File.WriteAllText(output_file, sb.ToString());

		}


		return;


		{

			int periodID = 11;

			var sb = new StringBuilder(@"
    <html>
    <head>
    <style>
    table    { border: 1px solid black; }
    table th { position: sticky; top: 0;  background: white; z-index: 1; border-bottom: 1px solid red; }
    table tr:nth-child(2) th { top: 20px; }
    table td { border-top: 1px solid green; border-left: 1px solid green; padding: 2px 2px 2px 2px; }
    table td:nth-child(3) { text-align: end; }
    table td:nth-child(4) { text-align: end; border-right: 1px solid green; }
    table td:nth-child(5) { text-align: end; }
    table td:nth-child(6) { text-align: end; border-right: 1px solid green; }
    table td:nth-child(7) { text-align: end; }
    table td:nth-child(8) { text-align: end; }
    </style>
    </head>
    <body>

    <table>
    <tr>
    <td > </td>
    <td > 2022." + periodID + @"   </td>
    <td colspan=2> Начални салда </td>
    <td colspan=2> Обороти       </td>
    <td colspan=2> Крайни салда  </td>
    </tr>
    <tr>
    <td> #  </td>
    <td> Сметка   </td>
    <td> Дебит  </td>
    <td> Кредит </td>
    <td> Дебит  </td>
    <td> Кредит </td>
    <td> Дебит  </td>
    <td> Кредит </td>
    </tr>

    ");

			string input_file = "C:\\gi\\EOOD\\2022_Ina_Dimitrova\\00." + periodID + "_ob_ved.txt";
			string[] lines = File.ReadAllLines(input_file);

			for (int i = 0; i < lines.Length; i++)
			{
				string line = lines[i];
				if (String.IsNullOrEmpty(line))
					continue;
				if (line.StartsWith("//"))
					continue;
				var splits = line.Split('\t');
				string accID = splits[0];
				string accName = splits[1];

				if (accName == "Основен капитал, изискващ регистрация")
					accName = "Основен капитал за регистрация";
				if (accName == "Непокрита загуба от минали години")
					accName = "Непокрита загуба от минали год";
				if (accName == "Печалби и загуби от текущата година")
					accName = "Печалби/загуби от текуща год";
				if (accName == "Амортизация на дълготрайни материални активи")
					accName = "Амортизация на Дълг.Мат.Актив";
				if (accName == "Разчети за данък върху добавената стойност")
					accName = "Разчети за ДДС";
				if (accName == "Разчети за данъци върху доходи на физически лица")
					accName = "Разчет данък доход физ.лица";
				if (accName == "Разплащателна сметка в левове")
					accName = "Разплащателна сметка в левa";
				if (accName == "Финансови разходи за бъдещи периоди")
					accName = "Фин.разходи за бъдещи периоди";

				sb.AppendLine("<tr><td>" + accID + "</td><td>" + accName + "</td>");

				string numbers = splits[2];
				string[] numsplits = numbers.Split(' ');
				for (int j = 0; j < numsplits.Length; j++)
				{
					string num = numsplits[j].Trim();
					if (num == "0.00")
						num = String.Empty;
					sb.Append("<td>" + num + "</td>");
				}
				sb.AppendLine("</tr>");
			}
			sb.AppendLine("</table>");
			sb.AppendLine("</body></html>");
			;
			string output_file = "C:\\gi\\EOOD\\2022_Ina_Dimitrova\\22." + periodID + "_ob_ved_gen.html";
			File.WriteAllText(output_file, sb.ToString());
			;


		}


		/*

		using System.Text.Json;
		using System.Text.Json.Nodes;
		using ConsoleApp1;

		string jsonString = File.ReadAllText("C:\\gi\\code\\aa\\saveip.txt");
		//[{"id":15992,"dtime":"2024.02.13 11:38:27","ip":",194.12.237.67,p=booking,Sofia,BG,Ng Ltd,"},
		// {"id":1,"dtime":"2023.02.10 15:43:50","ip":",95.111.51.212,p=eood,Sofia,BG,A1 Bulgaria EAD,"}]
		var jsonObject = JsonNode.Parse(jsonString);
		var arr = jsonObject.AsArray();
		var list = new List<Model1>();
		for (int i = 0; i < arr.Count; i++) // 16927
		{
			var item = arr[i];
			if (item == null)
				continue;

			var model = new Model1();
			var xxx = item["id"].AsValue();
			model.id = int.Parse( xxx.ToString() );
			model.dt = item["dtime"].AsValue().ToString();// "2024.01.01 11:38:27";

			var ip = item["ip"].AsValue().ToString();

			string[] splits = ip.Split(',');

			if (!String.IsNullOrEmpty(splits[0]))
				;
			model.ip = splits[1];
			model.pg = splits[2].Substring(2);// p=eood
			model.tw = splits[3]; // BG
			model.ct = splits[4]; // Sofia
			model.cp = splits[5]; // A1 Bulgaria EAD
			list.Add(model);
		}

		var newJson = JsonSerializer.Serialize( list );
		File.WriteAllText("C:\\gi\\code\\aa\\saveip2.json", newJson);
		//string jsonString = JsonSerializer.Serialize(weatherForecast);
		//Console.WriteLine(jsonString);



		*/




	}
}
