using System;
using System.Collections.Generic;
using System.Linq;

namespace Sand
{
	public class Program2
	{
		//First number is height, second number is max height
		private static readonly SandColumn[,] m_sand = new SandColumn[Size,Size];
		private const int Size = 3;
		private static readonly Random Rnd = new Random();

		public static void Test2()
		{
			for (int i = 0; i < Size; i++)
			{
				for (int j = 0; j < Size; j++)
				{
					m_sand[i, j] = new SandColumn{HeightLimit = 10, Height = 20};
				}
			}

			m_sand[1, 1] = new SandColumn { HeightLimit = 10, Height = 70 };

			SettleColumn2(m_sand, new Point{X = 1, Y = 1});
		}

		//private static bool SettleColumn(SandColumn[,] sand, int x, int y)
		//{
		//	var columnChanged = false;

		//	var column = sand[x, y];

		//	var neighbours = new List<SandColumn>();

		//	if (x-1 >= 0)
		//	{
		//		neighbours.Add(sand[x-1, y]);
		//	}
		//	if (x+1 < sand.GetLength(0))
		//	{
		//		neighbours.Add(sand[x+1, y]);
		//	}
		//	if (y-1 >= 0)
		//	{
		//		neighbours.Add(sand[x, y-1]);
		//	}
		//	if (y+1 < sand.GetLength(1))
		//	{
		//		neighbours.Add(sand[x, y+1]);
		//	}

		//	while (column.Pressure > neighbours.Min(n => n.IncreasedPressure))
		//	{
		//		SandColumn lowestPressureNeighbour = null;
		//		foreach (var neighbour in neighbours)
		//		{

		//			//if we could send sand to neighbour, and it's the lowest pressure neighbour we've seen so far, save it
		//			if (column.Pressure > neighbour.IncreasedPressure &&
		//			    (lowestPressureNeighbour == null ||
		//			     neighbour.IncreasedPressure < lowestPressureNeighbour.IncreasedPressure))
		//			{
		//				lowestPressureNeighbour = neighbour;
		//			}
		//		}

		//		if (lowestPressureNeighbour == null)
		//		{
		//			throw new Exception("lowestPressureNeighbour shouldn't be null");
		//		}

		//		column.Height--;
		//		lowestPressureNeighbour.Height++;
		//		columnChanged = true;
		//	}
		//	return columnChanged;
		//}

		private static bool SettleColumn2(SandColumn[,] sand, Point location)
		{
			var column = sand[location.X, location.Y];
			var neighbours = GetNeighbours(sand, location);

			bool columnChanged;
			do
			{
				columnChanged = false;

				var lowerPressureNeighbours = new List<SandColumn>();
				var equalPressureLowerHeightNeighbours = new List<SandColumn>();

				foreach (var neighbour in neighbours)
				{
					if (column.DecreasedPressure >= neighbour.IncreasedPressure)
					{
						lowerPressureNeighbours.Add(neighbour);
					}
					else if (column.DecreasedHeight >= neighbour.IncreasedHeight)
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

			} while (columnChanged);


			//while (column.Pressure > neighbours.Min(n => n.IncreasedPressure))
			//{
			//	SandColumn lowestPressureNeighbour = null;
			//	foreach (var neighbour in neighbours)
			//	{

			//		//if we could send sand to neighbour, and it's the lowest pressure neighbour we've seen so far, save it
			//		if (column.Pressure > neighbour.IncreasedPressure &&
			//		    (lowestPressureNeighbour == null ||
			//		     neighbour.IncreasedPressure < lowestPressureNeighbour.IncreasedPressure))
			//		{
			//			lowestPressureNeighbour = neighbour;
			//		}
			//	}

			//	if (lowestPressureNeighbour == null)
			//	{
			//		throw new Exception("lowestPressureNeighbour shouldn't be null");
			//	}

			//	column.Height--;
			//	lowestPressureNeighbour.Height++;
			//	columnChanged = true;
			//}

			//Need to look at sand height if lowest pressure is zero.
			//Can't start moving sand into into other columns now, because
			//that lowers pressure in this column, maning that it shouldn't
			//have been moved into the last neighbour.

			return columnChanged;
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

		//Applies gravity to the sand.
		private void SettleMap(Tuple<int, int>[,] sand)
		{
			//coordinates of sand that's too high
			var highSand = new List<Tuple<int, int>>();
			foreach (var tuple in sand)
			{
				//Look at all "unfilled" lowest neighbours. Random tie breaker
				//else, if column is compressed, move particle to neighbour with lowest "compression".
			}
		}

	}
}
