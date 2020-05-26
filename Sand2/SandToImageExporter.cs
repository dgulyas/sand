using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


namespace Sand2
{
	public class SandToImageExporter
	{
		private static int[,]  hexPattern = new int[,]
		{
			{0, 0, 1, 1, 0, 0},
			{0, 1, 1, 1, 1, 0},
			{1, 1, 1, 1, 1, 1},
			{1, 1, 1, 1, 1, 1},
			{0, 1, 1, 1, 1, 0},
			{0, 0, 1, 1, 0, 0}
		};

		public static void SaveMatrixAsImage(int[,] matrix, string outputFolder, string bmpName)
		{
			var (lowestSand, highestSand) = GetLowestAndHighestValue(matrix);

			ScaleMatrixValuesToFitGrayScale(matrix, lowestSand, highestSand);

			var bitmap = new Bitmap(100, 100);

			DrawMatrix(matrix, bitmap);

			bitmap.Save("C:\\Users\\david\\Desktop\\out\\2\\testG.bmp", ImageFormat.Bmp);
		}

		private static void DrawMatrix(int[,] matrix, Bitmap bitmap)
		{
			int matrixHeight = matrix.GetLength(0);
			int matrixWidth = matrix.GetLength(1);
			int patternHeight = hexPattern.GetLength(0);
			int patternWidth = hexPattern.GetLength(1);

			int xOffset = 0;

			for (int y = 0; y < matrixHeight; y++)
			{
				for (int x = 0; x < matrixWidth; x++)
				{
					if (matrix[y,x] != 0)
					{
						DrawHex(bitmap, x * patternWidth + xOffset, y * (patternHeight - 2), matrix[y,x]); //magic number
					}
				}
				xOffset += 3; //magic number
			}
		}

		private static void DrawHex(Bitmap bitmap, int x, int y, int grayscale)
		{
			int patternHeight = hexPattern.GetLength(0);
			int patternWidth = hexPattern.GetLength(1);

			for (int k = 0; k < patternHeight; k++)
			{
				for (int l = 0; l < patternWidth; l++)
				{
					if (hexPattern[k, l] == 1)
					{
						bitmap.SetPixel(x + l, y + k, Color.FromArgb(grayscale, grayscale, grayscale));
					}
				}
			}
		}

		private static (int, int) GetLowestAndHighestValue(int[,] matrix)
		{
			var lowest = int.MaxValue;
			var highest = int.MinValue;

			foreach (var i in matrix)
			{
				if (i != 0 && i > highest)
				{
					highest = i;
				}

				if (i != 0 && i < lowest)
				{
					lowest = i;
				}
			}

			return (lowest, highest);
		}

		private static void ScaleMatrixValuesToFitGrayScale(int[,] matrix, int lowest, int highest)
		{
			//gray scale is 0-255. Ignore 0 because that's a special value indicating there's no sand there.

			float maxGreyscale = 255;
			float minGreyScale = 1;

			float oldRange = highest - lowest;
			float newRange = maxGreyscale - minGreyScale;

			for (int x = 0; x < matrix.GetLength(0); x++)
			{
				for (int y = 0; y < matrix.GetLength(1); y++)
				{
					if (matrix[x, y] != 0)
					{
						if (Math.Abs(oldRange) < 0.001) //if the height matrix is all one height
						{
							matrix[x, y] = 100;
						}
						else
						{
							//This moves a number from one range into another, so that all numbers are between 0-255, which is the range of
							//greyscale for images.
							//https://stackoverflow.com/questions/929103/convert-a-number-range-to-another-range-maintaining-ratio
							matrix[x, y] =
								(int) ((matrix[x, y] - lowest) * newRange / oldRange + minGreyScale);
						}
					}
				}
			}


		}

	}
}
