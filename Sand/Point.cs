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

	}
}
