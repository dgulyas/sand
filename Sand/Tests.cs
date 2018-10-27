﻿using System;

namespace Sand
{
	public static class Tests
	{
		private const int Size = 23;

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
			Exporters.SandToImageExporter.SaveMatrixAsImage(sand, @"C:\Users\dgulyas\Desktop\out\sand", $"testG.png");
			Exporters.SandTo3dFileExporter.SaveSandAs3dFile(sand, @"C:\Users\dgulyas\Desktop\out\sand", $"testG.obj");
			//Console.ReadLine();
		}

		public static void Test3()
		{
			var sand = SandTable.CreateSandTable(Size, Size, 15);

			SandTable.ClearHeightLimits(sand);
			SandTable.ApplyHeightLimitPattern(sand, HeightLimitPatternLibrary.SmallCube, new Point { X = 10, Y = 10, Height = 10 });

			SandTable.SettleMapTwoPass(sand);
			Exporters.SandToImageExporter.SaveMatrixAsImage(sand, @"C:\Users\david\Desktop\out", $"testK.png");
			Exporters.SandTo3dFileExporter.SaveSandAs3dFile(sand, @"C:\Users\david\Desktop\out", $"testK.obj");
		}

		public static void Test4()
		{
			var diameter = 30;
			var sand = SandTable.CreateSandTable(diameter, diameter, diameter/2);
			var spherePattern = HeightLimitPatternLibrary.Sphere(diameter);

			SandTable.ApplyHeightLimitPattern(sand,spherePattern, new Point{X=0, Y = 0, Height = diameter/2});

			foreach (var sandColumn in sand)
			{
				sandColumn.Height = sandColumn.HeightLimit;
			}

			Exporters.SandToImageExporter.SaveMatrixAsImage(sand, @"C:\Users\david\Desktop\out", $"testL.png");
			Exporters.SandTo3dFileExporter.SaveSandAs3dFile(sand, @"C:\Users\david\Desktop\out", $"testL.obj");
		}

	}
}
