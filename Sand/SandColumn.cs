using System.Collections.Generic;

namespace Sand
{
	public class SandColumn
	{
		public int Height;
		public int HeightLimit;
		public List<SandColumn> Neighbours;
		public Point Location;

		public float Pressure => Height <= HeightLimit ? 0 : ((float)Height / (float)HeightLimit) - 1;
		//public float PressureWithHeight

		//what would be the new pressure if a level of sand was added or removed?
		public float IncreasedPressure => Height + 1 <= HeightLimit ? 0 : (((float)Height + 1) / (float)HeightLimit) - 1;
		public float DecreasedPressure => Height - 1 <= HeightLimit ? 0 : (((float)Height - 1) / (float)HeightLimit) - 1;

		public int IncreasedHeight => Height + 1;
		public int DecreasedHeight => Height - 1;

		public override string ToString()
		{
			return $"Height:{Height}, HightLimit:{HeightLimit}, Pressure:{Pressure}";
		}
	}
}
