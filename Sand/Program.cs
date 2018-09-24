using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace Sand
{
	class Program
	{
		static void Main(string[] args)
		{
			//Test1();
		}

		static void Test1()
		{
			var matrix = GetTestMatrix();

			SaveMatrixAsPng(matrix, "test");

			int[,] array = new int[11, 11];

			var pointsAtDistance = CalcPointsAtDistance();
			foreach (var key in pointsAtDistance.Keys)
			{
				Console.WriteLine($"Distance {key}");
				foreach (var point in pointsAtDistance[key])
				{
					Console.Write($"({point.X},{point.Y}) ");
				}
				Console.WriteLine();
			}

			Console.ReadLine();
		}

		static void Poke(int[,] sand, Point coords)
		{
			//this should simulate a ball being placed in the sand
			for (int xDelt = -5; xDelt <= 5; xDelt++)
			{

			}
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

		//origin of the matrix is in the upper right
		//matrix coords are in [x,y] order
		static void SaveMatrixAsPng(int[,] matrix, string bmpName)
		{
			var width = matrix.GetLength(0);
			var height = matrix.GetLength(1);
			var bitmap = new Bitmap(width, height);

			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					var alpha = matrix[x, y] + 30 > 255 ? 255 : matrix[x, y] + 30;
					bitmap.SetPixel(x, y, Color.FromArgb(alpha, alpha, alpha));
				}
			}

			bitmap.Save($"C:\\work\\repos\\Sand\\Sand\\{bmpName}.png", ImageFormat.Png);
		}

		static int[,] GetTestMatrix()
		{
			var matrix = new int[200, 200];

			var origin = new Point { X = 50, Y = 0 };
			for (int i = 0; i < 200; i++)
			{
				for (int j = 0; j < 200; j++)
				{
					var point = new Point { X = i, Y = j };
					var shade = calcDistance(origin, point);
					matrix[point.X, point.Y] = shade;
				}
			}

			return matrix;
		}

		static int calcDistance(Point a, Point b)
		{
			return (int)Math.Round(Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2)));
		}
	}
}
