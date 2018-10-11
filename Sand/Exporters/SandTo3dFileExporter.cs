using System.IO;
using System.Linq;
using System.Text;

namespace Sand.Exporters
{
	public class SandTo3dFileExporter
	{
		//https://3dviewer.net/
		//http://paulbourke.net/dataformats/obj/

		public static void SaveSandAs3dFile(SandColumn[,] sand, string outputFolder, string objFileName)
		{
			var sb = new StringBuilder("");
			for (int x = 0; x < sand.GetLength(0); x++)
			{
				for(int y = 0; y < sand.GetLength(1); y++)
				{
					var minHeight = GetMinHeight(sand[x, y]);

					for (int z = sand[x,y].Height; z >= minHeight; z--) {
						sb.Append(GetCubeDefinition(x,y,z));
					}
					sb.Append(GetCubeDefinition(x, y, 0));
				}
			}
			File.WriteAllText(Path.Combine(outputFolder, objFileName), sb.ToString());
		}

		private static int GetMinHeight(SandColumn column)
		{
			var minNeighbourHeight = column.Neighbours.Min(n => n.Height);
			return column.Height < minNeighbourHeight ? column.Height : minNeighbourHeight;
		}

		private static string GetCubeDefinition(int X, int Y, int Z)
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
			sb.Append($"v {X} {Y + 1} {Z + 1}{newLine}");
			sb.Append($"v {X} {Y} {Z + 1}{newLine}");
			sb.Append($"v {X + 1} {Y} {Z + 1}{newLine}");
			sb.Append($"v {X + 1} {Y + 1} {Z + 1}{newLine}");
			sb.Append($"v {X} {Y + 1} {Z}{newLine}");
			sb.Append($"v {X} {Y} {Z}{newLine}");
			sb.Append($"v {X + 1} {Y} {Z}{newLine}");
			sb.Append($"v {X + 1} {Y + 1} {Z}{newLine}");

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
