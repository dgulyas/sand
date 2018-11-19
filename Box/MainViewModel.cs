//This is based off an example project that came with Helix Toolkit
//Found at https://github.com/helix-toolkit/helix-toolkit/tree/develop/Source/Examples/WPF/SimpleDemo

using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;

using HelixToolkit.Wpf;


using Sand;

namespace Box
{
	public class MainViewModel
	{
		private readonly Material topMaterial = MaterialHelper.CreateMaterial(Colors.HotPink);
		private readonly Material bottomMaterial = MaterialHelper.CreateMaterial(Colors.HotPink);
		private readonly Material boxTopMaterial = MaterialHelper.CreateMaterial(Colors.Brown);
		private readonly Material boxBottomMaterial = MaterialHelper.CreateMaterial(Colors.HotPink);
		private readonly Material ballMaterial = MaterialHelper.CreateMaterial(Brushes.Red, 500, 255, true);

		//private Material sand = MaterialHelper.CreateImageMaterial("sand.jpg", 1);

		public MainViewModel()
		{
			Model = m_modelGroup;
			topMaterial = MaterialHelper.CreateImageMaterial("maglev.png", 1);

		}

		internal void AddMeshHeights(SandColumn[,] sand)
		{
			var points = new List<Point3D>();
			var texCoords = new List<System.Windows.Point>();
			var indices = new List<int>();
			var width = sand.GetLength(0);
			var height = sand.GetLength(1);

			for (var x = 0; x < width; x++)
			{
				for (var y = 0; y < height; y++)
				{
					var column = sand[x, y];
					points.Add(new Point3D(x, y, column.Height));
					texCoords.Add(new System.Windows.Point(x / (double)width, (height - y) / (double)height));

				}
			}

			for (var x = 0; x < width - 1; x++)
			{
				for (var y = 0; y < height; y++)
				{
					if (y < height - 1)
					{
						var p1 = (y * width) + x;
						var p2 = (y * width) + (x + 1);
						var p3 = ((y + 1) * width) + x;
						indices.Add(p3);
						indices.Add(p2);
						indices.Add(p1);
					}
					if (y > 0)
					{
						var p1 = (y * width) + x;
						var p2 = (y * width) + (x + 1);
						var p3 = ((y - 1) * width) + (x + 1);
						indices.Add(p1);
						indices.Add(p2);
						indices.Add(p3);
					}
				}
			}
			var mesh = new Mesh3D(points, texCoords, indices);
			m_modelGroup.Children.Add(new GeometryModel3D { Geometry = mesh.ToMeshGeometry3D(true), Material = topMaterial, BackMaterial = bottomMaterial });
		}


		internal void AddContainer(int width, int height, int depth)
		{
			width -= 1;
			height -= 1;
			var meshBuilder = new MeshBuilder();
			var corners = new[] {
				new Point3D(0, 0, depth),
				new Point3D(width, 0, depth),
				new Point3D(width, height , depth),
				new Point3D(0, height, depth),
				new Point3D(0, 0, 0),
				new Point3D(width, 0, 0),
				new Point3D(width, height, 0),
				new Point3D(0, height, 0),
					};
			meshBuilder.AddQuad(corners[0], corners[1], corners[5], corners[4]);
			meshBuilder.AddQuad(corners[1], corners[2], corners[6], corners[5]);
			meshBuilder.AddQuad(corners[2], corners[3], corners[7], corners[6]);
			meshBuilder.AddQuad(corners[3], corners[0], corners[4], corners[7]);
			meshBuilder.AddQuad(corners[4], corners[5], corners[6], corners[7]);
			m_modelGroup.Children.Add(new GeometryModel3D { Geometry = meshBuilder.ToMesh(true), Material = boxBottomMaterial, BackMaterial = boxTopMaterial });
		}

		internal void AddSphere(Point3D location, double radius)
		{
			var meshBuilder = new MeshBuilder();
			meshBuilder.AddSphere(location, radius);

			m_modelGroup.Children.Add(new GeometryModel3D { Geometry = meshBuilder.ToMesh(true), Material = ballMaterial, BackMaterial = ballMaterial });
		}

		public void Clear()
		{
			while (m_modelGroup.Children.Count > 0)
			{
				m_modelGroup.Children.RemoveAt(0);
			}
		}

		/// <summary>
		/// Gets or sets the model.
		/// </summary>
		/// <value>The model.</value>
		public Model3D Model { get; set; }
		private readonly Model3DGroup m_modelGroup = new Model3DGroup();
	}
}