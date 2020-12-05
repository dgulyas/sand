using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;


namespace Sand2
{
	public class SandToImageExporter
	{
		//Specifies which pixles should be colored when drawing a hex.
		private static readonly int[,]  HexPattern =
		{
			{0, 0, 1, 1, 0, 0},
			{0, 1, 1, 1, 1, 0},
			{1, 1, 1, 1, 1, 1},
			{1, 1, 1, 1, 1, 1},
			{0, 1, 1, 1, 1, 0},
			{0, 0, 1, 1, 0, 0}
		};

		private static int PatternHeight = HexPattern.GetLength(0);
		private static int PatternWidth = HexPattern.GetLength(1);

		public static void SaveMatrixAsImage(int[,] matrix, string outputFolder, string bmpName, bool scaleMatrix = true)
		{
			if (scaleMatrix)
			{
				ScaleMatrixValuesToFitGrayScale(matrix);
			}

			int matrixHeight = matrix.GetLength(0);
			int matrixWidth = matrix.GetLength(1);
			var imgHeight = matrixHeight * 4 + 2; //magic number
			var imgWidth = matrixWidth * 6; //magic number
			var bitmap = new Bitmap(imgWidth, imgHeight);
			ClearBitmap(bitmap, Color.White);

			DrawMatrix(matrix, bitmap);

			bitmap.Save(Path.Combine(outputFolder, bmpName), ImageFormat.Bmp);
		}

		private static void ClearBitmap(Bitmap bitmap, Color color)
		{
			for (int j = 0; j < bitmap.Height; j++)
			{
				for (int i = 0; i < bitmap.Width; i++)
				{
					bitmap.SetPixel(i,j,color);
				}
			}
		}

		private static void DrawMatrix(int[,] matrix, Bitmap bitmap)
		{
			int matrixHeight = matrix.GetLength(0);
			int matrixWidth = matrix.GetLength(1);

			var xOffset = (int)((matrixHeight - 1) * -1.5); //magic number

			for (int y = 0; y < matrixHeight; y++)
			{
				for (int x = 0; x < matrixWidth; x++)
				{
					if (matrix[y,x] != 0)
					{
						DrawHex(bitmap, x * PatternWidth + xOffset, y * (PatternHeight - 2), matrix[y,x]); //magic number
					}
				}
				xOffset += 3; //magic number
			}
		}

		private static void DrawHex(Bitmap bitmap, int x, int y, int grayscale)
		{
			for (int k = 0; k < PatternHeight; k++)
			{
				for (int l = 0; l < PatternWidth; l++)
				{
					if (HexPattern[k, l] == 1)
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
				if (i != 0 && i > highest) //magic number
				{
					highest = i;
				}

				if (i != 0 && i < lowest) //magic number
				{
					lowest = i;
				}
			}

			return (lowest, highest);
		}

		private static void ScaleMatrixValuesToFitGrayScale(int[,] matrix)
		{
			//gray scale is 0-255. We want the highest sand to be printed as black(?) and the lowest sand to be white(?).
			//Ignore 0 because that's a special value indicating there's no sand there.

			float maxGreyscale = 255;
			float minGreyScale = 1;

			var (lowest, highest) = GetLowestAndHighestValue(matrix);

			float oldRange = highest - lowest;
			float newRange = maxGreyscale - minGreyScale;

			for (int x = 0; x < matrix.GetLength(0); x++)
			{
				for (int y = 0; y < matrix.GetLength(1); y++)
				{
					if (matrix[x, y] != 0) //magic number
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
