using System;

namespace Sand
{
	public class Point
	{
		public int X;
		public int Y;
		public int Height;


		public override string ToString()
		{
			return $"X:{X}, Y:{Y}, Height:{Height}";
		}

		public override bool Equals(Object obj)
		{
			if (!(obj is Point item)) return false;

			return item.X == X && item.Y == Y && item.Height == Height;
		}

		public override int GetHashCode()
		{
			return X * Y + (Height % 102533);
		}

		public static Point operator +(Point p1, Point p2)
		{
			return new Point{X = p1.X+p2.X, Y = p1.Y + p2.Y, Height = p1.Height + p2.Height};
		}

	}
}
