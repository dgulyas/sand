using System.Collections.Generic;

namespace Sand
{
	public class SandColumn
	{
		private int m_height;
		public int Height
		{
			get => m_height;
			set => m_height = value < 0 ? 0 : value;
		}

		private int m_heightLimit;
		public int HeightLimit {
			get => m_heightLimit;
			set => m_heightLimit = value < 0 ? 0 : value;
		}

		public List<SandColumn> Neighbours;
		public Point Location;

		public float Pressure => Height <= HeightLimit ? 0 : ((float)Height / (float)HeightLimit) - 1;

		//what would be the new pressure if a level of sand was added or removed?
		public float IncreasedPressure => Height + 1 <= HeightLimit ? 0 : (((float)Height + 1) / (float)HeightLimit) - 1;
		public float DecreasedPressure => Height - 1 <= HeightLimit ? 0 : (((float)Height - 1) / (float)HeightLimit) - 1;

		public int IncreasedHeight => Height + 1;
		public int DecreasedHeight => Height - 1;

		public int NumExcessSand => Height > HeightLimit ? Height - HeightLimit : 0;
		public int NumExcessRoom => HeightLimit > Height ? HeightLimit - Height : 0;

		public override string ToString()
		{
			return $"Height:{Height}, HeightLimit:{HeightLimit}, Pressure:{Pressure}";
		}
	}
}
