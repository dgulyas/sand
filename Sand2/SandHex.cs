using System;
using System.Collections.Generic;
using System.Linq;

namespace Sand2
{
	public class SandHex
	{
		public readonly HexPoint Coord;

		//height of sand in the column
		private int m_height;
		public int Height
		{
			get => m_height;
			set => m_height = value < 0 ? 0 : value;
		}

		//How high can the sand go in the column? This simulates something pressing into the sand.
		private int m_heightLimit;
		public int HeightLimit
		{
			get => m_heightLimit;
			set => m_heightLimit = value < 0 ? 0 : value;
		}

		public float Pressure => Height <= HeightLimit ? 0 : ((float)Height / (float)HeightLimit) - 1;

		//what would be the new pressure if a level of sand was added or removed?
		public float IncreasedPressure => Height + 1 <= HeightLimit ? 0 : (((float)Height + 1) / (float)HeightLimit) - 1;
		public float DecreasedPressure => Height - 1 <= HeightLimit ? 0 : (((float)Height - 1) / (float)HeightLimit) - 1;

		public int IncreasedHeight => Height + 1;
		public int DecreasedHeight => Height - 1;

		public int NumExcessSand => Height > HeightLimit ? Height - HeightLimit : 0;
		public int NumExcessRoom => HeightLimit > Height ? HeightLimit - Height : 0;


		//Acts as the 'visited' flag for BFS or DFS
		//Each search through the table gets a unique int.
		//When the search gets to a Hex it sets the Visited value to that unique int.
		//This avoids setting a bool value to..
		//This might not be needed.
		//public int Visited;

	}
}
