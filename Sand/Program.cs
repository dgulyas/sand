using System;
using System.Collections.Generic;
namespace Sand
{
	class Program
	{
		static void Main(string[] args)
		{
			Tests.Test4();
		}

		static Dictionary<int, List<Point>> CalcPointsAtDistance()
		{
			var pointsAtDistance = new Dictionary<int, List<Point>>();

			for (int i = 0; i < 300; i++)
			{
				pointsAtDistance.Add(i, new List<Point>());
			}

			var origin = new Point { X = 0, Y = 0 };
			for (int i = -100; i < 100; i++)
			{
				for (int j = -100; j < 100; j++)
				{
					var point = new Point { X = i, Y = j };
					pointsAtDistance[calcDistance(origin, point)].Add(point);
				}
			}

			return pointsAtDistance;
		}

		static int calcDistance(Point a, Point b)
		{
			return (int)Math.Round(Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2)));
		}
	}
}
