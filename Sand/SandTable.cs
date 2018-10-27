using System;
using System.Collections.Generic;
using System.Linq;

namespace Sand
{
	public class SandTable
	{
		private static readonly Random Rnd = new Random();

		public static SandColumn[,] CreateSandTable(int height, int width, int defaultSandHeight, int defaultHeightLimit = int.MaxValue)
		{
			var sand = new SandColumn[width,height];

			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					sand[x, y] = new SandColumn
					{
						HeightLimit = defaultHeightLimit,
						Height = defaultSandHeight,
						Location = new Point { X = x, Y = y }
					};
				}
			}

			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					sand[x, y].Neighbours = GetNeighbours(sand, new Point {X = x, Y = y});
					Shuffle(sand[x, y].Neighbours);
				}
			}

			return sand;
		}

		private static bool SettleMap(SandColumn[,] sand)
		{
			var sandMoved = false;
			for(int x = 0; x < sand.GetLength(0); x++)
			{
				for(int y = 0; y < sand.GetLength(1); y++)
				{
					var columnSandMoved = SettleColumn(sand[x,y]);
					if (columnSandMoved)
					{
						sandMoved = true;
					}
				}
			}
			return sandMoved;
		}

		public static bool SettleMapRandom(SandColumn[,] sand)
		{
			var shuffledColumns = new List<SandColumn>();
			foreach (var column in sand)
			{
				shuffledColumns.Add(column);
			}
			Shuffle(shuffledColumns);

			var sandMoved = false;
			foreach (var sandColumn in shuffledColumns)
			{
				var columnSandMoved = SettleColumn(sandColumn);
				if (columnSandMoved)
				{
					sandMoved = true;
				}
			}
			return sandMoved;
		}

		public static void Shuffle<T>(List<T> list)
		{
			Random rng = new Random();
			int n = list.Count;
			while (n > 1)
			{
				n--;
				int k = rng.Next(n + 1);
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}

		private static bool SettleColumn(SandColumn column)
		{
			var columnChangedOnce = false; //if the column changed at all

			bool columnChanged; //if the column changed in this iteration
			do
			{
				columnChanged = false;

				var lowerPressureNeighbours = new List<SandColumn>();
				var equalPressureLowerHeightNeighbours = new List<SandColumn>();

				foreach (var neighbour in column.Neighbours)
				{
					if (column.Pressure > 0 && column.DecreasedPressure >= neighbour.IncreasedPressure)
					{
						lowerPressureNeighbours.Add(neighbour);
					}
					else if (column.Pressure <= 0 && neighbour.Pressure <= 0 && column.DecreasedHeight >= neighbour.IncreasedHeight)
					{
						equalPressureLowerHeightNeighbours.Add(neighbour);
					}
				}

				if (lowerPressureNeighbours.Count > 0)
				{
					var lowestPressure = lowerPressureNeighbours.Min(n => n.IncreasedPressure);
					lowerPressureNeighbours.RemoveAll(n => n.Pressure > lowestPressure);

					SandColumn chosenNeighbour = lowerPressureNeighbours[Rnd.Next(lowerPressureNeighbours.Count)];

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

		//2 passes over sand
		//1st pass, move all sand that overflows the maxHeight to the closest column that has room
		//2nd pass, apply gravity
		//Assumes that it's possible to reduce all columns to 0 pressure.
		public static void SettleMapTwoPass(SandColumn[,] sand)
		{
			foreach (var column in sand)
			{
				SettlePressuredSand(column);
			}

			for (int i = 0; i < 30; i++)
			{
				foreach (var column in sand)
				{
					ApplyGravity(column);
				}
			}
		}

		private static void SettlePressuredSand(SandColumn column)
		{
			//This is a basic breadth first search using a queue
			if (column.Pressure <= 0)
			{
				return;
			}

			var neighbourQueue = new Queue<SandColumn>();
			var visitedNeighbours = new List<SandColumn>();
			neighbourQueue.Enqueue(column);
			while (column.Pressure > 0)
			{
				var currentNeighbour = neighbourQueue.Dequeue();
				visitedNeighbours.Add(currentNeighbour);
				var emptySpace = currentNeighbour.NumExcessRoom;
				if (emptySpace > 0)
				{
					MoveSand(column, currentNeighbour, Math.Min(column.NumExcessSand, emptySpace));
				}

				foreach (var currentNeighbourNeighbour in currentNeighbour.Neighbours)
				{
					if (!visitedNeighbours.Contains(currentNeighbourNeighbour))
					{
						neighbourQueue.Enqueue(currentNeighbourNeighbour);
					}
				}
			}
		}

		private static void ApplyGravity(SandColumn column)
		{
			var noPressureLowerNeighbours = new Queue<SandColumn>();
			Shuffle(column.Neighbours);
			foreach (var neighbour in column.Neighbours)
			{
				noPressureLowerNeighbours.Enqueue(neighbour);
			}

			while (noPressureLowerNeighbours.Count > 0)
			{
				var neighbour = noPressureLowerNeighbours.Dequeue();

				if (neighbour.Height + 1 < column.Height && neighbour.NumExcessRoom > 0)
				{
					MoveSand(column, neighbour);
					if (neighbour.Height + 1 < column.Height && neighbour.NumExcessRoom > 0)
					{
						noPressureLowerNeighbours.Enqueue(neighbour);
					}
				}
			}
		}

		private static void MoveSand(SandColumn source, SandColumn dest, int amount = 1)
		{
			source.Height -= amount;
			dest.Height += amount;
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

		public static void ClearHeightLimits(SandColumn[,] sand)
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
		public static void ApplyHeightLimitPattern(SandColumn[,] sand, int?[,] pattern, Point origin)
		{
			for (int x = 0; x < pattern.GetLength(0); x++)
			{
				for (int y = 0; y < pattern.GetLength(1); y++)
				{
					if (pattern[x, y] != null)
					{
						var targetX = x + origin.X;
						var targetY = y + origin.Y;
						//if the point we want to modify is inside the bounds of sand
						if (targetX >= 0 && targetX < sand.GetLength(0) && targetY >= 0 && targetY < sand.GetLength(1))
						{
							sand[targetX, targetY].HeightLimit = origin.Height + pattern[x, y].Value;
						}
					}
				}
			}
		}


	}
}
