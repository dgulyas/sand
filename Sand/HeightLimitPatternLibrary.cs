using System;

namespace Sand
{
	//A pattern is a set of heightLimit diffs that are laid on a SandTable at a 3d point.
	//Nulls mean that that column isn't affected.

	public static class HeightLimitPatternLibrary
	{
		public static int?[,] SmallCube =
		{
			{0, 0, 0, 0},
			{0, 0, 0, 0},
			{0, 0, 0, 0},
			{0, 0, 0, 0}
		};

		public static int?[,] SmallPyramid =
		{
			{0,  0,  0,  0, 0},
			{0, -2, -2, -2, 0},
			{0, -2, -4, -2, 0},
			{0, -2, -2, -2, 0},
			{0,  0,  0,  0, 0},
		};


		public static int?[,] Sphere(int diameter)
		{
			//Computes a pattern for any sized sphere
			//sqrt(radius^2-x^2-y^2) is the height of the sphere over specific coordinates.
			//It's the pythagorean therom twice
			//https://www.wolframalpha.com/input/?i=sqrt(16-x%5E2-y%5E2)

			var radius = ((float) diameter) / 2;

			var pattern = new int?[diameter,diameter];
			for (float i = 0; i < diameter; i++)
			{
				for (float j = 0; j < diameter; j++)
				{
					var x = i - radius; //center sphere in middle of pattern
					var y = j - radius;

					if (Math.Sqrt(x * x + y * y) > radius) //point outside sphere
					{
						pattern[(int)i, (int)j] = null;
					}
					else
					{
						var heightOfSphere = (int) Math.Round(Math.Sqrt((double) (radius * radius - x * x - y * y)));
						pattern[(int) i, (int) j] = heightOfSphere * -1;
					}
				}
			}

			return pattern;
		}

	}
}
