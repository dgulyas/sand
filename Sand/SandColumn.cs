namespace Sand
{
	public class SandColumn
	{
		public int Height;
		public int HeightLimit;

		public float Pressure => Height <= HeightLimit ? 0 : ((float)Height / (float)HeightLimit) - 1;

		//what would be the new pressure if a level of sand was added?
		public float IncreasedPressure => Height + 1 <= HeightLimit ? 0 : (((float)Height + 1) / (float)HeightLimit) - 1;

		public override string ToString()
		{
			return $"Height:{Height}, HightLimit:{HeightLimit}, Pressure:{Pressure}";
		}
	}
}
