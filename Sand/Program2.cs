using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sand
{
	class Program2
	{
		//First number is height, second number is max height
		private readonly Tuple<int, int>[,] sand = new Tuple<int, int>[Size,Size];
		private const int Size = 100;

		public void Test2()
		{
			for (int i = 0; i < Size; i++)
			{
				for (int j = 0; j < Size; j++)
				{
					sand[i, j] = new Tuple<int, int>(50, Int32.MaxValue);
				}
			}


		}

		//Applies gravity to the sand.
		private void SettleMap(Tuple<int, int>[,] sand)
		{
			//coordinates of sand that's too high
			var highSand = new List<Tuple<int, int>>();
			foreach (var tuple in sand)
			{
				//Look at all "unfilled" lower neighbours. Pick one that's lowest. Random tie breaker
				//else, if column is compressed, move particle to neighbour with lowest "compression".
			}
		}



		private void Displace(Tuple<int, int>[,] sand)
		{
			//coordinates of sand that's too high
			var highSand = new List<Tuple<int, int>>();
			foreach (var tuple in sand)
			{

			}
		}

	}



}
