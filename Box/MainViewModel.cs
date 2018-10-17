//This is based off an example project that came with Helix Toolkit
//Found at https://github.com/helix-toolkit/helix-toolkit/tree/develop/Source/Examples/WPF/SimpleDemo

using System.Windows.Media;
using System.Windows.Media.Media3D;

using HelixToolkit.Wpf;

namespace Box
{
	public class MainViewModel
	{

		public MainViewModel()
		{
			this.Model = modelGroup;

			var meshBuilder = new MeshBuilder(false, false);
			meshBuilder.AddBox(new Rect3D(0, 0, 0, 1, 2, 4));
			var mesh = meshBuilder.ToMesh(true);
			var greenMaterial = MaterialHelper.CreateMaterial(Colors.Green);

			modelGroup.Children.Add(new GeometryModel3D { Geometry = mesh, Material = greenMaterial, BackMaterial = greenMaterial });
		}

		public void AddBox(Sand.Point boxLocation, int xLength, int yLength, int height)
		{
			var meshBuilder = new MeshBuilder(false, false);
			meshBuilder.AddBox(new Rect3D(boxLocation.X, boxLocation.Y, 0, xLength, yLength, height));
			var mesh = meshBuilder.ToMesh(true);

			var material2 = MaterialHelper.CreateMaterial(Colors.Green);

			modelGroup.Children.Add(new GeometryModel3D { Geometry = mesh, Material = material2, BackMaterial = material2 });
		}

		public void Clear()
		{
			while (modelGroup.Children.Count > 0)
			{
				modelGroup.Children.RemoveAt(0);
			}
		}

		private void Reminder()
		{
			// Create a mesh builder and add a box to it
			var meshBuilder = new MeshBuilder(false, false);
			meshBuilder.AddBox(new Point3D(0, 0, 1), 1, 2, 0.5);
			meshBuilder.AddBox(new Rect3D(0, 0, 1.2, 0.5, 1, 0.4));

			// Create a mesh from the builder (and freeze it)
			var mesh = meshBuilder.ToMesh(true);

			// Create some materials
			var greenMaterial = MaterialHelper.CreateMaterial(Colors.Green);
			var redMaterial = MaterialHelper.CreateMaterial(Colors.Red);
			var blueMaterial = MaterialHelper.CreateMaterial(Colors.Blue);
			var insideMaterial = MaterialHelper.CreateMaterial(Colors.Yellow);

			// Add 3 models to the group (using the same mesh, that's why we had to freeze it)
			modelGroup.Children.Add(new GeometryModel3D { Geometry = mesh, Material = greenMaterial, BackMaterial = insideMaterial });
			modelGroup.Children.Add(new GeometryModel3D { Geometry = mesh, Transform = new TranslateTransform3D(-2, 0, 0), Material = redMaterial, BackMaterial = insideMaterial });
			modelGroup.Children.Add(new GeometryModel3D { Geometry = mesh, Transform = new TranslateTransform3D(2, 0, 0), Material = blueMaterial, BackMaterial = insideMaterial });
		}

		/// <summary>
		/// Gets or sets the model.
		/// </summary>
		/// <value>The model.</value>
		public Model3D Model { get; set; }
		private readonly Model3DGroup modelGroup = new Model3DGroup();
	}
}