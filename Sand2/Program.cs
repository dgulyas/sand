using System;
using System.Collections.Generic;
using System.Linq;

namespace Sand2
{
	class Program
	{
		static void Main(string[] args)
		{
			var table = new SandTable(2, 10);
			table.sands[new HexPoint(0, -1, 1)].Height = 1;
			table.sands[new HexPoint(1, -1, 0)].Height = 1;
			table.sands[new HexPoint(-1, 0, 1)].Height = 1;
			table.sands[new HexPoint(0, 0, 0)].Height = 1;
			table.sands[new HexPoint(1, 0, -1)].Height = 1;
			table.sands[new HexPoint(-1, 1, 0)].Height = 1;
			table.sands[new HexPoint(0, 1, -1)].Height = 1;

			PrintSandTable(table);

			table.ApplyGravity();

			Console.WriteLine("===========================");
			PrintSandTable(table);

			Console.ReadLine();
		}

		public static void PrintSandTable(SandTable table)
		{
			foreach (var s in table.Print())
			{
				Console.WriteLine(s.Replace(" 0", "  "));
			}

		}

	}
}
