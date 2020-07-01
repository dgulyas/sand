using System.Collections.Generic;
using static System.Math;

namespace Sand2
{
	public static class HexHelpers
	{
		//A bunch of this is from //https://www.redblobgames.com/grids/hexagons

		public static List<HexPoint> GenHexMap(int radius)
		{
			var hexes = new List<HexPoint>();

			for (int i = 0 - radius; i <= radius; i++)
			{
				for (int j = Max(0 - radius, 0 - i - radius); j <= Min(radius, 0 - i + radius); j++)
				{
					var k = 0 - i - j;
					hexes.Add(new HexPoint(i, j, k));
					//Console.WriteLine($"{i} {j} {k} sum:{i + j + k}");
				}
			}

			return hexes;

		}


	}
}
