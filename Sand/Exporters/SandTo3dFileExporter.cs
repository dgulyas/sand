using System.IO;
using System.Text;

namespace Sand.Exporters
{
	public class SandTo3dFileExporter
	{
		//https://3dviewer.net/
		//http://paulbourke.net/dataformats/obj/
		//https://slimdx.org/tutorials/SimpleTriangle.php
		//WPF 3d ViewPort


		//For every column create a 3d box at the columns location with dimensions 1x1x(height+1)
		public static void SaveSandAs3dFile(SandColumn[,] sand, string outputFolder, string objFileName)
		{
			var sb = new StringBuilder("");
			for (int x = 0; x < sand.GetLength(0); x++)
			{
				for(int y = 0; y < sand.GetLength(1); y++)
				{
					sb.Append(GetCubeDefinition(x, y, sand[x, y].Height+1));
				}
			}
			//File.WriteAllText(Path.Combine(outputFolder, objFileName), sb.ToString());
		}

		private static string GetCubeDefinition(int x, int y, int zTop)
		{
			/* modified version of cube found at http://paulbourke.net/dataformats/obj/
			v 0 1 1
			v 0 0 1
			v 1 0 1
			v 1 1 1
			v 0 1 0
			v 0 0 0
			v 1 0 0
			v 1 1 0
			f -1 -2 -3 -4
			f -8 -7 -6 -5
			f -4 -3 -7 -8
			f -5 -1 -4 -8
			f -5 -6 -2 -1
			f -2 -6 -7 -3
			*/
			var newLine = System.Environment.NewLine;

			var sb = new StringBuilder("");
			sb.Append($"v {x}     {y + 1} {zTop}{newLine}");
			sb.Append($"v {x}     {y}     {zTop}{newLine}");
			sb.Append($"v {x + 1} {y}     {zTop}{newLine}");
			sb.Append($"v {x + 1} {y + 1} {zTop}{newLine}");
			sb.Append($"v {x}     {y + 1} {0}{newLine}");
			sb.Append($"v {x}     {y}     {0}{newLine}");
			sb.Append($"v {x + 1} {y}     {0}{newLine}");
			sb.Append($"v {x + 1} {y + 1} {0}{newLine}");

			sb.Append($"f -1 -2 -3 -4{newLine}");
			sb.Append($"f -8 -7 -6 -5{newLine}");
			sb.Append($"f -4 -3 -7 -8{newLine}");
			sb.Append($"f -5 -1 -4 -8{newLine}");
			sb.Append($"f -5 -6 -2 -1{newLine}");
			sb.Append($"f -2 -6 -7 -3{newLine}");

			return sb.ToString();
		}

	}
}
