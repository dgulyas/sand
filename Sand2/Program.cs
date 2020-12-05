using System;
using System.Collections.Generic;
using System.Linq;

//A hexagonal tiled sand table.

namespace Sand2
{
	class Program
	{
		private static string outputFolder = "C:\\Users\\david\\Desktop\\out\\2";

		static void Main(string[] args)
		{
			Test4();

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

		private static void Test1()
		{
			var table = new SandTable(2, 10);
			table.sands[new HexPoint(0, -1, 1)].Height = 1;
			table.sands[new HexPoint(1, -1, 0)].Height = 2;
			table.sands[new HexPoint(-1, 0, 1)].Height = 3;
			table.sands[new HexPoint(0, 0, 0)].Height = 4;
			table.sands[new HexPoint(1, 0, -1)].Height = 5;
			table.sands[new HexPoint(-1, 1, 0)].Height = 6;
			table.sands[new HexPoint(0, 1, -1)].Height = 7;

			SandToImageExporter.SaveMatrixAsImage(table.ToMatrix(), outputFolder, "testH.bmp");
		}

		private static void Test2() //Huge random table
		{
			var rand = new Random();

			var table = new SandTable(100, 10);
			foreach (var key in table.sands.Keys)
			{
				table.sands[key].Height = rand.Next(512);
			}

			SandToImageExporter.SaveMatrixAsImage(table.ToMatrix(), outputFolder, "testI.bmp");
		}

		private static void Test3() //reminder that highspots are white and low spots are black
		{
			var table = new SandTable(2, 10);
			table.sands[new HexPoint(-1, 0, 1)].Height = 200;
			SandToImageExporter.SaveMatrixAsImage(table.ToMatrix(), outputFolder, "testJ.bmp", false);
		}

		private static void Test4()
		{
			var table = new SandTable(100, 10);
			foreach (var key in table.sands.Keys)
			{
				var height = Math.Max(Math.Max(Math.Abs(key.Q), Math.Abs(key.R)), Math.Abs(key.S));
				table.sands[key].Height = height;
			}
			table.sands[new HexPoint(0, 0, 0)].Height = 1;
			SandToImageExporter.SaveMatrixAsImage(table.ToMatrix(), outputFolder, "testK.bmp");
		}

		private static void Test5() //Test
		{ }
		/*
		 * Clear "changedCells" list
		 * Apply pressure
		 * Apply Gravity
		 *
		 */




	}
}
