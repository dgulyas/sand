using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using static System.Math;

namespace Sand2
{
	public class SandTable
	{
		public Dictionary<HexPoint, SandHex> sands;

		public SandTable(int radius, int defaultSandHeight, int defaultHeightLimit = int.MaxValue)
		{
			sands = new Dictionary<HexPoint, SandHex>();

			foreach (var p in HexHelpers.GenHexMap(radius))
			{
				sands.Add(p, new SandHex{Height = 10, HeightLimit = int.MaxValue});
			}

		}

		public bool PointExists(HexPoint p)
		{
			return sands.Keys.Contains(p);
		}

		public void ApplyGravity()
		{
			bool sandMoved;
			do
			{
				sandMoved = false;
				foreach (var sand in sands)
				{
					var neighbours = new Queue<HexPoint>(HexPoint.HexRing(sand.Key, 1));
					while (neighbours.Count > 0)
					{
						var neighbour = neighbours.Dequeue();
						if (!PointExists(neighbour) || sands[neighbour].NumExcessRoom < 1 ||
						    sands[neighbour].Height + 1 >= sand.Value.Height)
						{
							continue;
						}

						sand.Value.Height--;
						sands[neighbour].Height++;
						neighbours.Enqueue(neighbour);
						sandMoved = true;
					}
				}
			} while (sandMoved);
		}

		public List<string> Print()
		{
			var matrix = ToMatrix();

			var output = new List<string>();
			var numRows = matrix.GetLength(0);
			var numCols = matrix.GetLength(1);
			for (int i = 0; i < numRows; i++)
			{
				var sb = new StringBuilder(new string(' ', i*2));
				for (int j = 0; j < numCols; j++)
				{
					sb.Append($"{matrix[i,j],2}  ");
				}
				output.Add(sb.ToString());
			}

			return output;
		}

		public int[,] ToMatrix()
		{
			int minq = int.MaxValue;
			int maxq = int.MinValue;
			int minr = int.MaxValue;
			int maxr = int.MinValue;

			foreach (var sandPoint in sands.Keys)
			{
				if (sandPoint.Q < minq) minq = sandPoint.Q;
				if (sandPoint.Q > maxq) maxq = sandPoint.Q;
				if (sandPoint.R < minr) minr = sandPoint.R;
				if (sandPoint.R > maxr) maxr = sandPoint.R;
			}

			var matrix = new int[maxr - minr + 1, maxq - minq + 1];

			foreach (var sand in sands)
			{
				matrix[sand.Key.R - minr, sand.Key.Q - minq] = sand.Value.Height;
			}

			return matrix;
		}
	}
}
