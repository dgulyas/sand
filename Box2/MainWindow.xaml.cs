using System.Collections;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Sand;
using Point = Sand.Point;

namespace Box2
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private const int Size = 100;
		private readonly SandColumn[,] m_sand = SandTable.CreateSandTable(Size, Size, 15);
		private readonly Point m_holeLocation = new Point { X = 15, Y = 15, Height = 15 };
		private readonly int?[,] m_spherePattern = HeightLimitPatternLibrary.Sphere(20);

		Point3DCollection m_corners = new Point3DCollection();
		Int32Collection m_triangles = new Int32Collection();
		Hashtable m_pointIndices = new Hashtable();

		public MainWindow()
		{
			InitializeComponent();
		}


		private void NextButton_OnClick(object sender, RoutedEventArgs e)
		{
			m_holeLocation.X++;
			m_holeLocation.Y++;

			SandTable.ClearHeightLimits(m_sand);
			SandTable.ApplyHeightLimitPattern(m_sand, m_spherePattern, m_holeLocation);
			SandTable.SettleMapTwoPass(m_sand);

		}

		private void AddTop(SandColumn column)
		{
		}

		/*
		 *MeshGeometry3D
			Point3DCollection:Positions: The mesh verticies
			Int32Collection:TriangleIndices: The triangles defined by the ordering of their points.

		GeometryModel3D
			Geometry3D:Geometry: This can be a MeshGeometry3D
			Material:Material: Can be the color the object is colored

		DirectionalLight
			Color:Color: The color the light should be
			Vector3D:Direction: I think the direction that the light should shine?????

		Model3DGroup
			Model3DCollection:Children: Apparently a GeometryModel3D and a DirectionalLight are added to this.
		 */


		private void Canvas1_Loaded(object sender, RoutedEventArgs e)
		{

		}
	}
}
