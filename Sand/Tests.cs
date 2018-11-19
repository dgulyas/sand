using System;
using System.Collections.Generic;

namespace Sand
{
	public static class Tests
	{
		private const int Size = 23;
		private const string OutDir = @"C:\Users\mbryan\Desktop\Sand";

		public static void Test2()
		{
			var sand = SandTable.CreateSandTable(Size, Size, 15);

			SandTable.ClearHeightLimits(sand);
			SandTable.ApplyHeightLimitPattern(sand, HeightLimitPatternLibrary.SmallCube, new Point { X = 10, Y = 10, Height = 10 });

			//int iteration = 0;
			//SandToImageOutputter.SaveMatrixAsImage(sand, @"C:\Users\dgulyas\Desktop\out\testC", $"test{iteration++}.png");

			var iteration = 0;
			while (SandTable.SettleMapRandom(sand) && iteration++ < 5)
			{
				Console.WriteLine(iteration);
				//SandToImageOutputter.SaveMatrixAsImage(sand, @"C:\Users\dgulyas\Desktop\out\testC", $"test{iteration++}.png");
			}
			//PrintMapHeights(m_sand);
			Exporters.SandToImageExporter.SaveMatrixAsImage(sand, OutDir, "testG.png");
			Exporters.SandTo3dFileExporter.SaveSandAs3dFile(sand, OutDir, "testG.obj");
			//Console.ReadLine();
		}

		public static void Test3()
		{
			var sand = SandTable.CreateSandTable(Size, Size, 15);

			SandTable.ClearHeightLimits(sand);
			SandTable.ApplyHeightLimitPattern(sand, HeightLimitPatternLibrary.SmallCube, new Point { X = 10, Y = 10, Height = 10 });

			SandTable.SettleMapTwoPass(sand);
			Exporters.SandToImageExporter.SaveMatrixAsImage(sand, OutDir, "testK.png");
			Exporters.SandTo3dFileExporter.SaveSandAs3dFile(sand, OutDir, "testK.obj");
		}

		public static void Test4()
		{
			var diameter = 30;
			var sand = SandTable.CreateSandTable(diameter, diameter, diameter / 2);
			var spherePattern = HeightLimitPatternLibrary.Sphere(diameter);

			SandTable.ApplyHeightLimitPattern(sand, spherePattern, new Point { X = 0, Y = 0, Height = diameter / 2 });

			foreach (var sandColumn in sand)
			{
				sandColumn.Height = sandColumn.HeightLimit;
			}

			Exporters.SandToImageExporter.SaveMatrixAsImage(sand, OutDir, "testL.png");
			Exporters.SandTo3dFileExporter.SaveSandAs3dFile(sand, OutDir, "testL.obj");
		}

		public static void Test5()
		{
			var path1 = new Queue<Point>();
			var path2 = new Queue<Point>();
			var path3 = new Queue<Point>();

			for (int i = 1; i < 6; i++)
			{
				path1.Enqueue(new Point {Height = i, Y = 0, X = 0});
				path2.Enqueue(new Point { Height = 0, Y = i, X = 0 });
				path3.Enqueue(new Point { Height = 0, Y = 0, X = i });
			}

			var journey = new Journey(new Point { Height = 10, Y = 1, X = 1 });
			journey.AddPath(new Path(path1));
			journey.AddPath(new Path(path2));
			journey.AddPath(new Path(path3));

			while (true)
			{
				var nextPoint = journey.GetNextLocation();
				if (nextPoint == null)
				{
					break;
				}
				Console.WriteLine(nextPoint);
			}

			Console.ReadLine();
		}

	}
}
