﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Sand
{
	public class SandTable
	{
		private const int Size = 50;
		private static readonly SandColumn[,] Sand = new SandColumn[Size,Size];
		private static readonly Random Rnd = new Random();

		public static void Test2()
		{
			for (int i = 0; i < Size; i++)
			{
				for (int j = 0; j < Size; j++)
				{
					Sand[i, j] = new SandColumn{HeightLimit = int.MaxValue, Height = 20};
				}
			}

			Sand[25, 25] = new SandColumn { HeightLimit = int.MaxValue, Height = 500 };

			//ApplyHeightLimitPattern(m_sand, HeigthLimitPatternLibrary.SmallCube, new Point{X=10, Y=10, Height = 10});

			int iteration = 0;
			SandToImageOutputter.SaveMatrixAsImage(Sand, @"C:\Users\dgulyas\Desktop\out\testC", $"test{iteration++}.png");

			while (SettleMap(Sand))
			{
				Console.WriteLine("a");
				SandToImageOutputter.SaveMatrixAsImage(Sand, @"C:\Users\dgulyas\Desktop\out\testC", $"test{iteration++}.png");
			}
			//PrintMapHeights(m_sand);
			SandToImageOutputter.SaveMatrixAsImage(Sand, @"C:\Users\dgulyas\Desktop\out\testC", $"test{iteration}.png");
			//Console.ReadLine();
		}

		private static bool SettleMap(SandColumn[,] sand)
		{
			var sandMoved = false;
			for(int x = 0; x < sand.GetLength(0); x++)
			{
				for(int y = 0; y < sand.GetLength(1); y++)
				{
					var columnSandMoved = SettleColumn(sand, new Point { X = x, Y = y });
					if (columnSandMoved)
					{
						sandMoved = true;
					}
				}
			}
			return sandMoved;
		}

		private static bool SettleColumn(SandColumn[,] sand, Point location)
		{
			var column = sand[location.X, location.Y];
			var neighbours = GetNeighbours(sand, location);

			var columnChangedOnce = false; //if the column changed at all

			bool columnChanged; //if the column changed in this iteration
			do
			{
				columnChanged = false;

				var lowerPressureNeighbours = new List<SandColumn>();
				var equalPressureLowerHeightNeighbours = new List<SandColumn>();

				foreach (var neighbour in neighbours)
				{
					if (column.Pressure > 0 && column.DecreasedPressure >= neighbour.IncreasedPressure)
					{
						lowerPressureNeighbours.Add(neighbour);
					}
					else if (column.Pressure <= 0 && column.DecreasedHeight >= neighbour.IncreasedHeight)
					{
						equalPressureLowerHeightNeighbours.Add(neighbour);
					}
				}

				if (lowerPressureNeighbours.Count > 0)
				{
					var lowestPressure = lowerPressureNeighbours.Min(n => n.IncreasedPressure);
					lowerPressureNeighbours.RemoveAll(n => n.Pressure > lowestPressure);

					SandColumn chosenNeighbour;
					if (lowerPressureNeighbours.Count == 1)
					{
						chosenNeighbour = lowerPressureNeighbours.First();
					}
					else
					{
						chosenNeighbour = lowerPressureNeighbours[Rnd.Next(lowerPressureNeighbours.Count)];
					}

					MoveSand(column, chosenNeighbour);
					columnChanged = true;
				}
				else if(equalPressureLowerHeightNeighbours.Count > 0)
				{
					MoveSand(column, equalPressureLowerHeightNeighbours[Rnd.Next(equalPressureLowerHeightNeighbours.Count)]);
					columnChanged = true;
				}

				columnChangedOnce = columnChanged || columnChangedOnce;

			} while (columnChanged);

			return columnChangedOnce;
		}

		private static void MoveSand(SandColumn source, SandColumn dest)
		{
			source.Height--;
			dest.Height++;
		}

		private static List<SandColumn> GetNeighbours(SandColumn[,] sand, Point location)
		{
			var x = location.X;
			var y = location.Y;

			var neighbours = new List<SandColumn>();
			if (x - 1 >= 0)
			{
				neighbours.Add(sand[x - 1, y]);
			}
			if (x + 1 < sand.GetLength(0))
			{
				neighbours.Add(sand[x + 1, y]);
			}
			if (y - 1 >= 0)
			{
				neighbours.Add(sand[x, y - 1]);
			}
			if (y + 1 < sand.GetLength(1))
			{
				neighbours.Add(sand[x, y + 1]);
			}

			return neighbours;
		}

		private static void PrintMapHeights(SandColumn[,] sand)
		{
			for (int x = 0; x < sand.GetLength(0); x++)
			{
				for (int y = 0; y < sand.GetLength(1); y++)
				{
					Console.Write(sand[x,y].Height);
					Console.Write(" ");
				}
				Console.WriteLine();
			}
		}

		private static void ClearHeightLimits(SandColumn[,] sand)
		{
			foreach (var sandColumn in sand)
			{
				sandColumn.HeightLimit = int.MaxValue;
			}
		}

		/// <summary>
		/// Starting from the origin point, sets the HeightLimit to be the offset in the pattern.
		/// </summary>
		/// <param name="sand">The sand field that the height limit pattern is applied to.</param>
		/// <param name="pattern">A 2D matrix of height patterns</param>
		/// <param name="origin">The point that the upper right (0,0) part of the pattern should be at</param>
		private static void ApplyHeightLimitPattern(SandColumn[,] sand, int[,] pattern, Point origin)
		{
			for (int x = 0; x < pattern.GetLength(0); x++)
			{
				for (int y = 0; y < pattern.GetLength(1); y++)
				{
					var targetX = x + origin.X;
					var targetY = y + origin.Y;
					//if the point we want to modify is inside the bounds of sand
					if (targetX >= 0 && targetX < sand.GetLength(0) && targetY >= 0 && targetY < sand.GetLength(1))
					{
						sand[targetX, targetY].HeightLimit = origin.Height + pattern[x, y];
					}
				}
			}
		}


	}
}