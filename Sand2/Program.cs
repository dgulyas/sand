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
			table.sands[new HexPoint(1, -1, 0)].Height = 2;
			table.sands[new HexPoint(-1, 0, 1)].Height = 3;
			table.sands[new HexPoint(0, 0, 0)].Height = 4;
			table.sands[new HexPoint(1, 0, -1)].Height = 5;
			table.sands[new HexPoint(-1, 1, 0)].Height = 6;
			table.sands[new HexPoint(0, 1, -1)].Height = 7;


			SandToImageExporter.SaveMatrixAsImage(table.ToMatrix(), "C:\\Users\\david\\Desktop\\out\\2", "testH.bmp");


			//PrintMatrix(table.ToMatrix());
			//Console.WriteLine("===========================");
			//PrintSandTable(table);

			//table.ApplyGravity();

			//Console.WriteLine("===========================");
			//PrintSandTable(table);

			//Console.ReadLine();
		}

		public static void PrintSandTable(SandTable table)
		{
			foreach (var s in table.Print())
			{
				Console.WriteLine(s.Replace(" 0", "  "));
			}

		}

		public static void PrintMatrix(int [,] matrix)
		{
			var numRows = matrix.GetLength(0);
			var numCols = matrix.GetLength(1);
			for (int i = 0; i < numRows; i++)
			{
				for (int j = 0; j < numCols; j++)
				{
					Console.Write($"{matrix[i, j],2}  ");
				}
				Console.WriteLine();
			}
		}

	}
}
