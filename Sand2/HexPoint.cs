using System.Collections.Generic;

namespace Sand2
{
	//Most of this is from https://www.redblobgames.com/grids/hexagons

	public class HexPoint
	{ //A point in a hex grid has 3 coordinates
		public readonly int Q;
		public readonly int R;
		public readonly int S;

		public readonly int Hashcode;

		public HexPoint(int q, int r, int s)
		{
			Q = q;
			R = r;
			S = s;

			Hashcode = q + r * 3001 + s * 100003;
		}

		public static List<HexPoint> HexDirection = new List<HexPoint>
		{ //adding one of these to the current point will move to a neighbour.
			new HexPoint(0,-1,1),
			new HexPoint(1,-1,0),
			new HexPoint(1,0,-1),
			new HexPoint(0,1,-1),
			new HexPoint(-1,1,0),
			new HexPoint(-1,0,1)
		};

		public static HexPoint operator +(HexPoint a, HexPoint b) => new HexPoint(a.Q + b.Q, a.R + b.R, a.S + b.S);

		//Multiplies the coordinates of a point by integer i
		public static HexPoint HexScale(HexPoint p, int i)
		{
			return new HexPoint(p.Q * i, p.R * i, p.S *i);
		}

		public bool Equals(HexPoint p)
		{
			return Q == p.Q && R == p.R && S == p.S;
		}

		public override int GetHashCode()
		{
			return Hashcode;
		}

		public override bool Equals(object obj)
		{
			return obj is HexPoint && Equals((HexPoint)obj);
		}

		//returns a list of hex coords that form a ring at radius around p
		public static List<HexPoint> HexRing(HexPoint p, int radius)
		{
			if(radius < 1) return new List<HexPoint>();

			var hexInRing = new List<HexPoint>();
			var currPoint = p + HexScale(HexDirection[4], radius);

			for (int i = 0; i < 6; i++)
			{
				for (int j = 0; j < radius; j++)
				{
					hexInRing.Add(currPoint);
					currPoint += HexDirection[i];
				}
			}

			return hexInRing;
		}

		public static List<HexPoint> GetNeighbours(HexPoint p)
		{	//These are all the hexes touching p
			//return HexRing(p, 1);
			//Lets do something faster then using the HexRing function

			var neighbours = new List<HexPoint>(6);
			foreach (var direction in HexDirection)
			{
				neighbours.Add(p + direction);
			}

			return neighbours;
		}

		public override string ToString()
		{
			return $"{Q} {R} {S}";
		}


	}
}
