using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Sand
{
	//takes a matrix of SandColumns and outputs a png
	public class SandToImageOutputter
	{
		//origin of the matrix is in the upper right
		//matrix coords are in [x,y] order
		public static void SaveMatrixAsImage(SandColumn[,] sand, string outputFolder, string bmpName)
		{
			var matrixWidth = sand.GetLength(0);
			var matrixHeight = sand.GetLength(1);

			var heightArray = new int[matrixHeight, matrixWidth];
			for (int x = 0; x < matrixWidth; x++)
			{
				for (int y = 0; y < matrixHeight; y++)
				{
					heightArray[x, y] = sand[x, y].Height;
				}
			}

			var (highestHeight, lowestHeight) = GetHighestAndLowestHeight(heightArray);
			CompressMatrixToFitImage(heightArray, highestHeight, lowestHeight);

			var bitmap = new Bitmap(matrixWidth, matrixHeight);


			for (int x = 0; x < matrixWidth; x++)
			{
				for (int y = 0; y < matrixHeight; y++)
				{
					var alpha = heightArray[x, y];
					bitmap.SetPixel(x, y, Color.FromArgb(alpha, alpha, alpha));
				}
			}

			bitmap.Save(Path.Combine(outputFolder, bmpName), ImageFormat.Bmp);
		}

		public static (int, int) GetHighestAndLowestHeight(int[,] heightMatrix)
		{
			var matrixWidth = heightMatrix.GetLength(0);
			var matrixHeight = heightMatrix.GetLength(1);

			var highestHeight = 0;
			var lowestHeight = int.MaxValue;
			for (int x = 0; x < matrixWidth; x++)
			{
				for (int y = 0; y < matrixHeight; y++)
				{
					var height = heightMatrix[x, y];
					if (height > highestHeight)
					{
						highestHeight = height;
					}

					if (height < lowestHeight)
					{
						lowestHeight = height;
					}

					if (height < 0)
					{
						Console.WriteLine($"Negative Height at ({x},{y}): {height}");
					}
				}
			}

			return (highestHeight, lowestHeight);
		}

		public static void CompressMatrixToFitImage(int[,] heightMatrix, int highestHeight, int lowestHeight)
		{
			float maxGreyscale = 255;
			float minGreyScale = 0;

			float oldRange = highestHeight - lowestHeight;
			float newRange = maxGreyscale - minGreyScale;

			for (int x = 0; x < heightMatrix.GetLength(0); x++)
			{
				for (int y = 0; y < heightMatrix.GetLength(1); y++)
				{
					if (Math.Abs(oldRange) < 0.001) //if the height matrix is all one height
					{
						heightMatrix[x, y] = 100;
					}
					else
					{
						//This moves a number from one range into another, so that all numbers are between 0-255, which is the range of
						//greyscale for images.
						//https://stackoverflow.com/questions/929103/convert-a-number-range-to-another-range-maintaining-ratio
						heightMatrix[x, y] =
							(int)((heightMatrix[x, y] - lowestHeight) * newRange / oldRange + minGreyScale);
					}
				}
			}

		}

	}
}
